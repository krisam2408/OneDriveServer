#undef TEST

using GoogleExplorer.DataTransfer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace RetiraUpdater
{
    internal class Program
    {
        private static string m_directoryPath;

        private static BoardManager Board { get; set; }

        private static async Task Main(string[] args)
        {
            Console.Title = "Retira Updater!";
            Board = new();

#if TEST
            m_directoryPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/Retira";
            Directory.CreateDirectory(m_directoryPath);
            Board.AddMessage("Testing Updater");
#else
            m_directoryPath = $"./";
            Console.WriteLine($"Retrieving version {args[0]}.");
#endif
            Board.AddMessage("...");

            FileMetadata[] filesData = await GetFilesAsync();

            List<Task> writeTasks = new();

            foreach (FileMetadata file in filesData)
                writeTasks.Add(Task.Run(async () => 
                { 
                    try
                    {
                        await OverwriteFiles(file);
                    }
                    catch(Exception ex)
                    {
                        Board.AddMessage(ex.Message);
                    }
                }));

            Board.StartLoading();
            await Task.WhenAll(writeTasks);
            Board.StopLoading();
            
            Board.AddMessage("Done!");

#if !TEST
            ReStartApp();
#endif
        }

        private static void ReStartApp()
        {
            Process retiraApp = new();
            retiraApp.StartInfo.UseShellExecute = false;
            retiraApp.StartInfo.RedirectStandardOutput = false;
            retiraApp.StartInfo.FileName = "./RetiraTracker.exe";
            try
            {
                retiraApp.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static async Task<FileMetadata[]> GetFilesAsync()
        {
            Board.StartLoading();
#if TEST
            await ExplorerManager.Instance.LogInAsync();
#else
            await ExplorerManager.Instance.LogInAsync("./");
#endif
            FileMetadata[] retiraFiles = await ExplorerManager.Instance.GetOnlineFolderFilesAsync("Retira");

            Board.AddMessage($"{retiraFiles.Length} files found.");

            List<Task> downloadTask = new();

            foreach(FileMetadata file in retiraFiles)
            {
                downloadTask.Add(Task.Run(async () =>
                {
                    byte[] buffer = await ExplorerManager.Instance.DownloadFile(file);
                    file.FileBuffer = buffer;
                }));
            }

            await Task.WhenAll(downloadTask);

            Board.StopLoading();

            return retiraFiles;
        }

        private static async Task OverwriteFiles(FileMetadata fileData)
        {
#if TEST
            string path = $"{m_directoryPath}/{fileData.Name}";
#else
            string path = $"{m_directoryPath}{fileData.Name}";
#endif
            using FileStream fs = File.Create(path, fileData.BufferSize);
            await fs.WriteAsync(fileData.FileBuffer.AsMemory(0, fileData.BufferSize));
        }
    }
}

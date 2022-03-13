using GoogleExplorer.DataTransfer;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace RetiraUpdater
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine($"Retrieving version {args[0]}.");
            Console.WriteLine("...");
            await GetFilesAsync();
            Console.ReadKey();
            ReStartApp();
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

        private static async Task GetFilesAsync()
        {
            await ExplorerManager.Instance.LogInAsync();
            FileMetadata[] retiraFiles = await ExplorerManager.Instance.GetOnlineFolderFilesAsync("Retira");

            foreach(FileMetadata file in retiraFiles)
                Console.WriteLine(file.Name);
        }

    }
}

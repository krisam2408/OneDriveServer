﻿using RetiraTracker.Core;
using RetiraTracker.Model.Domain;
using GoogleExplorer;
using GoogleExplorer.DataTransfer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace RetiraTracker.Model
{
    public class ExplorerManager
    {
        private Explorer Explorer { get; set; }
        private Timer Timer { get; set; }

        private static ExplorerManager instance;
        public static ExplorerManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new();
                return instance;
            }
        }

        private ExplorerManager() { }

        public async Task DisposeAsync()
        {
            await Explorer.Dispose();
            Explorer = null;

            if (Timer != null)
            {
                Timer.Stop();
                Timer.Dispose();
                Timer = null;
            }

            instance = null;
        }

        public async Task<string> LogInAsync()
        {
            Explorer = await Explorer.CreateAsync("Retira");

            if (Explorer == null)
            {
                return "## Application credetials are missing. Please contact your administrator.";
            }

            RequestResult authTry = await Explorer.Authenticate();

            if (authTry != RequestResult.Success)
            {
                switch (authTry)
                {
                    case RequestResult.Cancelled:
                        return "## Operation canceled.";
                    case RequestResult.TokenExpired:
                        return "## There was an authentication problem. Please try again.";
                    default:
                        return "## 100";
                }
            }

            return Explorer.UserMail;
        }

        public void CancelRequest()
        {
            Explorer.CancelRequest();
        }

        public async Task<ListItem[]> GetSettingsAsync()
        {
            FileMetadata[] metadata = await Explorer.GetAllFilesAsync("retiraSettings.json");

            if (metadata.Length == 0)
            {
                FileMetadata settingMetadata = await CreateOwnSetting();
                metadata = new FileMetadata[1] { settingMetadata };
            }

            List<Settings> settings = new();
            foreach (FileMetadata meta in metadata)
            {
                byte[] fileBuffer = await Explorer.DownloadFileAsync(meta.ID);

                if (fileBuffer != null)
                {
                    string json = ReadFileText(fileBuffer, meta.MimeType);
                    Settings settingItem = JsonConvert.DeserializeObject<Settings>(json);
                    settings.Add(settingItem);
                }

            }

            List<ListItem> output = new();
            int i = 0;
            foreach (Settings set in settings)
            {
                ListItem li = new(i.ToString(), $"Settings from {set.Owner}");
                li.SetContent(set);
                output.Add(li);
                i++;
            }

            return output.ToArray();
        }

        public async Task<FileMetadata> CreateOwnSetting()
        {
            FileMetadata folderMetadata = await Explorer.CreateFolderAsync(Explorer.CoreFolder, null);
            Settings setting = new()
            {
                Owner = Explorer.UserMail,
                FolderId = folderMetadata.ID
            };
            string settingJson = JsonConvert.SerializeObject(setting);
            byte[] settingBuffer = Encoding.UTF8.GetBytes(settingJson);
            FileMetadata settingMetadata = await Explorer.UploadFileAsync("retiraSettings.json", folderMetadata.ID, settingBuffer, MimeTypes.Text);
            return settingMetadata;
        }

        public async Task UpdateSettingsAsync(Settings settings)
        {
            FileMetadata settingMeta = new()
            {
                Name = "retiraSettings.json",
                MimeType = MimeTypes.Text,
                ParentFolder = new string[] { settings.FolderId }
            };

            FileMetadata[] metadata = await Explorer.GetAllFilesAsync("retiraSettings.json");

            foreach (FileMetadata mtdt in metadata)
                if (mtdt.ParentFolder.Contains(settings.FolderId))
                    settingMeta.ID = mtdt.ID;

            string settingJson = JsonConvert.SerializeObject(settings);
            byte[] settingBuffer = Encoding.UTF8.GetBytes(settingJson);

            FileMetadata newSettingsMetadata = await Explorer.OverwriteFileByIdAsync(settingMeta.ID, settingBuffer, MimeTypes.Text);

            foreach (Campaign c in settings.Campaigns)
                foreach (string p in c.Players)
                    await Explorer.ShareFile(newSettingsMetadata.ID, p);
        }

        public async Task<string> CreateFolderAsync(string folderName, string settingFolder)
        {
            FileMetadata folder = await Explorer.CreateFolderAsync(folderName, settingFolder);

            return folder.ID;
        }

        public async Task ShareFolderAsync(string folderId, Player player)
        {
            string playerJson = JsonConvert.SerializeObject(player);
            byte[] buffer = Encoding.UTF8.GetBytes(playerJson);

            string filename = player.EmailAddress.Split('@')[0];

            FileMetadata metadata = await Explorer.UploadFileAsync($"{filename}.json", folderId, buffer, MimeTypes.Text);
            await Explorer.ShareFile(metadata.ID, player.EmailAddress, Permissions.Writer);
        }

        public async Task<string> GetPlayerAsync(string fileName, string folderId)
        {
            FileMetadata metadata = await Explorer.GetFileMetaDataAsync(fileName, folderId);

            byte[] fileBuffer = await Explorer.DownloadFileAsync(metadata.ID);

            string output = ReadFileText(fileBuffer, metadata.MimeType);

            return output;
        }

        public async Task<bool> UpdatePlayerAsync(string filename, string folderId, Player player)
        {
            try
            {
                string playerJson = JsonConvert.SerializeObject(player);
                byte[] buffer = Encoding.UTF8.GetBytes(playerJson);

                await Explorer.OverwriteFileByNameAsync(filename, folderId, buffer, MimeTypes.Text);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private static string ReadFileText(byte[] buffer, MimeTypes mimeType)
        {
            switch(mimeType)
            {
                case MimeTypes.Text:
                    return Encoding.UTF8.GetString(buffer);
                case MimeTypes.OfficeWord:
                    string text = Encoding.UTF8.GetString(buffer);
                    return text;
                default:
                    throw new FormatException("Not a text format file.");
            }
        }
    }
}

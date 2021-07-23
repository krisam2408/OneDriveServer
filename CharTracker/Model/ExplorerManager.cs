using CharTracker.Core;
using CharTracker.Model.Domain;
using GoogleExplorer;
using GoogleExplorer.DataTransfer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace CharTracker.Model
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

        public async Task Dispose()
        {
            await Explorer.Dispose();
            Explorer = null;

            Timer.Stop();
            Timer.Dispose();
            Timer = null;

            instance = null;
        }

        public async Task<string> LogIn()
        {
            Explorer = await Explorer.CreateAsync("Retira");

            return Explorer.UserMail;
        }

        public async Task<ListItem[]> GetSettings()
        {
            FileMetadata[] metadata = await Explorer.GetAllFilesAsync("retiraSettings.json");

            if(metadata.Length == 0)
            {
                FileMetadata folderMetadata = await Explorer.CreateFolderAsync(Explorer.CoreFolder);
                Settings setting = new()
                {
                    Owner = Explorer.UserMail,
                    FolderId = folderMetadata.ID
                };
                string settingJson = JsonConvert.SerializeObject(setting);
                byte[] settingBuffer = Encoding.UTF8.GetBytes(settingJson);
                FileMetadata settingMetadata = await Explorer.UploadFileAsync("retiraSettings.json", folderMetadata.ID, settingBuffer, MimeTypes.Text);
                metadata = new FileMetadata[1] { settingMetadata };
            }

            List<Settings> settings = new();
            foreach (FileMetadata meta in metadata)
            {
                byte[] fileBuffer = await Explorer.DownloadFileAsync(meta.ID, MimeTypes.Text);
                string json = Encoding.UTF8.GetString(fileBuffer);
                Settings settingItem = JsonConvert.DeserializeObject<Settings>(json);
                settings.Add(settingItem);
            }

            List<ListItem> output = new();
            int i = 0;
            foreach(Settings set in settings)
            {
                ListItem li = new(i.ToString(), $"Settings from {set.Owner}");
                li.SetContent(set);
                output.Add(li);
                i++;
            }

            return output.ToArray();
        }

        private async Task Refresh(object sender, ElapsedEventArgs e)
        {

        }


    }
}

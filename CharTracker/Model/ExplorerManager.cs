using GoogleExplorer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;
using GFile = Google.Apis.Drive.v3.Data.File;

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

        public async Task GetFolder()
        {
            string coreFolder = Explorer.CoreFolder;
            GFile gCore = await Explorer.GetFile(coreFolder, MimeTypes.GoogleFolder);

            if(gCore == null)
            {
                await Explorer.CreateFolder(coreFolder);
            }
        }

        private async Task Refresh(object sender, ElapsedEventArgs e)
        {

        }


    }
}

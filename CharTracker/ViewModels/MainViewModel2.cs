using CharTracker.Core;
using CharTracker.Core.Abstracts;
using GoogleExplorer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using Timer = System.Timers.Timer;

namespace CharTracker.ViewModel
{
    public class MainViewModel2:BaseViewModel,INavegable
    {
        private Explorer Explorer { get; set; }
        private Timer Timer { get; set; }

        private string updateText;
        public string UpdateText { get { return updateText; } set { SetValue(ref updateText, value); } }

        private string readText;
        public string ReadText { get { return readText; } set { SetValue(ref readText, value); } }

        private bool isEnabled;
        public new bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                if (SetValue(ref isEnabled, value))
                {
                    NotifyPropertyChanged(nameof(IsLogued));
                }
            }
        }

        private bool isLogued;
        public bool IsLogued
        {
            get
            {
                if (!IsEnabled)
                    return false;
                return isLogued;
            }
            set { SetValue(ref isLogued, value); }
        }

        public ICommand LogInCommand { get { return new RelayCommand(async (s) => { await LogInAsync(); }); } }

        public ICommand LogOutCommand { get { return new RelayCommand(async (s) => { await LogOutAsync(); }); } }

        public ICommand UpdateCommand { get { return new RelayCommand(async (s) => { await UpdateAsync(); }); } }

        public MainViewModel2()
        {
            IsLogued = false;
            IsEnabled = true;
        }

        private async Task GetValue()
        {
            try
            {
                byte[] fileBuffer = await Explorer.DownloadFile("test002", "Retira", MimeTypes.Text);
                string fileText = Encoding.UTF8.GetString(fileBuffer);
                ReadText = fileText;
            }
            catch(Exception ex)
            {
                StringBuilder builder = new();
                builder.AppendLine("Error:");
                builder.AppendLine(ex.GetType().ToString());
                builder.AppendLine(ex.Message);
                builder.AppendLine(ex.StackTrace);
                if(ex.InnerException != null)
                {
                    builder.AppendLine("InnerException:");
                    builder.AppendLine(ex.InnerException.GetType().ToString());
                    builder.AppendLine(ex.InnerException.Message);
                    builder.AppendLine(ex.InnerException.StackTrace);
                }

                ReadText = builder.ToString();
            }
        }

        private async Task LogInAsync()
        {
            if(IsEnabled)
            {
                IsEnabled = false;

                Explorer = await Explorer.CreateAsync("Retira");
                await GetValue();

                Timer = new(5000);
                Timer.Elapsed += async (s, e) => await TimerElapsedAsync(s, e);
                Timer.Start();

                IsLogued = true;
                IsEnabled = true;
            }
        }

        private async Task LogOutAsync()
        {
            if (IsEnabled)
            {
                IsEnabled = false;

                await Explorer.Dispose();
                Explorer = null;

                Timer.Stop();
                Timer.Dispose();
                Timer = null;

                ReadText = string.Empty;
                IsLogued = false;
                IsEnabled = true;
            }
        }

        private async Task UpdateAsync()
        {
            if (IsEnabled)
            {
                IsEnabled = false;

                if(!string.IsNullOrWhiteSpace(UpdateText))
                {
                    string nText = UpdateText;
                    UpdateText = string.Empty;

                    byte[] fileBuffer = Encoding.UTF8.GetBytes(nText);
                    await Explorer.OverwriteFile("test002", "Retira", fileBuffer, MimeTypes.Text);
                }

                IsEnabled = true;
            }
        }

        private async Task TimerElapsedAsync(object sender, ElapsedEventArgs e)
        {
            await GetValue();
        }
    }
}

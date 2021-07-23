using CharTracker.Core;
using CharTracker.Core.Abstracts;
using CharTracker.Model;
using CharTracker.Model.Domain;
using GoogleExplorer.DataTransfer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CharTracker.ViewModels
{
    public class MainViewModel:BaseViewModel
    {
        private ObservableCollection<ListItem> settingsList;
        public ObservableCollection<ListItem> SettingsList { get { return settingsList; } set { SetValue(ref settingsList, value); } }

        private ListItem selectedSetting;
        public ListItem SelectedSetting 
        { 
            get { return selectedSetting; } 
            set 
            { 
                SetValue(ref selectedSetting, value);
                if(value != null)
                    SelectCampaigns();
            } 
        }

        private ObservableCollection<ListItem> campaigns;
        public ObservableCollection<ListItem> Campaigns { get { return campaigns; } set { SetValue(ref campaigns, value); } }

        private ListItem selectedCampaign;
        public ListItem SelectedCampaign 
        { 
            get { return selectedCampaign; } 
            set 
            {
                SetValue(ref selectedCampaign, value); 
            
            }
        }

        public ICommand LogInCommand
        {
            get
            {
                return new RelayCommand(async (s) =>
                {
                    if(Terminal.IsEnabled)
                    {
                        Terminal.IsEnabled = false;
                        await LogIn();
                        Terminal.IsEnabled = true;
                    }
                });
            }
        }

        private async Task LogIn()
        {
            Terminal.Instance.Navigation.IsLoading(true);

            Terminal.Instance.Navigation.UserMail = await ExplorerManager.Instance.LogIn();
            ListItem[] settings = await ExplorerManager.Instance.GetSettings();

            SettingsList = settings.ToObservableCollection();
            
            Terminal.Instance.Navigation.Navigation(NavigationViewModel.Pages.Settings);
            Terminal.Instance.Navigation.IsMenuVisible(true);
            Terminal.Instance.Navigation.IsLoading(false);
            return;
        }

        private void SelectCampaigns()
        {
            Settings setting = SelectedSetting.GetContent<Settings>();
            List<Campaign> campaigns = new();

            if(setting.Campaigns != null && setting.Campaigns.Length > 0)
                campaigns.AddRange(setting.Campaigns);

            List<ListItem> output = new();
            int i = 0;
            foreach(Campaign c in campaigns)
            {
                ListItem li = new(i.ToString(), c.Name);
                li.SetContent(c);
                output.Add(li);
                i++;
            }

            Campaigns = output.ToObservableCollection();
            Terminal.Instance.Navigation.Navigation(NavigationViewModel.Pages.Campaigns);
        }
    }
}

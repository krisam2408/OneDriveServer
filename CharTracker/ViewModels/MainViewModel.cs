using RetiraTracker.Core;
using RetiraTracker.Core.Abstracts;
using RetiraTracker.Extensions;
using RetiraTracker.Model;
using RetiraTracker.Model.Domain;
using RetiraTracker.View.Popups;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RetiraTracker.ViewModels
{
    public class MainViewModel:BaseViewModel
    {
        private ObservableCollection<ListItem> m_settingsList;
        public ObservableCollection<ListItem> SettingsList { get { return m_settingsList; } set { SetValue(ref m_settingsList, value); } }

        private ListItem m_selectedSetting;
        public ListItem SelectedSetting 
        { 
            get { return m_selectedSetting; } 
            set
            { 
                SetValue(ref m_selectedSetting, value);
                if(value != null)
                    SelectCampaigns();
            } 
        }

        private ObservableCollection<ListItem> m_campaignsList;
        public ObservableCollection<ListItem> CampaignsList { get { return m_campaignsList; } set { SetValue(ref m_campaignsList, value); } }

        private ListItem m_selectedCampaign;
        public ListItem SelectedCampaign 
        {
            get { return m_selectedCampaign; } 
            set 
            {
                if (SetValue(ref m_selectedCampaign, value))
                    GoEnabled = true;
            }
        }

        public new bool IsEnabled
        {
            get { return Terminal.IsEnabled; }
            set 
            {
                Terminal.IsEnabled = value;
                NotifyPropertyChanged(nameof(GoEnabled));
            }
        }

        private bool m_goEnabled;
        public bool GoEnabled
        {
            get
            {
                if (!IsEnabled)
                    return false;
                return m_goEnabled;
            }
            set { SetValue(ref m_goEnabled, value); }
        }

        private Visibility m_cancelButtonVisibility;
        public Visibility CancelButtonVisibility { get { return m_cancelButtonVisibility; } set { SetValue(ref m_cancelButtonVisibility, value); } }

        public ICommand LogInCommand
        {
            get
            {
                return new RelayCommand(async (e) =>
                {
                    if(IsEnabled)
                    {
                        IsEnabled = false;
                        await LogIn();
                        IsEnabled = true;
                    }
                });
            }
        }

        public ICommand CancelLogInCommand { get { return new RelayCommand((e) => { CancelLogIn(); }); } }

        public ICommand GoToCreateCampaignCommand
        {
            get
            {
                return new RelayCommand(async (e) =>
                {
                    if(Terminal.IsEnabled)
                    {
                        Terminal.Instance.Navigation.IsLoading(true);

                        await Terminal.Instance.Navigation.Navigation(NavigationViewModel.Pages.CreateCampaign);

                        Terminal.Instance.Navigation.IsLoading(false);
                    }
                });
            }
        }

        public ICommand GoToSelectedCampaign
        {
            get
            {
                return new RelayCommand(async (e) =>
                {
                    if(Terminal.IsEnabled && SelectedCampaign != null)
                    {
                        Terminal.Instance.Navigation.IsLoading(true);

                        Campaign selectedCampaign = SelectedCampaign.GetContent<Campaign>();
                        Terminal.Instance.Campaign = await CampaignViewModel.CreateAsync(selectedCampaign);
                        await Terminal.Instance.Navigation.Navigation(NavigationViewModel.Pages.Campaign);

                        Terminal.Instance.Navigation.IsLoading(false);
                    }
                });
            }
        }

        public ICommand CreateSettingCommand
        {
            get
            {
                return new RelayCommand(async (e) =>
                {
                    if(Terminal.IsEnabled)
                    {
                        Terminal.Instance.Navigation.IsLoading(true);

                        await CreateSettingAsync();
                    }
                });
            }
        }

        public MainViewModel()
        {
            CancelButtonVisibility = Visibility.Hidden;
        }

        private async Task LogIn()
        {
            Terminal.Instance.Navigation.IsLoading(true);
            CancelButtonVisibility = Visibility.Visible;

            string msg = await ExplorerManager.Instance.LogInAsync();

            if(msg.Contains("##"))
            {
                CancelButtonVisibility = Visibility.Hidden;
                
                msg = msg.Replace("## ", string.Empty);
                InfoPopup alert = new(msg);
                alert.Show();
                return;
            }

            Terminal.Instance.Navigation.UserMail = msg;

            await Terminal.Instance.Navigation.Navigation(NavigationViewModel.Pages.Settings);

            CancelButtonVisibility = Visibility.Hidden;
            Terminal.Instance.Navigation.IsMenuVisible(true);
            Terminal.Instance.Navigation.IsLoading(false);
        }

        private void CancelLogIn()
        {
            ExplorerManager.Instance.CancelRequest();
            CancelButtonVisibility = Visibility.Hidden;
            Terminal.Instance.Navigation.IsMenuVisible(true);
            Terminal.Instance.Navigation.IsLoading(false);
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
                ListItem li = new(i.ToString(), $"{c.Name} narrated by {c.Narrator}");
                li.SetContent(c);
                output.Add(li);
                i++;
            }

            CampaignsList = output.ToObservableCollection();
            Terminal.Instance.Navigation.Navigation(NavigationViewModel.Pages.Campaigns);
        }

        private async Task CreateSettingAsync()
        {
            bool ownSetting = false;
            foreach(ListItem li in SettingsList)
            {
                Settings setting = li.GetContent<Settings>();
                if(setting.Owner == Terminal.Instance.Navigation.UserMail)
                {
                    ownSetting = true;
                    break;
                }
            }

            if(ownSetting)
            {
                InfoPopup alert = new("You already own a Setting!");
                alert.Show();
                return;
            }

            await ExplorerManager.Instance.CreateOwnSetting();

            await Terminal.Instance.Navigation.Navigation(NavigationViewModel.Pages.Settings);
        }
    }
}

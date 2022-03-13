using Newtonsoft.Json;
using RetiraTracker.Core;
using RetiraTracker.Core.Abstracts;
using RetiraTracker.Extensions;
using RetiraTracker.Model;
using RetiraTracker.Model.DataTransfer;
using RetiraTracker.View.Popups;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RetiraTracker.ViewModels
{
    public class NavigationViewModel : BaseViewModel
    {
        private string m_frameDestination;
        public string FrameDestination { get { return m_frameDestination; } set { SetValue(ref m_frameDestination, value); } }

        private string m_userMail;
        public string UserMail { get { return m_userMail; } set { SetValue(ref m_userMail, value); } }

        public ICommand AppCloseCommand { get { return new RelayCommand(e => AppClose()); } }
        public ICommand AppMinimizeCommand { get { return new RelayCommand(e => AppMinimize()); } }
        public ICommand AppMaximizeCommand { get { return new RelayCommand(e => AppMaximize()); } }
        public ICommand AppMoveDownCommand { get { return new RelayCommand(e => AppMove((MouseEventArgs)e)); } }
        public ICommand AppHomeCommand { get { return new RelayCommand(async (e) => await AppHomeAsync()); } }
        public ICommand SignOutCommand { get { return new RelayCommand(async (e) => await SignOutAsync()); } }
        public ICommand UpdateCommand { get { return new RelayCommand(async (e) => await CheckLatestVersionAsync()); } }

        private Visibility m_menuVisible;
        public Visibility MenuVisibility { get { return m_menuVisible; } set { SetValue(ref m_menuVisible, value); } }

        private Visibility m_indicatorVisible;
        public Visibility IndicatorVisible { get { return m_indicatorVisible; } set { SetValue(ref m_indicatorVisible, value); } }

        private Visibility m_updaterVisible;
        public Visibility UpdaterVisible { get { return m_updaterVisible; } set { SetValue(ref m_updaterVisible, value); } }

        private bool m_isEnabled;
        public new bool IsEnabled
        {
            get { return m_isEnabled; }
            set
            {
                if(SetValue(ref m_isEnabled, value))
                {
                    if(!Terminal.Instance.Campaign.IsNull())
                        Terminal.Instance.Campaign.IsEnabled = value;

                    if(!Terminal.Instance.CreateCampaign.IsNull())
                        Terminal.Instance.CreateCampaign.IsEnabled = value;

                    if(!Terminal.Instance.Main.IsNull())
                        Terminal.Instance.Main.IsEnabled = value;

                }
            }
        }

        public NavigationViewModel()
        {
            Navigation(Pages.LogIn);
            IsMenuVisible(false);
            IsLoading(false);
        }

        public async Task Navigation(Pages page)
        {
            switch(page)
            {
                case Pages.Settings:
                    
                    IsLoading(true);

                    ListItem[] settings = await ExplorerManager.Instance.GetSettingsAsync();
                    Terminal.Instance.Main.SettingsList = settings.ToObservableCollection();
                    Terminal.Instance.Main.SelectedSetting = null;

                    IsLoading(false);

                    FrameDestination = "View/SettingsPage.xaml";
                    break;
                case Pages.Campaigns:
                    FrameDestination = "View/CampaignsPage.xaml";
                    break;
                case Pages.Campaign:
                    FrameDestination = "View/CampaignPage.xaml";
                    break;
                case Pages.CreateCampaign:
                    Terminal.Instance.CreateCampaign = new();
                    FrameDestination = "View/CreateCampaignPage.xaml";
                    break;
                default:
                    FrameDestination = "View/LogInPage.xaml";
                    break;
            }
        }

        public void IsMenuVisible(bool isVisible)
        {
            MenuVisibility = isVisible ? Visibility.Visible : Visibility.Hidden;
            UpdaterVisible = isVisible ? Visibility.Visible : Visibility.Hidden;
        }

        public void IsLoading(bool isVisible)
        {
            IsEnabled = !isVisible;
            IndicatorVisible = isVisible ? Visibility.Visible : Visibility.Hidden;
        }

        private static void AppClose()
        {
            Application.Current.Shutdown();
        }

        private static void AppMinimize()
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private static void AppMaximize()
        {
            if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
                return;
            }

            Application.Current.MainWindow.MaxHeight = GetCurrentScreenWorkAreaHeight();
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
        }

        private static void AppMove(MouseEventArgs e)
        {
            if(Application.Current.MainWindow.WindowState == WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
                Application.Current.MainWindow.Top = e.GetPosition((IInputElement)e.Source).Y;
            }

            if (e.LeftButton == MouseButtonState.Pressed)
                Application.Current.MainWindow.DragMove();
        }

        private async Task AppHomeAsync()
        {
            if (m_menuVisible == Visibility.Hidden)
            {
                await Navigation(Pages.LogIn);
                return;
            }

            await Navigation(Pages.Settings);
        }

        private async Task SignOutAsync()
        {
            await ExplorerManager.Instance.DisposeAsync();
            IsMenuVisible(false);
            await Navigation(Pages.LogIn);
        }

        private async Task CheckLatestVersionAsync()
        {
            IsLoading(true);

            AppVersion local = await GetLocalAppVersion();
            AppVersion online = await GetOnlineAppVersion();

            if(local < online)
            {
                ConfirmUpdatePopup updateAlert = new(online);
                updateAlert.Show();
                return;
            }

            InfoPopup alert = new("There is no application updates available.");
            alert.Show();
        }

        private async Task<AppVersion> GetLocalAppVersion()
        {
            string json = await File.ReadAllTextAsync("retiraVersion.json");
            AppVersion appVersion = JsonConvert.DeserializeObject<AppVersion>(json);
            return appVersion;
        }

        private async Task<AppVersion> GetOnlineAppVersion()
        {
            string json = await ExplorerManager.Instance.GetOnlineVersionAsync();
            AppVersion appVersion = JsonConvert.DeserializeObject<AppVersion>(json);
            return appVersion;
        }

        private static double GetCurrentScreenWorkAreaHeight()
        {
            return SystemParameters.MaximizedPrimaryScreenHeight;
        }

        public enum Pages
        {
            LogIn, Settings, Campaigns, CreateCampaign, Campaign
        }
    }
}

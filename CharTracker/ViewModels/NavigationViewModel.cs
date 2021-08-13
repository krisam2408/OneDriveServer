using RetiraTracker.Core;
using RetiraTracker.Core.Abstracts;
using RetiraTracker.Model;
using ColorRoseLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MouseAction = RetiraTracker.Core.MouseAction;

namespace RetiraTracker.ViewModels
{
    public class NavigationViewModel:BaseViewModel
    {
        private string frameDestination;
        public string FrameDestination { get { return frameDestination; } set { SetValue(ref frameDestination, value); } }

        private string userMail;
        public string UserMail { get { return userMail; } set { SetValue(ref userMail, value); } }

        public ICommand AppCloseCommand { get { return new RelayCommand(e => AppClose()); } }
        public ICommand AppMinimizeCommand { get { return new RelayCommand(e => AppMinimize()); } }
        public ICommand AppMaximizeCommand { get { return new RelayCommand(e => AppMaximize()); } }
        public ICommand AppMoveDownCommand { get { return new RelayCommand(e => AppMove((MouseEventArgs)e)); } }
        public ICommand AppHomeCommand { get { return new RelayCommand(async (e) => await AppHomeAsync()); } }
        public ICommand SignOutCommand { get { return new RelayCommand(async (e) => await SignOut()); } }

        private Visibility menuVisible;
        public Visibility MenuVisibility { get { return menuVisible; } set { SetValue(ref menuVisible, value); } }

        private Visibility indicatorVisible;
        public Visibility IndicatorVisible { get { return indicatorVisible; } set { SetValue(ref indicatorVisible, value); } }

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
                    IsEnabled = false;
                    IsLoading(true);

                    ListItem[] settings = await ExplorerManager.Instance.GetSettingsAsync();
                    Terminal.Instance.Main.SettingsList = settings.ToObservableCollection();
                    Terminal.Instance.Main.SelectedSetting = null;

                    IsLoading(false);
                    IsEnabled = true;

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
        }

        public void IsLoading(bool isVisible)
        {
            IndicatorVisible = isVisible ? Visibility.Visible : Visibility.Hidden;
        }

        private void AppClose()
        {
            Application.Current.Shutdown();
        }

        private void AppMinimize()
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void AppMaximize()
        {
            if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
                return;
            }

            Application.Current.MainWindow.WindowState = WindowState.Maximized;
        }

        private void AppMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                Application.Current.MainWindow.DragMove();
        }

        private async Task AppHomeAsync()
        {
            if (menuVisible == Visibility.Hidden)
            {
                await Navigation(Pages.LogIn);
                return;
            }

            await Navigation(Pages.Settings);
        }

        private async Task SignOut()
        {
            await ExplorerManager.Instance.DisposeAsync();
            IsMenuVisible(false);
            await Navigation(Pages.LogIn);
        }

        public enum Pages
        {
            LogIn, Settings, Campaigns, CreateCampaign, Campaign
        }
    }
}

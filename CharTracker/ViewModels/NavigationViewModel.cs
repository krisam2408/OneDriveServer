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

        private bool IsWindowBeingDragged { get; set; }
        private Point WindowPosition { get; set; }

        public ICommand AppCloseCommand { get { return new RelayCommand(e => AppClose()); } }
        public ICommand AppMinimizeCommand { get { return new RelayCommand(e => AppMinimize()); } }
        public ICommand AppMaximizeCommand { get { return new RelayCommand(e => AppMaximize()); } }
        public ICommand AppMoveUpCommand { get { return new RelayCommand(e => AppMove(MouseAction.Up, (MouseEventArgs)e)); } }
        public ICommand AppMoveDownCommand { get { return new RelayCommand(e => AppMove(MouseAction.Down, (MouseEventArgs)e)); } }
        public ICommand AppMoveDragCommand { get { return new RelayCommand(e => AppMove(MouseAction.Drag, (MouseEventArgs)e)); } }
        public ICommand AppMoveLeaveCommand { get { return new RelayCommand(e => AppMove(MouseAction.Leave, (MouseEventArgs)e)); } }
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

        private void AppMove(MouseAction action, MouseEventArgs e)
        {
            Point mousePosition = e.GetPosition(Application.Current.MainWindow);
            if (action == MouseAction.Down)
            {
                IsWindowBeingDragged = true;
                WindowPosition = mousePosition;
            }

#if !TEMP
            if (action == MouseAction.Up || action == MouseAction.Leave)
                IsWindowBeingDragged = false;
#else
            if (action == MouseAction.Up)
                IsWindowBeingDragged = false;
#endif

            if (action == MouseAction.Drag && e.MouseDevice.LeftButton == MouseButtonState.Released)
                IsWindowBeingDragged = false;

#if TEMP
            if (action == MouseAction.Leave && IsWindowBeingDragged)
            {
                int wasUp = mousePosition.Y < WindowPosition.Y ? 1 : -1;
                Application.Current.MainWindow.Left += mousePosition.X - WindowPosition.X;
                Application.Current.MainWindow.Top = mousePosition.Y + (WindowPosition.Y * wasUp);
            }
#endif
            
            if (action == MouseAction.Drag && IsWindowBeingDragged)
            {
                Application.Current.MainWindow.Left += mousePosition.X - WindowPosition.X;
                Application.Current.MainWindow.Top += mousePosition.Y - WindowPosition.Y;
            }
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

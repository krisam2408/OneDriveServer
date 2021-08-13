﻿using RetiraTracker.Core;
using RetiraTracker.Core.Abstracts;
using RetiraTracker.Model;
using RetiraTracker.Model.Domain;
using RetiraTracker.View.Popups;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RetiraTracker.ViewModels
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

        private ObservableCollection<ListItem> campaignsList;
        public ObservableCollection<ListItem> CampaignsList { get { return campaignsList; } set { SetValue(ref campaignsList, value); } }

        private ListItem selectedCampaign;
        public ListItem SelectedCampaign 
        {
            get { return selectedCampaign; } 
            set 
            {
                if (SetValue(ref selectedCampaign, value))
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

        private bool goEnabled;
        public bool GoEnabled
        {
            get
            {
                if (!IsEnabled)
                    return false;
                return goEnabled;
            }
            set { SetValue(ref goEnabled, value); }
        }

        public ICommand LogInCommand
        {
            get
            {
                return new RelayCommand(async (e) =>
                {
                    if(Terminal.IsEnabled)
                    {
                        IsEnabled = false;
                        await LogIn();
                        IsEnabled = true;
                    }
                });
            }
        }

        public ICommand GoToCreateCampaignCommand
        {
            get
            {
                return new RelayCommand(async (e) =>
                {
                    if(Terminal.IsEnabled)
                    {
                        IsEnabled = false;
                        Terminal.Instance.Navigation.IsLoading(true);

                        await Terminal.Instance.Navigation.Navigation(NavigationViewModel.Pages.CreateCampaign);

                        Terminal.Instance.Navigation.IsLoading(false);
                        IsEnabled = true;
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
                        IsEnabled = false;
                        Terminal.Instance.Navigation.IsLoading(true);

                        Campaign selectedCampaign = SelectedCampaign.GetContent<Campaign>();
                        Terminal.Instance.Campaign = await CampaignViewModel.CreateAsync(selectedCampaign);
                        await Terminal.Instance.Navigation.Navigation(NavigationViewModel.Pages.Campaign);

                        Terminal.Instance.Navigation.IsLoading(false);
                        IsEnabled = true;
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
                        IsEnabled = false;
                        Terminal.Instance.Navigation.IsLoading(true);

                        await CreateSettingAsync();
                    }
                });
            }
        }

        private async Task LogIn()
        {
            Terminal.Instance.Navigation.IsLoading(true);

            Terminal.Instance.Navigation.UserMail = await ExplorerManager.Instance.LogInAsync();
            
            await Terminal.Instance.Navigation.Navigation(NavigationViewModel.Pages.Settings);
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
                Terminal.Instance.Navigation.IsLoading(false);
                InfoPopup alert = new InfoPopup("You already own a Setting!");
                alert.Show();
                return;
            }

            await ExplorerManager.Instance.CreateOwnSetting();

            await Terminal.Instance.Navigation.Navigation(NavigationViewModel.Pages.Settings);
        }
    }
}

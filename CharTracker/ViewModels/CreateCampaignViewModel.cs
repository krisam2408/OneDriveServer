using CharTracker.Core;
using CharTracker.Core.Abstracts;
using CharTracker.Model;
using CharTracker.Model.Domain;
using RPGTemplates;
using RPGTemplates.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CharTracker.ViewModels
{
    public class CreateCampaignViewModel : BaseViewModel
    {
        private ObservableCollection<ListItem> gamesList;
        public ObservableCollection<ListItem> GamesList { get { return gamesList; } set { SetValue(ref gamesList, value); } }

        private ListItem selectedGame;
        public ListItem SelectedGame 
        { 
            get { return selectedGame; } 
            set 
            { 
                SetValue(ref selectedGame, value); 
            } 
        }

        private ObservableCollection<ListItem> templatesList;
        public ObservableCollection<ListItem> TemplatesList { get { return templatesList; } set { SetValue(ref templatesList, value); } }

        private ListItem selectedTemplate;
        public ListItem SelectedTemplate
        {
            get { return selectedTemplate; }
            set
            {
                SetValue(ref selectedTemplate, value);
            }
        }

        private string campaignsName;
        public string CampaignsName { get { return campaignsName; } set { SetValue(ref campaignsName, value); } }

        private string playersEmail;
        public string PlayersEmail { get { return playersEmail; } set { SetValue(ref playersEmail, value); } }

        private Visibility campaignWarningVisibility;
        public Visibility CampaignWarningVisibility { get { return campaignWarningVisibility; } set { SetValue(ref campaignWarningVisibility, value); } }

        private Visibility emailWarningVisibility;
        public Visibility EmailWarningVisibility { get { return emailWarningVisibility; } set { SetValue(ref emailWarningVisibility, value); } }

        private bool playerEnabled;
        public bool PlayerEnabled { get { return playerEnabled; } set { SetValue(ref playerEnabled, value); } }

        public ICommand ValidateCampaignNameCommand  { get { return new RelayCommand((e) => { ValidateCampaignName((TextChangedEventArgs)e); }); } }
        public ICommand ValidateEmailCommand { get { return new RelayCommand((e) => { ValidateEmail((TextChangedEventArgs)e); }); } }
        public ICommand AddPlayerCommand { get { return new RelayCommand((e) => { AddPlayer(); }); } }

        public CreateCampaignViewModel()
        {
            GamesList = SetGamesList();
            CampaignWarningVisibility = Visibility.Hidden;
            EmailWarningVisibility = Visibility.Hidden;
        }

        private ObservableCollection<ListItem> SetGamesList()
        {
            List<ListItem> output = new();
            foreach(Games g in Enum.GetValues(typeof(Games)))
            {
                ListItem li = new(g.ToString(), g.WriteEnum());
                li.SetContent(g);
                output.Add(li);
            }

            return output.ToObservableCollection();
        }

        private void ValidateCampaignName(TextChangedEventArgs e)
        {
            Settings setting = Terminal.Instance.Main.SelectedSetting.GetContent<Settings>();
            CampaignWarningVisibility = Visibility.Hidden;

            if(setting.Campaigns != null)
            {
                foreach(Campaign cmp in setting.Campaigns)
                {
                    TextBox sender = (TextBox)e.OriginalSource;
                    string senderValue = sender.Text;
                    if(cmp.Name.ToUpper() == senderValue.ToUpper())
                    {
                        CampaignWarningVisibility = Visibility.Visible;
                        return;
                    }
                }
            }
        }

        private void ValidateEmail(TextChangedEventArgs e)
        {
            
        }

        private void AddPlayer()
        {

        }
    }
}

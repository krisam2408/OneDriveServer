using RetiraTracker.Core;
using RetiraTracker.Core.Abstracts;
using RetiraTracker.Model;
using RetiraTracker.Model.Domain;
using Newtonsoft.Json;
using SheetDrama;
using SheetDrama.Abstracts;
using SheetDrama.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;

namespace RetiraTracker.ViewModels
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
                PlayerEnabled = false;
                SetValue(ref selectedGame, value);
                if (value != null)
                {
                    Games g = value.GetContent<Games>();
                    TemplatesList = SetTemplatesList(g);

                    if (!string.IsNullOrWhiteSpace(CampaignsName) && validCampaign)
                        PlayerEnabled = true;
                }
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
                AddPlayerEnabled = false;
                SetValue(ref selectedTemplate, value);
                if (value != null && !string.IsNullOrWhiteSpace(PlayersEmail) && validPlayer)
                    AddPlayerEnabled = true;
            }
        }

        private ObservableCollection<Player> playersList;
        public ObservableCollection<Player> PlayersList { get { return playersList; } set { SetValue(ref playersList, value); } }

        private string campaignsName;
        public string CampaignsName 
        { 
            get { return campaignsName; } 
            set 
            {
                PlayerEnabled = false;
                SetValue(ref campaignsName, value);
                if (!string.IsNullOrWhiteSpace(value) && SelectedGame != null && validCampaign)
                    PlayerEnabled = true;
            } 
        }

        private string playersEmail;
        public string PlayersEmail 
        { 
            get { return playersEmail; } 
            set 
            {
                AddPlayerEnabled = false;
                SetValue(ref playersEmail, value);
                if (!string.IsNullOrWhiteSpace(value) && SelectedTemplate != null && validPlayer)
                    AddPlayerEnabled = true;
            }
        }

        private bool validCampaign;
        private Visibility campaignWarningVisibility;
        public Visibility CampaignWarningVisibility { get { return campaignWarningVisibility; } set { SetValue(ref campaignWarningVisibility, value); } }

        private bool validPlayer;
        private Visibility emailWarningVisibility;
        public Visibility EmailWarningVisibility { get { return emailWarningVisibility; } set { SetValue(ref emailWarningVisibility, value); } }

        public new bool IsEnabled 
        { 
            get { return Terminal.IsEnabled; }
            set
            {
                Terminal.IsEnabled = value;
                NotifyPropertyChanged(nameof(PlayerEnabled));
                NotifyPropertyChanged(nameof(AddPlayerEnabled));
                NotifyPropertyChanged(nameof(CreateCampaignEnabled));
            }
        }

        private bool playerEnabled;
        public bool PlayerEnabled 
        { 
            get 
            {
                if (!IsEnabled)
                    return false;
                return playerEnabled; 
            } 
            set { SetValue(ref playerEnabled, value); } 
        }

        private bool addPlayerEnabled;
        public bool AddPlayerEnabled 
        { 
            get 
            {
                if (!IsEnabled)
                    return false;
                return addPlayerEnabled; 
            } 
            set { SetValue(ref addPlayerEnabled, value); } 
        }

        private bool createCampaignEnabled;
        public bool CreateCampaignEnabled
        {
            get
            {
                if (!IsEnabled)
                    return false;
                return createCampaignEnabled;
            }
            set { SetValue(ref createCampaignEnabled, value); }
        }

        public ICommand ValidateCampaignNameCommand  { get { return new RelayCommand((e) => { ValidateCampaignName((TextChangedEventArgs)e); }); } }
        public ICommand ValidateEmailCommand { get { return new RelayCommand((e) => { ValidateEmail((TextChangedEventArgs)e); }); } }
        public ICommand AddPlayerCommand { get { return new RelayCommand((e) => { AddPlayer(); }); } }
        public ICommand CreateCampaignCommand { get { return new RelayCommand(async (e) => { await CreateCampaign(); }); } }
        public ICommand CancelCommand { get { return new RelayCommand(async (e) => { await Terminal.Instance.Navigation.Navigation(NavigationViewModel.Pages.Campaigns); }); } }

        public CreateCampaignViewModel()
        {
            CampaignsName = "Test";
            GamesList = SetGamesList();
            IsEnabled = true;
            PlayerEnabled = false;
            AddPlayerEnabled = false;
            CreateCampaignEnabled = false;
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

        private ObservableCollection<ListItem> SetTemplatesList(Games game)
        {
            List<ListItem> output = new();
            foreach (GameTemplates gt in game.GameTemplates())
            {
                ListItem li = new(gt.ToString(), gt.WriteEnum());
                li.SetContent(gt);
                output.Add(li);
            }

            return output.ToObservableCollection();
        }

        private void ValidateCampaignName(TextChangedEventArgs e)
        {
            CampaignWarningVisibility = Visibility.Hidden;
            validCampaign = true;
            Settings setting = Terminal.Instance.Main.SelectedSetting.GetContent<Settings>();
            TextBox sender = (TextBox)e.OriginalSource;
            string text = sender.Text;

            if(text.Length > 30)
            {
                text = text.Substring(0, 30);
                sender.Text = text;
            }

            //CampaignsName = text;

            if (setting.Campaigns != null)
            {
                foreach(Campaign cmp in setting.Campaigns)
                {
                    if(cmp.Name.ToUpper() == text.ToUpper())
                    {
                        CampaignWarningVisibility = Visibility.Visible;
                        validCampaign = false;
                        return;
                    }
                }
            }
        }

        private void ValidateEmail(TextChangedEventArgs e)
        {
            EmailWarningVisibility = Visibility.Hidden;
            validPlayer = true;
            TextBox sender = (TextBox)e.OriginalSource;
            string text = sender.Text;

            if(!string.IsNullOrWhiteSpace(text))
            {
                if (text.Length > 30)
                {
                    text = text.Substring(0, 30);
                    sender.Text = text;
                }

                //PlayersEmail = text;

                string[] firstSplit = text.Split(new char[] { ' ', '@' });

                if(firstSplit.Length != 2)
                {
                    EmailWarningVisibility = Visibility.Visible;
                    validPlayer = false;
                    return;
                }

                string[] secondSplit = firstSplit[1].Split('.');

                if(secondSplit.Length != 2)
                {
                    EmailWarningVisibility = Visibility.Visible;
                    validPlayer = false;
                    return;
                }

                if(secondSplit[0].Length == 0 || secondSplit[1].Length == 0)
                {
                    EmailWarningVisibility = Visibility.Visible;
                    validPlayer = false;
                }
            }
        }

        private void AddPlayer()
        {
            if (PlayersList == null)
                PlayersList = new();

            Player player = new()
            {
                EmailAddress = PlayersEmail,
                SheetTemplate = SelectedTemplate.GetContent<GameTemplates>().GetTemplate()
            };

            GameTemplates template = SelectedTemplate.GetContent<GameTemplates>();
            string frame = GetTemplatePage(template);
            ISheet sheet = SheetDrama.SheetDrama.GetSheet(template.GetTemplate(), frame, null, GetGameCommands(template.TemplateGame()));
            sheet.PlayerName = PlayersEmail;
            player.SheetJson = JsonConvert.SerializeObject(sheet);

            PlayersList.Add(player);
            NotifyPropertyChanged(nameof(PlayersList));

            CreateCampaignEnabled = true;

            PlayersEmail = string.Empty;
            SelectedTemplate = null;
        }

        private async Task CreateCampaign()
        {
            IsEnabled = false;
            Terminal.Instance.Navigation.IsLoading(true);

            if(PlayersList != null && PlayersList.Count > 0)
            {
                try
                {
                    string name = CampaignsName;
                    string game = SelectedGame?.Display;
                    List<Player> players = PlayersList.ToList();

                    string folderName = $"{name}-{game}-{DateTime.Now.Ticks}";
                    Settings currSettings = Terminal.Instance.Main.SelectedSetting.GetContent<Settings>();

                    string folderId = await ExplorerManager.Instance.CreateFolderAsync(folderName, currSettings.FolderId);

                    foreach (Player ply in players)
                        await ExplorerManager.Instance.ShareFolderAsync(folderId, ply);

                    List<Campaign> campaigns = new();

                    if (currSettings.Campaigns != null)
                        campaigns = currSettings.Campaigns.ToList();

                    Campaign newCampaign = new()
                    {
                        Name = name,
                        FolderID = folderId,
                        Narrator = Terminal.Instance.Navigation.UserMail,
                        Players = players.Select(p => p.EmailAddress).ToArray()
                    };

                    campaigns.Add(newCampaign);

                    currSettings.Campaigns = campaigns.ToArray();

                    await ExplorerManager.Instance.UpdateSettingsAsync(currSettings);

                    Terminal.Instance.Campaign = await CampaignViewModel.CreateAsync(newCampaign);
                    await Terminal.Instance.Navigation.Navigation(NavigationViewModel.Pages.Campaign);
                }
                catch(Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }

            Terminal.Instance.Navigation.IsLoading(false);
            IsEnabled = true;
        }

        private string GetTemplatePage(GameTemplates template)
        {
            string output = template.GetTemplate();
            output = output.Replace("Sheet", "Page.xaml");
            return output;
        }

        private string[] GetGameCommands(Games game)
        {
            switch(game)
            {
                case Games.ChroniclesOfDarkness:
                case Games.ChroniclesOfDarknessDarkAges:
                    return new string[] { "CoDTemplateCommand" };
                default:
                    return null;
            }
        }
    }
}

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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;
using RetiraTracker.View.Popups;
using RetiraTracker.Extensions;

namespace RetiraTracker.ViewModels
{
    public class CreateCampaignViewModel : BaseViewModel
    {
        private ObservableCollection<ListItem> m_gamesList;
        public ObservableCollection<ListItem> GamesList { get { return m_gamesList; } set { SetValue(ref m_gamesList, value); } }

        private ListItem m_selectedGame;
        public ListItem SelectedGame 
        { 
            get { return m_selectedGame; } 
            set 
            {
                PlayerEnabled = false;
                SetValue(ref m_selectedGame, value);
                if (value != null)
                {
                    PlayersList?.Clear();
                    NotifyPropertyChanged(nameof(PlayersList));

                    Games g = value.GetContent<Games>();
                    TemplatesList = SetTemplatesList(g);

                    if (!string.IsNullOrWhiteSpace(CampaignsName) && m_validCampaign)
                        PlayerEnabled = true;
                }
            } 
        }

        private ObservableCollection<ListItem> m_templatesList;
        public ObservableCollection<ListItem> TemplatesList { get { return m_templatesList; } set { SetValue(ref m_templatesList, value); } }

        private ListItem m_selectedTemplate;
        public ListItem SelectedTemplate
        {
            get { return m_selectedTemplate; }
            set
            {
                AddPlayerEnabled = false;
                SetValue(ref m_selectedTemplate, value);
                if (value != null && !string.IsNullOrWhiteSpace(PlayersEmail) && m_validPlayer)
                    AddPlayerEnabled = true;
            }
        }

        private ObservableCollection<Player> m_playersList;
        public ObservableCollection<Player> PlayersList { get { return m_playersList; } set { SetValue(ref m_playersList, value); } }

        private string m_campaignsName;
        public string CampaignsName 
        { 
            get { return m_campaignsName; } 
            set 
            {
                PlayerEnabled = false;
                SetValue(ref m_campaignsName, value);

                if(!string.IsNullOrWhiteSpace(value))
                {
                    PlayersList?.Clear();
                    NotifyPropertyChanged(nameof(PlayersList));

                    if (SelectedGame != null && m_validCampaign)
                        PlayerEnabled = true;
                }
            } 
        }

        private string m_playersEmail;
        public string PlayersEmail 
        { 
            get { return m_playersEmail; } 
            set 
            {
                AddPlayerEnabled = false;
                SetValue(ref m_playersEmail, value);
                if (!string.IsNullOrWhiteSpace(value) && SelectedTemplate != null && m_validPlayer)
                    AddPlayerEnabled = true;
            }
        }

        private bool m_validCampaign;
        private Visibility m_campaignWarningVisibility;
        public Visibility CampaignWarningVisibility { get { return m_campaignWarningVisibility; } set { SetValue(ref m_campaignWarningVisibility, value); } }

        private bool m_validPlayer;
        private Visibility m_emailWarningVisibility;
        public Visibility EmailWarningVisibility { get { return m_emailWarningVisibility; } set { SetValue(ref m_emailWarningVisibility, value); } }

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

        private bool m_playerEnabled;
        public bool PlayerEnabled 
        { 
            get 
            {
                if (!IsEnabled)
                    return false;
                return m_playerEnabled; 
            } 
            set { SetValue(ref m_playerEnabled, value); } 
        }

        private bool m_addPlayerEnabled;
        public bool AddPlayerEnabled 
        { 
            get 
            {
                if (!IsEnabled)
                    return false;
                return m_addPlayerEnabled; 
            } 
            set { SetValue(ref m_addPlayerEnabled, value); } 
        }

        private bool m_createCampaignEnabled;
        public bool CreateCampaignEnabled
        {
            get
            {
                if (!IsEnabled)
                    return false;
                return m_createCampaignEnabled;
            }
            set { SetValue(ref m_createCampaignEnabled, value); }
        }

        public ICommand ValidateCampaignNameCommand  { get { return new RelayCommand((e) => { ValidateCampaignName((TextChangedEventArgs)e); }); } }
        public ICommand ValidateEmailCommand { get { return new RelayCommand((e) => { ValidateEmail((TextChangedEventArgs)e); }); } }
        public ICommand AddPlayerCommand { get { return new RelayCommand((e) => { AddPlayer(); }); } }
        public ICommand CreateCampaignCommand { get { return new RelayCommand(async (e) => { await CreateCampaign(); }); } }
        public ICommand CancelCommand { get { return new RelayCommand(async (e) => { await Terminal.Instance.Navigation.Navigation(NavigationViewModel.Pages.Campaigns); }); } }

        public CreateCampaignViewModel()
        {
            GamesList = SetGamesList();
            IsEnabled = true;
            PlayerEnabled = false;
            AddPlayerEnabled = false;
            CreateCampaignEnabled = false;
            CampaignWarningVisibility = Visibility.Hidden;
            EmailWarningVisibility = Visibility.Hidden;
        }

        private static ObservableCollection<ListItem> SetGamesList()
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

        private static ObservableCollection<ListItem> SetTemplatesList(Games game)
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
            m_validCampaign = true;
            Settings setting = Terminal.Instance.Main.SelectedSetting.GetContent<Settings>();
            TextBox sender = (TextBox)e.OriginalSource;
            string text = sender.Text;

            if(text.Length > 30)
            {
                text = text.Substring(0, 30);
                sender.Text = text;
            }

            if (setting.Campaigns != null)
            {
                foreach(Campaign cmp in setting.Campaigns)
                {
                    if(cmp.Name.ToUpper() == text.ToUpper())
                    {
                        CampaignWarningVisibility = Visibility.Visible;
                        m_validCampaign = false;
                        return;
                    }
                }
            }
        }

        private void ValidateEmail(TextChangedEventArgs e)
        {
            EmailWarningVisibility = Visibility.Hidden;
            m_validPlayer = true;
            TextBox sender = (TextBox)e.OriginalSource;
            string text = sender.Text;

            if(!string.IsNullOrWhiteSpace(text))
            {
                if (text.Length > 30)
                {
                    text = text.Substring(0, 30);
                    sender.Text = text;
                }

                string[] firstSplit = text.Split(new char[] { ' ', '@' });

                if(firstSplit.Length != 2)
                {
                    EmailWarningVisibility = Visibility.Visible;
                    m_validPlayer = false;
                    return;
                }

                string[] secondSplit = firstSplit[1].Split('.');

                if(secondSplit.Length != 2)
                {
                    EmailWarningVisibility = Visibility.Visible;
                    m_validPlayer = false;
                    return;
                }

                if(secondSplit[0].Length == 0 || secondSplit[1].Length == 0)
                {
                    EmailWarningVisibility = Visibility.Visible;
                    m_validPlayer = false;
                }
            }
        }

        private void AddPlayer()
        {
            if (PlayersList == null)
                PlayersList = new();

            GameTemplates template = SelectedTemplate.GetContent<GameTemplates>();
            string frame = template.GetTemplatePage();
            ISheet sheet = SheetFactory.GetSheet(template.GetTemplate(), frame, null, template.TemplateGame().GetGameCommands());

            if(sheet.IsNull())
            {
                InfoPopup alert = new InfoPopup("That template doesn't exist yet!");
                alert.Show();
                return;
            }

            sheet.PlayerName = PlayersEmail;
            Player player = new()
            {
                EmailAddress = PlayersEmail,
                SheetTemplate = SelectedTemplate.GetContent<GameTemplates>().GetTemplate(),
                SheetJson = JsonConvert.SerializeObject(sheet)
            };

            PlayersList.Add(player);
            NotifyPropertyChanged(nameof(PlayersList));

            CreateCampaignEnabled = true;

            PlayersEmail = string.Empty;
            SelectedTemplate = null;
        }

        private async Task CreateCampaign()
        {
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
        }
    }
}

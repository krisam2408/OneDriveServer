using Newtonsoft.Json;
using RetiraTracker.Core;
using RetiraTracker.Core.Abstracts;
using RetiraTracker.Extensions;
using RetiraTracker.Model;
using RetiraTracker.Model.DataTransfer;
using RetiraTracker.Model.Domain;
using RetiraTracker.View.Popups;
using RetiraTracker.ViewModels.TemplateCommand;
using SheetDrama;
using SheetDrama.Abstracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RetiraTracker.ViewModels
{
    public class CampaignViewModel:BaseViewModel
    {
        private Campaign CurrentCampaign { get; set; }
        public ITemplateCommand Commands { get; set; }

        private string m_campaignsName;
        public string CampaignsName { get { return m_campaignsName; } set { SetValue(ref m_campaignsName, value); } }

        private ObservableCollection<ListItem> m_sheetList;
        public ObservableCollection<ListItem> SheetList { get { return m_sheetList; } set { SetValue(ref m_sheetList, value); } }

        private ListItem m_selectedSheet;
        public ListItem SelectedSheet
        {
            get { return m_selectedSheet; }
            set
            {
                SetValue(ref m_selectedSheet, value);
                if(value != null)
                {
                    AppSheet sheet = value.GetContent<AppSheet>();
                    sheet.Initialize();

                    ChangeSheetButtonVisibility = OriginalSheetCanChangeValue(sheet.Sheet) && Terminal.Instance.Navigation.UserMail == CurrentCampaign.Narrator ? Visibility.Visible : Visibility.Hidden;

                    for(int i = 0; i < SheetList.Count; i++)
                        if(SheetList[i].Equals(value))
                        {
                            CurrentSheet = Sheets[i];
                            break;
                        }

                }
            }
        }

        private ObservableCollection<ISheet> Sheets { get; set; }

        private ISheet m_currentSheet;
        public ISheet CurrentSheet { get { return m_currentSheet; } set { SetValue(ref m_currentSheet, value); } }

        private Visibility m_changeSheetButtonVisibility;
        public Visibility ChangeSheetButtonVisibility { get { return m_changeSheetButtonVisibility; } set { SetValue(ref m_changeSheetButtonVisibility, value); } }

        public new bool IsEnabled
        {
            get { return Terminal.IsEnabled; }
            set { Terminal.IsEnabled = value; }
        }

        public ICommand UpdateSheetCommand { get { return new RelayCommand((e) => UpdateSheet()); } }
        public ICommand SyncSheetsCommand { get { return new RelayCommand(async (e) => await SyncSheets()); } }
        public ICommand ChangeTemplateCommand { get { return new RelayCommand((e) => ChangeSheetTemplate()); } }

        private CampaignViewModel(Campaign campaign) 
        {
            CurrentCampaign = campaign;
        }

        public static async Task<CampaignViewModel> CreateAsync(Campaign campaign)
        {
            CampaignViewModel vm = new(campaign);

            vm.CampaignsName = campaign.Name;

            string[] players = SetPlayersDisplay(campaign);

            ObservableCollection<ListItem> appSheetlist = new();
            vm.Sheets = new();

            List<Task> tasks = new();
            foreach(string ply in players)
            {
                tasks.Add(Task.Run(async () =>
                {
                    ListItem item = await GetPlayerItem(ply, campaign.FolderID);
                    appSheetlist.Add(item);
                }));
            }

            await Task.WhenAll(tasks);

            vm.SheetList = appSheetlist.OrderBy(l => l.Key)
                .ToObservableCollection();
            vm.Sheets = vm.SheetList
                .Select(l =>
                {
                    AppSheet appSheet = l.GetContent<AppSheet>();
                    appSheet.Initialize();
                    return appSheet.Sheet;
                })
                .ToObservableCollection();
            vm.SelectedSheet = vm.SheetList[0];

            vm.SetTemplateCommand(vm.CurrentSheet.SheetScripts[0]);


            return vm;
        }

        private static async Task<ListItem> GetPlayerItem(string playerFilename, string campaignFolderId)
        {
            string playerJson = await ExplorerManager.Instance.GetPlayerAsync(playerFilename, campaignFolderId);
            Player player = JsonConvert.DeserializeObject<Player>(playerJson);
            AppSheet sheet = new(player);

            ListItem li = new(sheet.Sheet.SheetId, sheet.Display);
            li.SetContent(sheet);

            return li;
        }

        private void UpdateSheet()
        {
            CurrentSheet.LastModified = DateTime.Now;
            NotifyPropertyChanged(nameof(CurrentSheet));
        }

        private async Task SyncSheets()
        {
            Terminal.Instance.Navigation.IsLoading(true);
            IsEnabled = false;

            List<Task> updateTasks = new();
            for(int i = 0; i < Sheets.Count; i++)
                updateTasks.Add(UpdateCharacterSheet(i));

            await Task.WhenAll(updateTasks);

            ListItem temp = SelectedSheet;
            SelectedSheet = null;
            SelectedSheet = temp;

            IsEnabled = true;
            Terminal.Instance.Navigation.IsLoading(false);
        }

        private async Task UpdateCharacterSheet(int index)
        {
            AppSheet localAppSheet = SheetList[index].GetContent<AppSheet>();
            ISheet localSheet = Sheets[index];

            string playerID = SetPlayerDisplay(localAppSheet.Player.EmailAddress);
            string onlinePlayerJson = await ExplorerManager.Instance.GetPlayerAsync(playerID, CurrentCampaign.FolderID);
            Player onlinePlayer = JsonConvert.DeserializeObject<Player>(onlinePlayerJson);
            AppSheet onlineAppSheet = new(onlinePlayer);
            ISheet onlineSheet = onlineAppSheet.Sheet;

            if (localSheet.LastModified > onlineSheet.LastModified)
            {
                AppSheet updatedAppSheet = new(localAppSheet.Player);
                updatedAppSheet.UpdatePlayer(localSheet);
                await ExplorerManager.Instance.UpdatePlayerAsync(playerID, CurrentCampaign.FolderID, updatedAppSheet.Player);
                return;
            }
            
            SheetList[index].SetContent(onlineAppSheet);
            Sheets[index] = onlineSheet;
        }

        private void ChangeSheetTemplate()
        {
            IsEnabled = false;

            string sheetType = CurrentSheet.GetType().Name;
            ISheet basicSheet = SheetFactory.GetBasicSheet(sheetType);

            ChangeSheetTemplatePopup popup = new(basicSheet.CanChangeTo);
            popup.Show();
        }

        private void SetTemplateCommand(string commandName)
        {
            IEnumerable<Type> templates = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.Namespace == "RetiraTracker.ViewModels.TemplateCommand");

            Type targetTemplate = templates.Where(t => t.Name == commandName)
                .FirstOrDefault();

            if (targetTemplate == null)
                return;

            Type[] ctorParameters = new Type[1] { typeof(BaseViewModel) };
            ConstructorInfo ctorInfo = targetTemplate.GetConstructor(ctorParameters);
            object[] parameters = new object[] { this };
            ITemplateCommand cmds = (ITemplateCommand)ctorInfo.Invoke(parameters);

            Commands = cmds;
        }

        private static string[] SetPlayersDisplay(Campaign campaign)
        {

            if (campaign.Narrator != Terminal.Instance.Navigation.UserMail)
            {
                return campaign.Players
                    .Where(p => p.ToUpper() == Terminal.Instance.Navigation.UserMail.ToUpper())
                    .Select(p => SetPlayerDisplay(p))
                    .ToArray();
            }

            return campaign.Players
                .Select(p => SetPlayerDisplay(p))
                .ToArray();
        }

        private static string SetPlayerDisplay(string playerEmail)
        {
            string user = playerEmail.Split('@')[0];
            return $"{user}.json";
        }

        private static bool OriginalSheetCanChangeValue(ISheet sheet)
        {
            Type sheetType = sheet.GetType();
            ISheet basicSheet = SheetFactory.GetBasicSheet(sheetType.Name);
            return basicSheet.CanChange;
        }
    }
}

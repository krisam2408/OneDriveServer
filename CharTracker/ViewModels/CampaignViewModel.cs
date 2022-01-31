using Newtonsoft.Json;
using RetiraTracker.Core;
using RetiraTracker.Core.Abstracts;
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

        private string campaignsName;
        public string CampaignsName { get { return campaignsName; } set { SetValue(ref campaignsName, value); } }

        private ObservableCollection<ListItem> sheetList;
        public ObservableCollection<ListItem> SheetList { get { return sheetList; } set { SetValue(ref sheetList, value); } }

        private ListItem selectedSheet;
        public ListItem SelectedSheet
        {
            get { return selectedSheet; }
            set
            {
                SetValue(ref selectedSheet, value);
                if(value != null)
                {
                    AppSheet sheet = value.GetContent<AppSheet>();
                    sheet.SetAppSheet();

                    if (!sheet.Sheet.SheetFrame.Contains("SheetTemplates/"))
                        sheet.Sheet.SheetFrame = $"SheetTemplates/{sheet.Sheet.SheetFrame}";

                    SheetData = sheet.Sheet;

                    ChangeSheetButtonVisibility = OriginalSheetCanChangeValue(SheetData) && Terminal.Instance.Navigation.UserMail == CurrentCampaign.Narrator ? Visibility.Visible : Visibility.Hidden;
                }
            }
        }

        private Visibility changeSheetButtonVisibility;
        public Visibility ChangeSheetButtonVisibility { get { return changeSheetButtonVisibility; } set { SetValue(ref changeSheetButtonVisibility, value); } }

        public new bool IsEnabled
        {
            get { return Terminal.IsEnabled; }
            set { Terminal.IsEnabled = value; }
        }

        private ISheet sheetData;
        public ISheet SheetData { get { return sheetData; } set { SetValue(ref sheetData, value); } }

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

            ObservableCollection<ListItem> list = new();

            List<Task> tasks = new();
            foreach(string ply in players)
            {
                tasks.Add(Task.Run(async () =>
                {
                    list.Add(await GetPlayerItem(ply, campaign.FolderID));
                }));
            }

            await Task.WhenAll(tasks);

            AppSheet firstAppSheet = list[0].GetContent<AppSheet>();
            firstAppSheet.SetAppSheet();
            ISheet firstSheet = firstAppSheet.Sheet;
            vm.SetTemplateCommand(firstSheet.SheetScripts[0]);

            vm.SheetList = list;

            vm.SelectedSheet = vm.SheetList[0];

            return vm;
        }

        private static async Task<ListItem> GetPlayerItem(string playerFilename, string campaignFolderId)
        {
            string playerJson = await ExplorerManager.Instance.GetPlayerAsync(playerFilename, campaignFolderId);
            Player player = JsonConvert.DeserializeObject<Player>(playerJson);
            AppSheet sheet = new(player);

            ListItem li = new(playerFilename, sheet.Display);
            li.SetContent(sheet);

            return li;
        }

        private void UpdateSheet()
        {
            SheetData.LastModified = DateTime.Now;

            AppSheet newSheet = SelectedSheet.GetContent<AppSheet>();
            newSheet.Player.SheetJson = JsonConvert.SerializeObject(SheetData);
            newSheet.SetAppSheet();

            SelectedSheet.SetContent(newSheet);
            SelectedSheet.SetDisplay(SheetPlayerDisplay());
            NotifyPropertyChanged(nameof(SheetList));
            
            NotifyPropertyChanged(nameof(SheetData));
        }

        private async Task SyncSheets()
        {
            Terminal.Instance.Navigation.IsLoading(true);
            IsEnabled = false;

            List<Task> updateTasks = new();
            for(int i = 0; i < SheetList.Count; i++)
                updateTasks.Add(UpdateCharacterSheet(i));

            await Task.WhenAll(updateTasks);

            IsEnabled = true;
            Terminal.Instance.Navigation.IsLoading(false);
        }

        private async Task UpdateCharacterSheet(int index)
        {
            AppSheet localAppSheet = SheetList[index].GetContent<AppSheet>();
            localAppSheet.SetAppSheet();
            ISheet localSheet = localAppSheet.Sheet;

            string playerID = SetPlayerDisplay(localAppSheet.Player.EmailAddress);
            string onlinePlayerJson = await ExplorerManager.Instance.GetPlayerAsync(playerID, CurrentCampaign.FolderID);
            Player onlinePlayer = JsonConvert.DeserializeObject<Player>(onlinePlayerJson);
            AppSheet onlineAppSheet = new(onlinePlayer);
            ISheet onlineSheet = onlineAppSheet.Sheet;

            if (localSheet.LastModified > onlineSheet.LastModified)
            {
                await ExplorerManager.Instance.UpdatePlayerAsync(playerID, CurrentCampaign.FolderID, localAppSheet.Player);
            }
            else
            {
                SheetList[index].SetContent(onlineAppSheet);
                SelectedSheet = SelectedSheet;
            }
        }

        private void ChangeSheetTemplate()
        {
            IsEnabled = false;

            string sheetType = SheetData.GetType().Name;
            ISheet basicSheet = SheetFactory.GetBasicSheet(sheetType);

            ChangeSheetTemplatePopup popup = new(basicSheet.CanChangeTo);
            popup.Show();
        }

        private string SheetPlayerDisplay()
        {
            if (!string.IsNullOrWhiteSpace(SheetData.CharacterName))
                return SheetData.CharacterName;

            string username = SheetData.PlayerName.Split('@')[0];
            return $"({username})";
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

using Newtonsoft.Json;
using RetiraTracker.Core;
using RetiraTracker.Core.Abstracts;
using RetiraTracker.Model;
using RetiraTracker.Model.DataTransfer;
using RetiraTracker.Model.Domain;
using RetiraTracker.ViewModels.TemplateCommand;
using SheetDrama.Abstracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
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
                    SheetData = sheet.Sheet;
                    SheetSource = $"SheetTemplates/{sheet.Sheet.SheetFrame}";
                    ChangeSheetButtonVisibility = SheetData.CanChange ? Visibility.Visible : Visibility.Hidden;
                }
            }
        }

        private Visibility changeSheetButtonVisibility;
        public Visibility ChangeSheetButtonVisibility { get { return changeSheetButtonVisibility; } set { SetValue(ref changeSheetButtonVisibility, value); } }

        public new bool IsEnabled
        {
            get { return Terminal.IsEnabled; }
            set
            {
                Terminal.IsEnabled = value;
            }
        }

        private string sheetSource;
        public string SheetSource { get { return sheetSource; } set { SetValue(ref sheetSource, value); } }

        private ISheet sheetData;
        public ISheet SheetData { get { return sheetData; } set { SetValue(ref sheetData, value); } }

        public ICommand UpdateSheetCommand { get { return new RelayCommand((e) => UpdateSheet()); } }
        public ICommand SyncSheetsCommand { get { return new RelayCommand(async (e) => await SyncSheets()); } }

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
            ISheet firstSheet = null;

            int i = 0;
            foreach(string ply in players)
            {
                string playerJson = await ExplorerManager.Instance.GetPlayerAsync(ply, campaign.FolderID);
                Player player = JsonConvert.DeserializeObject<Player>(playerJson);
                AppSheet sheet = new();
                sheet.SetAppSheet(player);

                if (i == 0)
                    firstSheet = sheet.Sheet;

                ListItem li = new(ply, sheet.Display);
                li.SetContent(sheet);

                list.Add(li);
                i++;
            }

            vm.SetTemplateCommand(firstSheet.SheetScripts[0]);

            vm.SheetList = list;

            vm.SelectedSheet = vm.SheetList[0];

            return vm;
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

            int result = 0;

            for(int i = 0; i < SheetList.Count; i++)
            {
                AppSheet localAppSheet = SheetList[i].GetContent<AppSheet>();
                localAppSheet.SetAppSheet();
                ISheet localSheet = localAppSheet.Sheet;

                string playerID = SetPlayerDisplay(localAppSheet.Player.EmailAddress);
                string onlinePlayerJson = await ExplorerManager.Instance.GetPlayerAsync(playerID, CurrentCampaign.FolderID);
                Player onlinePlayer = JsonConvert.DeserializeObject<Player>(onlinePlayerJson);
                AppSheet onlineAppSheet = new();
                onlineAppSheet.SetAppSheet(onlinePlayer);
                ISheet onlineSheet = onlineAppSheet.Sheet;

                if(localSheet.LastModified > onlineSheet.LastModified)
                {
                    Player localPlayer = new()
                    {
                        EmailAddress = onlinePlayer.EmailAddress,
                        SheetTemplate = onlinePlayer.SheetTemplate,
                        SheetJson = JsonConvert.SerializeObject(localSheet)
                    };
                    bool transResult = await ExplorerManager.Instance.UpdatePlayerAsync(playerID, CurrentCampaign.FolderID, localPlayer);

                    if (!transResult)
                        result++;
                }
                else
                {
                    SheetList[i].SetContent(onlineAppSheet);
                }
            }

            if(result > 0)
            {
                // Popup de resultados con errores
            }

            IsEnabled = true;
            Terminal.Instance.Navigation.IsLoading(false);
        }

        private string SheetPlayerDisplay()
        {
            if (!string.IsNullOrWhiteSpace(SheetData.CharacterName))
                if (SelectedSheet.Display != SheetData.CharacterName)
                    return SheetData.CharacterName;

            return $"({SheetData.PlayerName})";
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
    }
}

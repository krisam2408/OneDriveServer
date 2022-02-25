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
using System.Windows.Controls;
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

                    SetVisibleSheetFrame(sheet.Sheet);

                    ChangeSheetButtonVisibility = OriginalSheetCanChangeValue(sheet.Sheet) && Terminal.Instance.Navigation.UserMail == CurrentCampaign.Narrator ? Visibility.Visible : Visibility.Hidden;
                }
            }
        }

        private ObservableCollection<Frame> frameList;
        public ObservableCollection<Frame> FrameList { get { return frameList; } set { SetValue(ref frameList, value); } }

        private Visibility changeSheetButtonVisibility;
        public Visibility ChangeSheetButtonVisibility { get { return changeSheetButtonVisibility; } set { SetValue(ref changeSheetButtonVisibility, value); } }

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

            vm.SheetList = list;
            vm.FrameList = SetFrames(list);

            vm.SelectedSheet = list[0];
            AppSheet firstAppSheet = vm.SelectedSheet.GetContent<AppSheet>();
            firstAppSheet.SetAppSheet();
            ISheet firstSheet = firstAppSheet.Sheet;
            vm.SetTemplateCommand(firstSheet.SheetScripts[0]);

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
            ISheet selectedSheet = GetSelectedSheet();
            selectedSheet.LastModified = DateTime.Now;
            
            SelectedSheet.SetDisplay(SheetPlayerDisplay(selectedSheet));
        }

        private async Task SyncSheets()
        {
            Terminal.Instance.Navigation.IsLoading(true);
            IsEnabled = false;

            List<Task> updateTasks = new();
            for(int i = 0; i < FrameList.Count; i++)
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
            ISheet localSheet = (ISheet)FrameList[index].DataContext;

            string playerID = SetPlayerDisplay(localAppSheet.Player.EmailAddress);
            string onlinePlayerJson = await ExplorerManager.Instance.GetPlayerAsync(playerID, CurrentCampaign.FolderID);
            Player onlinePlayer = JsonConvert.DeserializeObject<Player>(onlinePlayerJson);
            AppSheet onlineAppSheet = new(onlinePlayer);
            ISheet onlineSheet = onlineAppSheet.Sheet;

            if (localSheet.LastModified > onlineSheet.LastModified)
            {
                AppSheet updatedAppSheet = new(localAppSheet.Player);
                updatedAppSheet.Sheet = localSheet;
                await ExplorerManager.Instance.UpdatePlayerAsync(playerID, CurrentCampaign.FolderID, updatedAppSheet.Player);
            }
            else
            {
                SheetList[index].SetContent(onlineAppSheet);
                FrameList[index].DataContext = onlineSheet;
            }
        }

        private void ChangeSheetTemplate()
        {
            IsEnabled = false;

            ISheet selectedSheet = GetSelectedSheet();

            string sheetType = selectedSheet.GetType().Name;
            ISheet basicSheet = SheetFactory.GetBasicSheet(sheetType);

            ChangeSheetTemplatePopup popup = new(basicSheet.CanChangeTo);
            popup.Show();
        }

        private string SheetPlayerDisplay(ISheet sheet)
        {
            if (!string.IsNullOrWhiteSpace(sheet.CharacterName))
                return sheet.CharacterName;

            string username = sheet.PlayerName.Split('@')[0];
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

        private static ObservableCollection<Frame> SetFrames(ObservableCollection<ListItem> sheetList)
        {
            ObservableCollection<Frame> output = new();

            foreach (ListItem item in sheetList)
            {
                Frame frame = new();
                AppSheet appSheet = item.GetContent<AppSheet>();
                appSheet.SetAppSheet();

                if (!appSheet.Sheet.SheetFrame.Contains("SheetTemplates/"))
                    appSheet.Sheet.SheetFrame = $"SheetTemplates/{appSheet.Sheet.SheetFrame}";

                frame.Source = new(appSheet.Sheet.SheetFrame);
                frame.DataContext = appSheet.Sheet;
                frame.Visibility = Visibility.Collapsed;
                output.Add(frame);
            }

            return output;
        }

        private static bool OriginalSheetCanChangeValue(ISheet sheet)
        {
            Type sheetType = sheet.GetType();
            ISheet basicSheet = SheetFactory.GetBasicSheet(sheetType.Name);
            return basicSheet.CanChange;
        }

        private void SetVisibleSheetFrame(ISheet selectedSheet)
        {
            foreach (Frame frame in FrameList)
            {
                frame.Visibility = Visibility.Collapsed;
                if(frame.DataContext == selectedSheet)
                    frame.Visibility = Visibility.Visible;
            }
        }

        private ISheet GetSelectedSheet()
        {
            AppSheet selectedSheet = SelectedSheet.GetContent<AppSheet>();
            selectedSheet.SetAppSheet();

            foreach(Frame frame in FrameList)
                if(frame.DataContext == selectedSheet.Sheet)
                    return (ISheet)frame.DataContext;
            throw new ArgumentOutOfRangeException(nameof(selectedSheet), $"{nameof(selectedSheet)} out of range.");
        }
    }
}

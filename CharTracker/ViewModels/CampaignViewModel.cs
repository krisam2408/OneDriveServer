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
using System.Windows.Input;

namespace RetiraTracker.ViewModels
{
    public class CampaignViewModel:BaseViewModel
    {
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
                }
            }
        }

        private string sheetSource;
        public string SheetSource { get { return sheetSource; } set { SetValue(ref sheetSource, value); } }

        private ISheet sheetData;
        public ISheet SheetData 
        { 
            get { return sheetData; } 
            set { SetValue(ref sheetData, value); } 
        }

        public ICommand UpdateSheetCommand { get { return new RelayCommand(async (e) => await UpdateSheet()); } }

        private CampaignViewModel() { }

        public static async Task<CampaignViewModel> CreateAsync(Campaign campaign)
        {
            CampaignViewModel vm = new();

            vm.CampaignsName = campaign.Name;

            string[] players = SetPlayersDisplay(campaign);

            ObservableCollection<ListItem> list = new();
            ISheet firstSheet = null;

            int i = 0;
            foreach(string ply in players)
            {
                string playerJson = await ExplorerManager.Instance.GetPlayer(ply, campaign.FolderID);
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

            vm.NotifyPropertyChanged(nameof(SheetData)+".Intelligence");

            return vm;
        }

        private async Task UpdateSheet()
        {
            SheetData.LastModified = DateTime.Now;
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
                    .Select(p =>
                    {
                        string user = p.Split('@')[0];
                        return $"{user}.json";
                    })
                    .ToArray();
            }

            return campaign.Players
                .Select(p =>
                {
                    string user = p.Split('@')[0];
                    return $"{user}.json";
                })
                .ToArray();
        }
    }
}

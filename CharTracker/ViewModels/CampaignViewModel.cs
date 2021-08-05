using Newtonsoft.Json;
using RetiraTracker.Core;
using RetiraTracker.Core.Abstracts;
using RetiraTracker.Model;
using RetiraTracker.Model.DataTransfer;
using RetiraTracker.Model.Domain;
using SheetDrama.Abstracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetiraTracker.ViewModels
{
    public class CampaignViewModel:BaseViewModel
    {
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

        private CampaignViewModel() { }

        public static async Task<CampaignViewModel> CreateAsync(Campaign campaign)
        {
            CampaignViewModel vm = new();

            vm.CampaignsName = campaign.Name;

            string[] players = SetPlayersDisplay(campaign);

            ObservableCollection<ListItem> list = new();

            foreach(string ply in players)
            {
                string playerJson = await ExplorerManager.Instance.GetPlayer(ply, campaign.FolderID);
                Player player = JsonConvert.DeserializeObject<Player>(playerJson);
                AppSheet sheet = new();
                sheet.SetAppSheet(player);

                ListItem li = new(ply, sheet.Display);
                li.SetContent(sheet);

                list.Add(li);
            }

            vm.SheetList = list;

            vm.SelectedSheet = vm.SheetList[0];

            return vm;
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

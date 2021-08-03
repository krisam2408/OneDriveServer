using Newtonsoft.Json;
using RetiraTracker.Core.Abstracts;
using RetiraTracker.Model;
using RetiraTracker.Model.DataTransfer;
using RetiraTracker.Model.Domain;
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
        private ObservableCollection<AppSheet> sheetList;
        public ObservableCollection<AppSheet> SheetList { get { return sheetList; } set { SetValue(ref sheetList, value); } }

        private CampaignViewModel() { }

        public static async Task<CampaignViewModel> CreateAsync(Campaign campaign)
        {
            CampaignViewModel vm = new();

            string[] players = campaign.Players
                .Select(p =>
                {
                    string user = p.Split('@')[0];
                    return $"{user}.json";
                })
                .ToArray();

            ObservableCollection<AppSheet> list = new();

            foreach(string ply in players)
            {
                string playerJson = await ExplorerManager.Instance.GetPlayer(ply, campaign.FolderID);
                Player player = JsonConvert.DeserializeObject<Player>(playerJson);
                AppSheet sheet = new(player);

                list.Add(sheet);
            }

            vm.SheetList = list;

            return vm;
        }
    }
}

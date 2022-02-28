using RetiraTracker.Model.Domain;
using SheetDrama.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SheetDrama;
using SheetDrama.Extensions;

namespace RetiraTracker.Model.DataTransfer
{
    public class AppSheet
    {
        public Player Player { get; set; }
        [JsonIgnore]
        public ISheet Sheet { get; set; }

        public string Display
        {
            get
            {
                if(Player != null && !string.IsNullOrWhiteSpace(Player.EmailAddress))
                {
                    string playerName = Player.EmailAddress.Split('@')[0];
                    return playerName;
                }

                return "( - )";
            }
        }

        public AppSheet(Player player)
        {
            Player = player;
            Sheet = SheetFactory.GetSheet(player.SheetTemplate, player.SheetJson);
        }

        public void Initialize()
        {
            if (Player == null)
                throw new ArgumentNullException(nameof(Player), "Player is null.");

            ISheet newSheet = SheetFactory.GetSheet(Player.SheetTemplate, Player.SheetJson);
            Sheet = newSheet;
        }

        public void UpdatePlayer(ISheet sheet)
        {
            string sheetType = sheet.Template.GetTemplate();
            string sheetJson = JsonConvert.SerializeObject(sheet);
            Player.SheetTemplate = sheetType;
            Player.SheetJson = sheetJson;
        }
    }
}

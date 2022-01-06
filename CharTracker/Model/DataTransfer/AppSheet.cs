using RetiraTracker.Model.Domain;
using SheetDrama.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SheetDrama;

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
                if (Sheet != null && !string.IsNullOrWhiteSpace(Sheet.CharacterName))
                    return Sheet.CharacterName;

                if(Player != null && !string.IsNullOrWhiteSpace(Player.EmailAddress))
                {
                    string playerName = Player.EmailAddress.Split('@')[0];
                    return $"({playerName})";
                }

                return "( )";
            }
        }

        public AppSheet(Player player)
        {
            Player = player;
            Sheet = SheetFactory.GetSheet(player.SheetTemplate, player.SheetJson);
        }

        public void SetAppSheet()
        {
            if (Player == null)
                throw new ArgumentNullException(nameof(Player), "Player is null.");

            Sheet = SheetFactory.GetSheet(Player.SheetTemplate, Player.SheetJson);
        }
    }
}

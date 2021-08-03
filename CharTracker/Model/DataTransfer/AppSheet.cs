using RetiraTracker.Model.Domain;
using SheetDrama.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetiraTracker.Model.DataTransfer
{
    public class AppSheet
    {
        public Player Player { get; init; }
        public ISheet Sheet { get; set; }

        public string Display
        {
            get
            {
                if (Sheet != null && !string.IsNullOrWhiteSpace(Sheet.CharacterName))
                    return Sheet.CharacterName;

                string playerName = Player.EmailAddress.Split('@')[0];
                return $"({playerName})";
            }
        }

        public AppSheet(Player player)
        {
            Player = player;
        }
    }
}

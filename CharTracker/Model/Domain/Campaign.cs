using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharTracker.Model.Domain
{
    public class Campaign
    {
        public string Name { get; set; }
        public string FolderID { get; set; }
        public string Narrator { get; set; }
        public Player[] Players { get; set; }
    }
}

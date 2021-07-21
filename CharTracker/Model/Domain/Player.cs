using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharTracker.Model.Domain
{
    public class Player
    {
        public string EmailAddress { get; set; }
        public string SheetJson { get; set; }
        public string SheetTemplate { get; set; }
    }
}

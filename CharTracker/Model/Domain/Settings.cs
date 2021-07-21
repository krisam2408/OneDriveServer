using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharTracker.Model.Domain
{
    public class Settings
    {
        public string FolderId { get; set; }
        public string Owner { get; set; }
        public Campaign[] Campaigns { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Settings settings)
                if (settings.FolderId == FolderId)
                    return true;

            return false;
        }
    }
}

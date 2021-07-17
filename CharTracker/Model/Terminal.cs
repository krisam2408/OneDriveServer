using CharTracker.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharTracker.Model
{
    public class Terminal
    {
        public MainViewModel Main { get; set; }

        private static Terminal instance;
        public static Terminal Instance
        {
            get
            {
                if (instance == null)
                    instance = new();
                return instance;
            }
        }

        private Terminal() { }
    }
}

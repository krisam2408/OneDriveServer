using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetiraTracker.Model.DataTransfer
{
    public struct AppVersion
    {
        public string Version { get; set; }
        public int MajorVersionNumber { get { return VersionNumberSplice(0); } }
        public int MiddleVersionNumber { get { return VersionNumberSplice(1); } }
        public int MinorVersionNumber { get { return VersionNumberSplice(2); } }

        private int VersionNumberSplice(int splice)
        {
            if(splice < 0)
                splice = 0;

            if(splice > 2)
                splice = 2;

            int[] splices = Version.Split('.')
                .Select(n => int.Parse(n))
                .ToArray();

            return splices[splice];
        }

        public static bool operator < (AppVersion a, AppVersion b)
        {
            if (a.MajorVersionNumber < b.MajorVersionNumber)
                return true;
            if (a.MiddleVersionNumber < b.MiddleVersionNumber)
                return true;
            if (a.MinorVersionNumber < b.MinorVersionNumber)
                return true;
            return false;
        }

        public static bool operator > (AppVersion a, AppVersion b)
        {
            if (a.MajorVersionNumber > b.MajorVersionNumber)
                return true;
            if (a.MiddleVersionNumber > b.MiddleVersionNumber)
                return true;
            if (a.MinorVersionNumber > b.MinorVersionNumber)
                return true;
            return false;
        }

        public static bool operator == (AppVersion a, AppVersion b)
        {
            return a.Equals(b);
        }

        public static bool operator != (AppVersion a, AppVersion b)
        {
            return !a.Equals(b);
        }

        public override bool Equals(object obj)
        {
            if(obj is AppVersion version)
            {
                if (version.Version.Equals(Version))
                    return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Version.GetHashCode();
        }
    }
}

using Google.Apis.Drive.v3.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleExplorer.Extensions
{
    public class FileEqualityComparer : IEqualityComparer<File>
    {
        public bool Equals(File x, File y)
        {
            if (x.Id == y.Id)
                return true;

            return false;
        }

        public int GetHashCode([DisallowNull] File obj)
        {
            int hashCode = obj.Id.GetHashCode();
            return hashCode;
        }
    }
}

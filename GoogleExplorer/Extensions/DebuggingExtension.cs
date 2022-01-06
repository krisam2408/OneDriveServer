using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleExplorer.Extensions
{
    public static class DebuggingExtension
    {
        public static void DebugExpectionType(Exception ex)
        {
            Type exType = ex.GetType();
            string strExType = exType.ToString();
            Debug.WriteLine(strExType);
            Debug.WriteLine(ex.Message);
        }
    }
}

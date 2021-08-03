using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RetiraTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();

#if TEST1
            byte[] buffer = explorer.DownloadFile("test001").Result;
            string read = Encoding.UTF8.GetString(buffer);
#endif
#if TEST2
            string strTest = "Actualizacion Otro Texto 3";
            byte[] buffer = Encoding.UTF8.GetBytes(strTest);
            explorer.OverwriteFile("test002", buffer, MimeTypes.Text).Wait();
#endif

#if TEST3
            explorer.CreateFolder("FolderTest").Wait();
            await Application.Current.Dispatcher.Invoke(async () => { await GetValue(); });
#endif

        }
    }
}

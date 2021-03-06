using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace RetiraTracker.View
{
    /// <summary>
    /// Interaction logic for CampaignPage.xaml
    /// </summary>
    public partial class CampaignPage : Page
    {
        public CampaignPage()
        {
            InitializeComponent();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollviewer = (ScrollViewer)sender;
            scrollviewer.ScrollToVerticalOffset(scrollviewer.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}

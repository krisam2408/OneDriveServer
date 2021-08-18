using RetiraTracker.Core;
using RetiraTracker.Model;
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
using System.Windows.Shapes;

namespace RetiraTracker.View.Popups
{
    /// <summary>
    /// Interaction logic for InfoPopup.xaml
    /// </summary>
    public partial class InfoPopup : Window
    {
        public InfoPopup(string message)
        {
            InitializeComponent();

            Message = message;
            DataContext = this;
        }

        public string Message { get; set; }

        public ICommand CloseCommand
        {
            get
            {
                return new RelayCommand(e =>
                {
                    Terminal.Instance.Navigation.IsLoading(false);
                    Close();
                });
            }
        }
    }
}

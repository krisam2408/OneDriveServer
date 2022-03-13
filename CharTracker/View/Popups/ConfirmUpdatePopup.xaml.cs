using RetiraTracker.Core;
using RetiraTracker.Model;
using RetiraTracker.Model.DataTransfer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for ConfirmUpdatePopup.xaml
    /// </summary>
    public partial class ConfirmUpdatePopup : Window
    {
        private AppVersion m_appVersion;

        public ConfirmUpdatePopup(AppVersion version)
        {
            InitializeComponent();

            m_appVersion = version;

            DataContext = this;
        }

        public string Message
        {
            get
            {
                return $"Update {m_appVersion.Version} is available. Do you wish tu update now?";
            }
        }

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

        public ICommand UpdateCommand
        {
            get
            {
                return new RelayCommand(e =>
                {
                    Process updateProcess = new();
                    updateProcess.StartInfo.UseShellExecute = false;
                    updateProcess.StartInfo.RedirectStandardOutput = false;
                    updateProcess.StartInfo.Arguments = m_appVersion.Version;
                    updateProcess.StartInfo.FileName = "Updater/RetiraUpdater.exe";
                    updateProcess.Start();

                    Terminal.Instance.Navigation.AppCloseCommand.Execute(null);
                    CloseCommand.Execute(null);
                });
            }
        }
    }
}

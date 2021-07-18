using CharTracker.Core;
using CharTracker.Core.Abstracts;
using CharTracker.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CharTracker.ViewModels
{
    public class MainViewModel:BaseViewModel
    {

        public ICommand LogInCommand { get { return new RelayCommand(async (s) => await LogIn()); } }

        private async Task LogIn()
        {
            Terminal.Instance.Navigation.UserMail = await ExplorerManager.Instance.LogIn();
            Terminal.Instance.Navigation.IsMenuVisible(true);
            Terminal.Instance.Navigation.Navigation(NavigationViewModel.Pages.Campaigns);

        }
    }
}

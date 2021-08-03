
namespace RetiraTracker.Core.Abstracts
{
    public abstract class BaseViewModel:BaseNotifyPropertyChanged
    {
        private bool isEnabled;
        public bool IsEnabled { get { return isEnabled; } set { SetValue(ref isEnabled, value); } }

        protected BaseViewModel() 
        {
            IsEnabled = true;
        }
    }
}


namespace RetiraTracker.Core.Abstracts
{
    public abstract class BaseViewModel:BaseNotifyPropertyChanged
    {
        private bool m_isEnabled;
        public bool IsEnabled { get { return m_isEnabled; } set { SetValue(ref m_isEnabled, value); } }

        protected BaseViewModel() 
        {
            IsEnabled = true;
        }
    }
}

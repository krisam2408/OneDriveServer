using RetiraTracker.ViewModels;

namespace RetiraTracker.Model
{
    public class Terminal
    {
        public NavigationViewModel Navigation { get; set; }
        public MainViewModel Main { get; set; }
        public CampaignViewModel Campaign { get; set; }
        public CreateCampaignViewModel CreateCampaign { get; set; }

        private static Terminal instance;
        public static Terminal Instance
        {
            get
            {
                if (instance == null)
                    instance = new();
                return instance;
            }
        }

        private Terminal() { }

        public static bool IsEnabled 
        { 
            get { return Instance.Navigation.IsEnabled; }
            set { Instance.Navigation.IsEnabled = value; }
        }
    }
}

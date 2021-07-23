using CharTracker.ViewModels;

namespace CharTracker.Model
{
    public class Terminal
    {
        public int WindowWidth { get; init; }
        public int WindowHeight { get; init; }

        public NavigationViewModel Navigation { get; set; }
        public MainViewModel Main { get; set; }

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

        private Terminal() 
        {
            WindowWidth = 800;
            WindowHeight = 450;
        }

        public static bool IsEnabled 
        { 
            get { return Instance.Navigation.IsEnabled; }
            set { Instance.Navigation.IsEnabled = value; }
        }
    }
}

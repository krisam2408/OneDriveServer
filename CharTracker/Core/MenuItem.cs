using System.Windows.Media;

namespace CharTracker.Core
{
    public class MenuItem:ListItem
    {
        public Brush BackgroundColor { get; set; }
        public Brush TextColor { get; set; }

        public MenuItem(string key, string display, bool selected = false) : base(key, display, selected) { }
    }
}

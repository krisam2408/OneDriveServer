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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ColorRoseLib;

namespace RetiraTracker.View.UserControls
{
    /// <summary>
    /// Interaction logic for ButtonControl.xaml
    /// </summary>
    public partial class ButtonControl : UserControl
    {
        public ButtonControl()
        {
            InitializeComponent();
        }

        public SolidColorBrush BorderBackground { get; set; }
        public SolidColorBrush MouseOverBorderBackground { get { return SetMouseOverColor(); } }
        public SolidColorBrush DisabledBorderBackground { get { return SetDisabledColor(); } }

        public string Text { get; set; }

        public double BorderWidth { get; set; }
        public double BorderHeight { get; set; }

        public static DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(ButtonControl));
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        private SolidColorBrush SetMouseOverColor()
        {
            HSBColor activeColor = HSBColor.FromARGB(255, BorderBackground.Color.R, BorderBackground.Color.G, BorderBackground.Color.B);
            activeColor.Darken(16);
            byte[] color = activeColor.ToARGB();
            Color brushColor = Color.FromArgb(color[0], color[1], color[2], color[3]);
            return new SolidColorBrush(brushColor);
        }

        private SolidColorBrush SetDisabledColor()
        {
            HSBColor activeColor = HSBColor.FromARGB(255, BorderBackground.Color.R, BorderBackground.Color.G, BorderBackground.Color.B);
            activeColor.Brighten(16);
            byte[] color = activeColor.ToARGB();
            Color brushColor = Color.FromArgb(color[0], color[1], color[2], color[3]);
            return new SolidColorBrush(brushColor);
        }
    }
}

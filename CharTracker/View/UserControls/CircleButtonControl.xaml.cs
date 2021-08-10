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
using ColorRoseLib;

namespace RetiraTracker.View.UserControls
{
    /// <summary>
    /// Interaction logic for CircleButtonControl.xaml
    /// </summary>
    public partial class CircleButtonControl : UserControl
    {
        public CircleButtonControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        public SolidColorBrush BorderBackground { get; set; }
        public SolidColorBrush MouseOverBorderBackground { get { return SetMouseOverColor(); } }
        public SolidColorBrush DisabledBorderBackground { get { return SetDisabledColor(); } }

        public string Data { get; set; }

        public double Diameter { get; set; }

        public static DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(CircleButtonControl));
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static DependencyProperty CustomParameterProperty = DependencyProperty.Register("CustomParameter", typeof(object), typeof(CircleButtonControl));
        public object CustomParameter
        {
            get { return GetValue(CustomParameterProperty); }
            set { SetValue(CustomParameterProperty, value); }
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

using ColorRoseLib;
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
using Color = System.Drawing.Color;
using MediaColor = System.Windows.Media.Color;

namespace CharTracker.View.UserControls
{
    /// <summary>
    /// Interaction logic for CloseButtonControl.xaml
    /// </summary>
    public partial class CloseButtonControl : UserControl
    {
        public CloseButtonControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        public SolidColorBrush DarkRot { get { return SetDarkRot(); } }

        public static DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(CloseButtonControl));
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        private SolidColorBrush SetDarkRot()
        {
            MediaColor rotColor = (MediaColor)Application.Current.Resources["RotColor"];
            Color color = Color.FromArgb(rotColor.R, rotColor.G, rotColor.B);
            HSBColor activeColor = HSBColor.FromARGBColor(color);
            activeColor.Darken(16);
            color = activeColor.ToARGBColor();
            MediaColor brushColor = MediaColor.FromArgb(color.A, color.R, color.G, color.B);
            return new SolidColorBrush(brushColor);
        }
    }
}

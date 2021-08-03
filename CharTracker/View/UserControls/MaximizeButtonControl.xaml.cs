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

namespace RetiraTracker.View.UserControls
{
    /// <summary>
    /// Interaction logic for MaximizeButtonControl.xaml
    /// </summary>
    public partial class MaximizeButtonControl : UserControl
    {
        public MaximizeButtonControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        public SolidColorBrush DarkGruen { get { return SetDarkGruen(); } }

        public static DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(MaximizeButtonControl));
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        private SolidColorBrush SetDarkGruen()
        {
            MediaColor gruenColor = (MediaColor)Application.Current.Resources["GruenColor"];
            Color color = Color.FromArgb(gruenColor.R, gruenColor.G, gruenColor.B);
            HSBColor activeColor = HSBColor.FromARGBColor(color);
            activeColor.Darken(16);
            color = activeColor.ToARGBColor();
            MediaColor brushColor = MediaColor.FromArgb(color.A, color.R, color.G, color.B);
            return new SolidColorBrush(brushColor);
        }
    }
}

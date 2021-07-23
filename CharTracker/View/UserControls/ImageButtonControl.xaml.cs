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
using Color = System.Drawing.Color;
using MediaColor = System.Windows.Media.Color;

namespace CharTracker.View.UserControls
{
    /// <summary>
    /// Interaction logic for ImageButtonControl.xaml
    /// </summary>
    public partial class ImageButtonControl : UserControl
    {
        public ImageButtonControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        public SolidColorBrush BorderBackground { get; set; }
        public SolidColorBrush MouseOverBorderBackground { get { return SetMouseOverColor(); } }
        public SolidColorBrush DisabledBorderBackground { get { return SetDisabledColor(); } }

        public string Text { get; set; }
        public string Data { get; set; }

        public double BorderWidth { get; set; }
        public double BorderHeight { get; set; }

        public static DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(ImageButtonControl));
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        private SolidColorBrush SetMouseOverColor()
        {
            Color color = Color.FromArgb(BorderBackground.Color.R, BorderBackground.Color.G, BorderBackground.Color.B);
            HSBColor activeColor = HSBColor.FromARGBColor(color);
            activeColor.Darken(16);
            color = activeColor.ToARGBColor();
            MediaColor brushColor = MediaColor.FromArgb(color.A, color.R, color.G, color.B);
            return new SolidColorBrush(brushColor);
        }

        private SolidColorBrush SetDisabledColor()
        {
            Color color = Color.FromArgb(BorderBackground.Color.R, BorderBackground.Color.G, BorderBackground.Color.B);
            HSBColor activeColor = HSBColor.FromARGBColor(color);
            activeColor.Brighten(16);
            color = activeColor.ToARGBColor();
            MediaColor brushColor = MediaColor.FromArgb(color.A, color.R, color.G, color.B);
            return new SolidColorBrush(brushColor);
        }
    }
}

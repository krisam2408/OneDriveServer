using RetiraTracker.Core;
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

namespace RetiraTracker.View.UserControls
{
    /// <summary>
    /// Interaction logic for RectNumberControl.xaml
    /// </summary>
    public partial class RectNumberControl : UserControl
    {
        private readonly SolidColorBrush transparent = new SolidColorBrush(Color.FromArgb(30, 0, 0, 0));
        private readonly LinearGradientBrush slash;
        private Rectangle[] Rects;

        public EventHandler NumberChanged;

        public RectNumberControl()
        {
            InitializeComponent();

            Color transparentColor = transparent.Color;
            Color darkColor = (Color)Application.Current.Resources["DarkColor"];

            slash = new LinearGradientBrush(new GradientStopCollection(new List<GradientStop>
                {
                    new GradientStop(transparentColor, 0.4),
                    new GradientStop(darkColor, 0.47),
                    new GradientStop(darkColor, 0.63),
                    new GradientStop(transparentColor, 0.7)
                }),
                new Point(0, 0),
                new Point(1, 1)
            );

            Rects = new Rectangle[20] { Rect00, Rect01, Rect02, Rect03, Rect04, Rect05, Rect06, Rect07, Rect08, Rect09, Rect10, Rect11, Rect12, Rect13, Rect14, Rect15, Rect16, Rect17, Rect18, Rect19, };

            NumberChanged += (sender, e) =>
            {
                SetRectFills(Number);
                ValueChanged?.Execute(null);
            };
        }

        public static DependencyProperty NumberProperty = DependencyProperty.Register("Number", typeof(int), typeof(RectNumberControl), new PropertyMetadata(0, OnNumberChanged));
        public int Number
        {
            get
            {
                int val = (int)GetValue(NumberProperty);
                return val;
            }
            set
            {
                int input = 0;
                if (value > 0 && value <= 5)
                    input = value;

                SetValue(NumberProperty, input);
            }
        }

        private static void OnNumberChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RectNumberControl source = (RectNumberControl)d;

            if (source.NumberChanged != null)
                source.NumberChanged(source, EventArgs.Empty);
        }

        public static DependencyProperty ValueChangedProperty = DependencyProperty.Register("ValueChanged", typeof(ICommand), typeof(RectNumberControl));
        public ICommand ValueChanged
        {
            get { return (ICommand)GetValue(ValueChangedProperty); }
            set { SetValue(ValueChangedProperty, value); }
        }

        public ICommand Rect00Command
        {
            get
            {
                return new RelayCommand((e) =>
                {
                    if (Number >= 1)
                    {
                        Number = 0;
                        return;
                    }
                    Number = 1;
                });
            }
        }
        public ICommand Rect01Command { get { return new RelayCommand((e) => { Number = 2; }); } }
        public ICommand Rect02Command { get { return new RelayCommand((e) => { Number = 3; }); } }
        public ICommand Rect03Command { get { return new RelayCommand((e) => { Number = 4; }); } }
        public ICommand Rect04Command { get { return new RelayCommand((e) => { Number = 5; }); } }
        public ICommand Rect05Command { get { return new RelayCommand((e) => { Number = 6; }); } }
        public ICommand Rect06Command { get { return new RelayCommand((e) => { Number = 7; }); } }
        public ICommand Rect07Command { get { return new RelayCommand((e) => { Number = 8; }); } }
        public ICommand Rect08Command { get { return new RelayCommand((e) => { Number = 9; }); } }
        public ICommand Rect09Command { get { return new RelayCommand((e) => { Number = 10; }); } }
        public ICommand Rect10Command { get { return new RelayCommand((e) => { Number = 11; }); } }
        public ICommand Rect11Command { get { return new RelayCommand((e) => { Number = 12; }); } }
        public ICommand Rect12Command { get { return new RelayCommand((e) => { Number = 13; }); } }
        public ICommand Rect13Command { get { return new RelayCommand((e) => { Number = 14; }); } }
        public ICommand Rect14Command { get { return new RelayCommand((e) => { Number = 15; }); } }
        public ICommand Rect15Command { get { return new RelayCommand((e) => { Number = 16; }); } }
        public ICommand Rect16Command { get { return new RelayCommand((e) => { Number = 17; }); } }
        public ICommand Rect17Command { get { return new RelayCommand((e) => { Number = 18; }); } }
        public ICommand Rect18Command { get { return new RelayCommand((e) => { Number = 19; }); } }
        public ICommand Rect19Command { get { return new RelayCommand((e) => { Number = 20; }); } }

        public void SetRectFills(int val = 0)
        {
            for (int i = 0; i < Rects.Length; i++)
                Rects[i].Fill = transparent;

            for (int i = 0; i < val; i++)
                Rects[i].Fill = slash;
        }
    }
}

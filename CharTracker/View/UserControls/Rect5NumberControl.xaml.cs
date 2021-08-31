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
    /// Interaction logic for Rect5NumberControl.xaml
    /// </summary>
    public partial class Rect5NumberControl : UserControl
    {
        private readonly SolidColorBrush transparent = new SolidColorBrush(Color.FromArgb(30, 0, 0, 0));
        private readonly SolidColorBrush dark = (SolidColorBrush)Application.Current.Resources["Dark"];
        private Rectangle[] Rects;

        public EventHandler NumberChanged;

        public Rect5NumberControl()
        {
            InitializeComponent();

            Rects = new Rectangle[5] { Rect0, Rect1, Rect2, Rect3, Rect4 };

            NumberChanged += (sender, e) =>
            {
                SetRectFills(Number);
                ValueChanged?.Execute(null);
            };
        }

        public static DependencyProperty NumberProperty = DependencyProperty.Register("Number", typeof(int), typeof(Rect5NumberControl), new PropertyMetadata(0, OnNumberChanged));
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
            Rect5NumberControl source = (Rect5NumberControl)d;

            if (source.NumberChanged != null)
                source.NumberChanged(source, EventArgs.Empty);
        }

        public static DependencyProperty ValueChangedProperty = DependencyProperty.Register("ValueChanged", typeof(ICommand), typeof(Rect5NumberControl));
        public ICommand ValueChanged
        {
            get { return (ICommand)GetValue(ValueChangedProperty); }
            set { SetValue(ValueChangedProperty, value); }
        }

        public ICommand Rect0Command
        {
            get
            {
                return new RelayCommand((e) =>
                {
                    if(Number >= 1)
                    {
                        Number = 0;
                        return;
                    }
                    Number = 1;
                });
            }
        }
        public ICommand Rect1Command { get { return new RelayCommand((e) => { Number = 2; }); } }
        public ICommand Rect2Command { get { return new RelayCommand((e) => { Number = 3; }); } }
        public ICommand Rect3Command { get { return new RelayCommand((e) => { Number = 4; }); } }
        public ICommand Rect4Command { get { return new RelayCommand((e) => { Number = 5; }); } }

        public void SetRectFills(int val = 0)
        {
            for (int i = 0; i < 5; i++)
                Rects[i].Fill = transparent;

            for (int i = 0; i < val; i++)
                Rects[i].Fill = dark;
        }
    }
}

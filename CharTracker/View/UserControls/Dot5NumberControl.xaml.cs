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
    /// Interaction logic for Dot5NumberControl.xaml
    /// </summary>
    public partial class Dot5NumberControl : UserControl
    {
        private readonly SolidColorBrush transparent = new SolidColorBrush(Color.FromArgb(30, 0, 0, 0));
        private readonly SolidColorBrush dark = (SolidColorBrush)Application.Current.Resources["Dark"];
        private Ellipse[] Dots;

        public EventHandler NumberChanged;

        public Dot5NumberControl()
        {
            InitializeComponent();

            Dots = new Ellipse[5] { Dot0, Dot1, Dot2, Dot3, Dot4 };

            NumberChanged += (sender, e) =>
            {
                SetDotFills(Number);
                ValueChanged?.Execute(null);
            };
        }

        public static DependencyProperty NumberProperty = DependencyProperty.Register("Number", typeof(int), typeof(Dot5NumberControl), new PropertyMetadata(0, OnNumberChanged));
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
                if (value > 0 && value <= 10)
                    input = value;

                SetValue(NumberProperty, input);
            }
        }

        private static void OnNumberChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Dot5NumberControl source = (Dot5NumberControl)d;

            if (source.NumberChanged != null)
                source.NumberChanged(source, EventArgs.Empty);
        }

        public static DependencyProperty ValueChangedProperty = DependencyProperty.Register("ValueChanged", typeof(ICommand), typeof(Dot5NumberControl));
        public ICommand ValueChanged
        {
            get { return (ICommand)GetValue(ValueChangedProperty); }
            set { SetValue(ValueChangedProperty, value); }
        }

        public ICommand Dot0Command
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
        public ICommand Dot1Command { get { return new RelayCommand((e) => { Number = 2; }); } }
        public ICommand Dot2Command { get { return new RelayCommand((e) => { Number = 3; }); } }
        public ICommand Dot3Command { get { return new RelayCommand((e) => { Number = 4; }); } }
        public ICommand Dot4Command { get { return new RelayCommand((e) => { Number = 5; }); } }

        public void SetDotFills(int val = 0)
        {
            for (int i = 0; i < Dots.Length; i++)
                Dots[i].Fill = transparent;

            for (int i = 0; i < val; i++)
                Dots[i].Fill = dark;
        }
    }
}

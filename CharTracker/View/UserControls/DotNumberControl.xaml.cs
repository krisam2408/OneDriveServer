using RetiraTracker.Core;
using RetiraTracker.Model;
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
    /// Interaction logic for DotNumberControl.xaml
    /// </summary>
    public partial class DotNumberControl : UserControl
    {
        private readonly SolidColorBrush transparent = new SolidColorBrush(Color.FromArgb(30, 0, 0, 0));
        private readonly SolidColorBrush dark = (SolidColorBrush)Application.Current.Resources["Dark"];
        private Ellipse[] Dots;

        public EventHandler NumberChanged;

        public DotNumberControl()
        {
            InitializeComponent();

            Dots = new Ellipse[10] { Dot0, Dot1, Dot2, Dot3, Dot4, Dot5, Dot6, Dot7, Dot8, Dot9 };

            NumberChanged += (sender, e) =>
            {
                SetDotFills(Number);
                ValueChanged?.Execute(null);
            };

            
        }

        public static DependencyProperty NumberProperty = DependencyProperty.Register("Number", typeof(int), typeof(DotNumberControl), new PropertyMetadata(0, OnNumberChanged));
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

        private static void OnNumberChanged (DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DotNumberControl source = (DotNumberControl)d;

            if (source.NumberChanged != null)
                source.NumberChanged(source, EventArgs.Empty);
        }

        public static DependencyProperty ValueChangedProperty = DependencyProperty.Register("ValueChanged", typeof(ICommand), typeof(DotNumberControl));
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
                    if(Number >= 1)
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
        public ICommand Dot5Command { get { return new RelayCommand((e) => { Number = 6; }); } }
        public ICommand Dot6Command { get { return new RelayCommand((e) => { Number = 7; }); } }
        public ICommand Dot7Command { get { return new RelayCommand((e) => { Number = 8; }); } }
        public ICommand Dot8Command { get { return new RelayCommand((e) => { Number = 9; }); } }
        public ICommand Dot9Command { get { return new RelayCommand((e) => { Number = 10; }); } }

        public void SetDotFills(int val = 0)
        {
            for (int i = 0; i < 10; i++)
                Dots[i].Fill = transparent;

            for (int i = 0; i < val; i++)
                Dots[i].Fill = dark;
        }
    }
}

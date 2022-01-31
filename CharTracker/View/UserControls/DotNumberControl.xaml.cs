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
    /// Interaction logic for DotNumberControl.xaml
    /// </summary>
    public partial class DotNumberControl : UserControl
    {
        private readonly SolidColorBrush transparent = new SolidColorBrush(Color.FromArgb(30, 0, 0, 0));
        private readonly SolidColorBrush dark = (SolidColorBrush)Application.Current.Resources["Dark"];
        private readonly SolidColorBrush orange = (SolidColorBrush)Application.Current.Resources["Orange"];
        private readonly SolidColorBrush gruen = (SolidColorBrush)Application.Current.Resources["Gruen"];
        private readonly SolidColorBrush rot = (SolidColorBrush)Application.Current.Resources["Rot"];
        private Ellipse[] Dots;

        public EventHandler NumberChanged;

        public DotNumberControl()
        {
            InitializeComponent();

            Dots = new Ellipse[10] { Dot0, Dot1, Dot2, Dot3, Dot4, Dot5, Dot6, Dot7, Dot8, Dot9 };

            NumberChanged += OnValuesChanged;
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
                int input = CalculateValue(value, DotNumberControlValueType.Number);

                SetValue(NumberProperty, input);
            }
        }

        private static void OnNumberChanged (DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DotNumberControl source = (DotNumberControl)d;

            if (source.NumberChanged != null)
                source.NumberChanged(source, EventArgs.Empty);
        }

        public static DependencyProperty BonusProperty = DependencyProperty.Register("Bonus", typeof(int), typeof(DotNumberControl), new PropertyMetadata(0, OnNumberChanged));
        public int Bonus
        {
            get
            {
                int val = (int)GetValue(BonusProperty);
                return val;
            }
            set
            {
                int input = CalculateValue(value, DotNumberControlValueType.Bonus);

                SetValue(BonusProperty, input);
            }
        }

        public static DependencyProperty AutoBonusProperty = DependencyProperty.Register("AutoBonus", typeof(int), typeof(DotNumberControl), new PropertyMetadata(0, OnNumberChanged));
        public int AutoBonus
        {
            get
            {
                int val = (int)GetValue(AutoBonusProperty);
                
                return val;
            }
            set
            {
                int input = CalculateValue(value, DotNumberControlValueType.AutoBonus);
                
                SetValue(AutoBonusProperty, input);
            }
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

        public ICommand BonusDot0Command { get { return new RelayCommand((e) => { Bonus = CalculateBonus(1); }); } }
        public ICommand BonusDot1Command { get { return new RelayCommand((e) => { Bonus = CalculateBonus(2); }); } }
        public ICommand BonusDot2Command { get { return new RelayCommand((e) => { Bonus = CalculateBonus(3); }); } }
        public ICommand BonusDot3Command { get { return new RelayCommand((e) => { Bonus = CalculateBonus(4); }); } }
        public ICommand BonusDot4Command { get { return new RelayCommand((e) => { Bonus = CalculateBonus(5); }); } }
        public ICommand BonusDot5Command { get { return new RelayCommand((e) => { Bonus = CalculateBonus(6); }); } }
        public ICommand BonusDot6Command { get { return new RelayCommand((e) => { Bonus = CalculateBonus(7); }); } }
        public ICommand BonusDot7Command { get { return new RelayCommand((e) => { Bonus = CalculateBonus(8); }); } }
        public ICommand BonusDot8Command { get { return new RelayCommand((e) => { Bonus = CalculateBonus(9); }); } }
        public ICommand BonusDot9Command { get { return new RelayCommand((e) => { Bonus = CalculateBonus(10); }); } }

        public void SetDotFills()
        {
            for (int i = 0; i < Dots.Length; i++)
                Dots[i].Fill = transparent;

            for (int i = 0; i < Number && i < 10; i++)
                Dots[i].Fill = dark;

            SetAutoBonusDots();

            SetBonusDots();
        }

        public void SetAutoBonusDots()
        {
            if(AutoBonus < 0)
            {
                for (int i = Number + AutoBonus; i < Number && i < 10; i++)
                    Dots[i].Fill = rot;
                return;
            }

            for (int i = Number; i < Number + AutoBonus && i < 10; i++)
                Dots[i].Fill = gruen;
        }

        private void SetBonusDots()
        {
            if(AutoBonus < 0)
            {
                for (int i = Number + AutoBonus; i < Number + AutoBonus + Bonus && i < 10; i++)
                    Dots[i].Fill = orange;
                return;
            }

            if(Bonus < 0)
            {
                for (int i = Number + AutoBonus + Bonus; i < Number + AutoBonus && i < 10; i++)
                    Dots[i].Fill = rot;
                return;
            }

            for (int i = Number + AutoBonus; i < Number + AutoBonus + Bonus && i < 10; i++)
                Dots[i].Fill = orange;
        }

        private int CalculateValue(int val, DotNumberControlValueType type = DotNumberControlValueType.Number)
        {
            int maxLimit = 10;
            int input = val;

            switch(type)
            {
                case DotNumberControlValueType.Number:
                    if (input < 0)
                        input = 0;
                    break;
                case DotNumberControlValueType.AutoBonus:
                    maxLimit -= Number;

                    if (input + Number < 0)
                        input = -Number;
                    break;
                case DotNumberControlValueType.Bonus:
                    maxLimit -= Number + AutoBonus;

                    if (input + Number < 0)
                        input = -(Number + AutoBonus);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type));
            }

            if (input > maxLimit)
                input = maxLimit;

            return input;
        }

        private int CalculateBonus(int val)
        {
            int input = val - (Number + AutoBonus + 1);

            if (input >= 0)
                input++;

            if(input == 1 && Bonus > 0)
                input = 0;

            return input;
        }

        private void OnValuesChanged(object sender, EventArgs e)
        {
            SetDotFills();
            ValueChanged?.Execute(null);
        }
    }
}

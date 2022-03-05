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
        private readonly SolidColorBrush m_transparent = new SolidColorBrush(Color.FromArgb(30, 0, 0, 0));
        private readonly SolidColorBrush m_dark = (SolidColorBrush)Application.Current.Resources["Dark"];
        private readonly SolidColorBrush m_orange = (SolidColorBrush)Application.Current.Resources["Orange"];
        private readonly SolidColorBrush m_gruen = (SolidColorBrush)Application.Current.Resources["Gruen"];
        private readonly SolidColorBrush m_rot = (SolidColorBrush)Application.Current.Resources["Rot"];

        public EventHandler NumberChanged;

        public DotNumberControl()
        {
            InitializeComponent();

            NumberChanged += (sender, e) =>
            {
                SetDotFills();
                ValueChanged?.Execute(null);
            };

            SetDotFills();
        }

        public static DependencyProperty MinNumberProperty = DependencyProperty.Register("MinNumber", typeof(int), typeof(DotNumberControl), new PropertyMetadata(0, OnNumberChanged));
        public int MinNumber
        {
            get
            {
                int val = (int)GetValue(MinNumberProperty);
                return val;
            }
            set
            {
                int input = value;
                SetValue(MinNumberProperty, input);
            }
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

        public static DependencyProperty MaxNumberProperty = DependencyProperty.Register("MaxNumber", typeof(int), typeof(DotNumberControl), new PropertyMetadata(10, OnNumberChanged));
        public int MaxNumber
        {
            get { return (int)GetValue(MaxNumberProperty); }
            set { SetValue(MaxNumberProperty, value); }
        }

        public static DependencyProperty AcceptsBonusesProperty = DependencyProperty.Register("AcceptsBonuses", typeof(bool), typeof(DotNumberControl), new PropertyMetadata(false));
        public bool AcceptsBonuses
        {
            get { return (bool)GetValue(AcceptsBonusesProperty); }
            set { SetValue(AcceptsBonusesProperty, value); }
        }

        public static DependencyProperty ValueChangedProperty = DependencyProperty.Register("ValueChanged", typeof(ICommand), typeof(DotNumberControl));
        public ICommand ValueChanged
        {
            get { return (ICommand)GetValue(ValueChangedProperty); }
            set { SetValue(ValueChangedProperty, value); }
        }

        public void SetDotFills()
        {
            Panel.Children.Clear();

            int val = 0;
            int numberValue = Number;
            int totalBonuses = Bonus + AutoBonus;
            if (totalBonuses < 0)
                numberValue += totalBonuses;

            for(int i = val; i < numberValue && i < MaxNumber; i++)
            {
                val++;
                Panel.Children.Add(DefaultDot(m_dark, val));
            }

            for(int i = 0; totalBonuses < 0 && i < Math.Abs(totalBonuses) && i < MaxNumber; i++)
            {
                val++;
                Panel.Children.Add(DefaultDot(m_rot, val));
            }

            for(int i = 0; totalBonuses > 0 && i < AutoBonus && i < MaxNumber; i++)
            {
                val++;
                Panel.Children.Add(DefaultDot(m_gruen, val));
            }

            for(int i = 0; totalBonuses > 0 && i < Bonus && i < MaxNumber; i++)
            {
                val++;
                Panel.Children.Add(DefaultDot(m_orange, val));
            }

            for(int i = val; i < MaxNumber; i++)
            {
                val++;
                Panel.Children.Add(DefaultDot(m_transparent, val));
            }
        }

        private int CalculateValue(int val, DotNumberControlValueType type = DotNumberControlValueType.Number)
        {
            int maxLimit = MaxNumber;
            int input = val;

            switch(type)
            {
                case DotNumberControlValueType.Number:
                    if (input < MinNumber)
                        input = MinNumber;
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

            if(input == Bonus)
                input = 0;

            return input;
        }

        private int CalculateNumber(int val)
        {
            if (val == MinNumber + 1 && Number > MinNumber)
                return MinNumber;
            return val;
        }

        private Ellipse DefaultDot(Brush fill, int val)
        {
            Ellipse dot = new();
            dot.Stroke = (SolidColorBrush)Application.Current.Resources["Dark"];
            dot.StrokeThickness = 1.5;
            dot.Width = 16;
            dot.Height = 16;
            dot.Fill = fill;
            dot.Margin = new Thickness(1);

            if(val > MinNumber || AcceptsBonuses)
            {
                dot.Cursor = Cursors.Hand;
                dot.IsMouseDirectlyOverChanged += (sender, e) =>
                {
                    Ellipse d = (Ellipse)sender;
                    bool mouseOver = (bool)e.NewValue;
                    if(mouseOver)
                    {
                        d.Opacity = 0.8;
                        return;
                    }
                    d.Opacity = 1;
                };
            }

            if(val > MinNumber)
            {
                dot.MouseLeftButtonUp += (sender, e) =>
                {
                    Number = CalculateNumber(val);
                };
            }

            if(AcceptsBonuses)
            {
                dot.MouseRightButtonUp += (sender, e) =>
                {
                    Bonus = CalculateBonus(val);
                };
            }

            return dot;
        }

        private enum DotNumberControlValueType { Number, Bonus, AutoBonus }
    }
}

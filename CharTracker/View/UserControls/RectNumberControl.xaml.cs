using RetiraTracker.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private readonly SolidColorBrush m_transparent = new SolidColorBrush(Color.FromArgb(30, 0, 0, 0));
        private readonly SolidColorBrush m_disabled;
        private readonly LinearGradientBrush m_slash;
        
        public EventHandler NumberChanged;
        public EventHandler MaxNumberChanged;

        public RectNumberControl()
        {
            InitializeComponent();

            Color transparentColor = m_transparent.Color;
            Color darkColor = (Color)Application.Current.Resources["DarkColor"];

            m_slash = new LinearGradientBrush(new GradientStopCollection(new List<GradientStop>
                {
                    new GradientStop(transparentColor, 0.4),
                    new GradientStop(darkColor, 0.47),
                    new GradientStop(darkColor, 0.63),
                    new GradientStop(transparentColor, 0.7)
                }),
                new Point(0, 0),
                new Point(1, 1)
            );

            Color disabledColor = (Color)Application.Current.Resources["GrauColor"];
            disabledColor.A = 162;
            m_disabled = new SolidColorBrush(disabledColor);

            NumberChanged += (sender, e) =>
            {
                SetRectFills();
                ValueChanged?.Execute(null);
            };

            MaxNumberChanged += (sender, e) =>
            {
                SetRectFills();
            };

            SetRectFills();
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
                if (value > 0)
                    input = value;
                if(value > MaxNumber)
                    input = MaxNumber;

                SetValue(NumberProperty, input);
            }
        }

        private static void OnNumberChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RectNumberControl source = (RectNumberControl)d;

            source.NumberChanged?.Invoke(source, EventArgs.Empty);
        }

        public static DependencyProperty MaxNumberProperty = DependencyProperty.Register("MaxNumber", typeof(int), typeof(RectNumberControl), new PropertyMetadata(0, OnMaxNumberChanged));
        public int MaxNumber
        {
            get
            {
                int val = (int)GetValue(MaxNumberProperty);
                return val;
            }
            set
            {
                int input = 0;
                if (value > 0)
                    input = value;
                if(value > 100)
                    input = 100;

                SetValue(MaxNumberProperty, input);
            }
        }

        private static void OnMaxNumberChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RectNumberControl source = (RectNumberControl)d;

            source.MaxNumberChanged?.Invoke(source, EventArgs.Empty);
        }

        public static DependencyProperty ValueChangedProperty = DependencyProperty.Register("ValueChanged", typeof(ICommand), typeof(RectNumberControl));
        public ICommand ValueChanged
        {
            get { return (ICommand)GetValue(ValueChangedProperty); }
            set { SetValue(ValueChangedProperty, value); }
        }

        private int CalculateNumber(int val)
        {
            if (val == 1 && Number >= 1)
                return 0;
            return val;
        }

        public void SetRectFills()
        {
            Panel.Children.Clear();

            int val = 0;
            int maxVal = SetMaxValue();

            for (int i = 0; i < Number; i++)
            {
                val++;
                Panel.Children.Add(DefaultRect(m_slash, val));
            }

            for(int i = Number; i < MaxNumber; i++)
            {
                val++;
                Panel.Children.Add(DefaultRect(m_transparent, val));
            }

            for(int i = MaxNumber; i < maxVal; i++)
            {
                val++;
                Panel.Children.Add(DefaultRect(m_disabled, val));
            }
        }

        private Rectangle DefaultRect(Brush fill, int val)
        {
            Rectangle rect = new();

            rect.Stroke = (SolidColorBrush)Application.Current.Resources["Dark"];
            rect.StrokeThickness = 1.5;
            rect.Width = 16;
            rect.Height = 16;
            rect.Fill = fill;
            rect.Margin = new Thickness(3);

            if(fill != m_disabled)
            {
                rect.Cursor = Cursors.Hand;
                rect.IsMouseDirectlyOverChanged += (sender, e) =>
                {
                    Rectangle r = (Rectangle)sender;
                    bool mouseOver = (bool)e.NewValue;
                    if (mouseOver)
                    {
                        r.Opacity = 0.8;
                        return;
                    }

                    r.Opacity = 1;
                };
                rect.MouseLeftButtonUp += (sender, e) =>
                {
                    Number = CalculateNumber(val);
                };
            }

            return rect;
        }

        private int SetMaxValue()
        {
            if(MaxNumber < 10)
                return MaxNumber;

            int output = 10;
            while (MaxNumber > output && MaxNumber <= 100)
                output += 10;

            return output;
        }
    }
}

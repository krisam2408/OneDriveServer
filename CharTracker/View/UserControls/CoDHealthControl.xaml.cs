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
    /// Interaction logic for HealthControl.xaml
    /// </summary>
    public partial class CoDHealthControl : UserControl
    {
        private readonly SolidColorBrush transparent = new SolidColorBrush(Color.FromArgb(30, 0, 0, 0));
        private readonly SolidColorBrush dark = (SolidColorBrush)Application.Current.Resources["Dark"];
        private readonly LinearGradientBrush slash;
        private readonly LinearGradientBrush half;
        private Ellipse[] Dots;
        private Rectangle[] Rects;

        public EventHandler MaxHealthChanged;
        public EventHandler DamageChanged;

        public CoDHealthControl()
        {
            InitializeComponent();

            Color darkColor = dark.Color;
            Color transparentColor = transparent.Color;
            
            slash = new LinearGradientBrush(new GradientStopCollection(new List<GradientStop>
                { 
                    new GradientStop(transparentColor, 0.4),
                    new GradientStop(darkColor, 0.45),
                    new GradientStop(darkColor, 0.65),
                    new GradientStop(transparentColor, 0.6)
                }),
                new Point(0, 1),
                new Point(1, 0)
            );

            half = new LinearGradientBrush(new GradientStopCollection(new List<GradientStop>
                {
                    new GradientStop(transparentColor, 0.49),
                    new GradientStop(darkColor, 0.51)
                }),
                new Point(0, 1),
                new Point(1, 0)
            );

            Dots = new Ellipse[12] { Dot00, Dot01, Dot02, Dot03, Dot04, Dot05, Dot06, Dot07, Dot08, Dot09, Dot10, Dot11 };
            Rects = new Rectangle[] { Rect00 };

            MaxHealthChanged += (sender, e) =>
            {
                SetDotsFills(MaxHealth);
            };

            DamageChanged += (sender, e) =>
            {
                SetRectsFills(Damage);
                ValueChanged?.Execute(null);
            };
        }

        public static DependencyProperty MaxHealthProperty = DependencyProperty.Register("MaxHealth", typeof(int), typeof(CoDHealthControl), new PropertyMetadata(0, OnMaxHealthChanged));
        public int MaxHealth
        {
            get
            {
                int val = (int)GetValue(MaxHealthProperty);
                return val;
            }
            set
            {
                SetValue(MaxHealthProperty, value);
            }
        }

        private static void OnMaxHealthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CoDHealthControl source = (CoDHealthControl)d;

            if (source.MaxHealthChanged != null)
                source.MaxHealthChanged(source, EventArgs.Empty);
        }

        public static DependencyProperty DamageProperty = DependencyProperty.Register("Damage", typeof(string), typeof(CoDHealthControl), new PropertyMetadata(string.Empty, OnDamageChanged));
        public string Damage
        {
            get
            {
                string val = (string)GetValue(DamageProperty);
                return val;
            }
            set
            {
                SetValue(DamageProperty, value);
            }
        }

        private static void OnDamageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CoDHealthControl source = (CoDHealthControl)d;

            if (source.DamageChanged != null)
                source.DamageChanged(source, EventArgs.Empty);
        }

        public static DependencyProperty ValueChangedProperty = DependencyProperty.Register("ValueChanged", typeof(ICommand), typeof(Rect5NumberControl));
        public ICommand ValueChanged
        {
            get { return (ICommand)GetValue(ValueChangedProperty); }
            set { SetValue(ValueChangedProperty, value); }
        }

        public ICommand Rect00Command { get { return new RelayCommand((e) => { TranslateDamage(0); }); } }


        private void SetDotsFills(int val = 0)
        {
            for (int i = 0; i < 12; i++)
                Dots[i].Fill = transparent;

            for (int i = 0; i < val; i++)
                Dots[i].Fill = dark;
        }

        private void SetRectsFills(string val = "")
        {

        }

        private void TranslateDamage(int pos)
        {

        }
    }
}

using RetiraTracker.Core;
using RetiraTracker.Core.Abstracts;
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
using System.Windows.Threading;

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

        public EventHandler MaxHealthChanged;
        public EventHandler DamageChanged;

        public ObservableCollection<EnumeratedBrush> DamageMeter { get; private set; }
        
        public CoDHealthControl()
        {
            InitializeComponent();

            Color darkColor = dark.Color;
            Color transparentColor = transparent.Color;
            
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

            half = new LinearGradientBrush(new GradientStopCollection(new List<GradientStop>
                {
                    new GradientStop(transparentColor, 0.49),
                    new GradientStop(darkColor, 0.51)
                }),
                new Point(0, 0),
                new Point(1, 1)
            );

            MaxHealthChanged += (sender, e) =>
            {
                TranslateDamage();
                PenTextBlock.Text = Penalties();
            };

            DamageChanged += (sender, e) =>
            {
                TranslateDamage();
                PenTextBlock.Text = Penalties();
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
            set { SetValue(MaxHealthProperty, value); }
        }

        private static void OnMaxHealthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CoDHealthControl source = (CoDHealthControl)d;

            if (source.MaxHealthChanged != null)
                source.MaxHealthChanged(source, EventArgs.Empty);
        }

        public static DependencyProperty DamageProperty = DependencyProperty.Register("Damage", typeof(List<char>), typeof(CoDHealthControl), new PropertyMetadata(new List<char>(), OnDamageChanged));
        public List<char> Damage
        {
            get
            {
                List<char> val = (List<char>)GetValue(DamageProperty);
                return val;
            }
            set { SetValue(DamageProperty, value); }
        }

        private static void OnDamageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CoDHealthControl source = (CoDHealthControl)d;

            if (source.DamageChanged != null)
                source.DamageChanged(source, EventArgs.Empty);
        }

        public static DependencyProperty ValueChangedProperty = DependencyProperty.Register("ValueChanged", typeof(ICommand), typeof(CoDHealthControl));
        public ICommand ValueChanged
        {
            get { return (ICommand)GetValue(ValueChangedProperty); }
            set { SetValue(ValueChangedProperty, value); }
        }

        public static DependencyProperty CalculatePenaltiesProperty = DependencyProperty.Register("CalculatePenalties", typeof(bool), typeof(CoDHealthControl), new PropertyMetadata(true));
        public bool CalculatePenalties
        {
            get { return (bool)GetValue(CalculatePenaltiesProperty); }
            set { SetValue(CalculatePenaltiesProperty, value); }
        }

        public ICommand HealthUpCommand
        {
            get
            {
                return new RelayCommand((e) =>
                {
                    MouseEventArgs me = (MouseEventArgs)e;
                    Path originalSource = (Path)me.OriginalSource;
                    Grid sourceParent = (Grid)originalSource.Parent;
                    TextBlock tblock = (TextBlock)sourceParent.Children[1];
                    int index = int.Parse(tblock.Text);
                    DamageUp(index);

                });
            }
        }

        public ICommand HealthDownCommand
        {
            get
            {
                return new RelayCommand((e) =>
                {
                    MouseEventArgs me = (MouseEventArgs)e;
                    Path originalSource = (Path)me.OriginalSource;
                    Grid sourceParent = (Grid)originalSource.Parent;
                    TextBlock tblock = (TextBlock)sourceParent.Children[1];
                    int index = int.Parse(tblock.Text);
                    DamageDown(index);
                });
            }
        }

        private void DamageUp(int index)
        {
            char target = Damage[index];

            char output = target switch
            {
                ' ' => '/',
                '/' => 'X',
                'X' => '*',
                _ => ' '
            };

            Damage[index] = output;
            DamageChanged(null, EventArgs.Empty);
        }

        private void DamageDown(int index)
        {
            char target = Damage[index];

            char output = target switch
            {
                ' ' => '*',
                '/' => ' ',
                'X' => '/',
                '*' => 'X',
                _ => ' '
            };

            Damage[index] = output;
            DamageChanged.Invoke(null, null);
        }

        private void TranslateDamage()
        {
            if(Damage.Count != MaxHealth)
            {
                char[] tDmg = Damage.ToArray();
                Damage.Clear();
                for (int i = 0; i < MaxHealth; i++)
                {
                    if (i >= tDmg.Length)
                        Damage.Add(' ');
                    else
                        Damage.Add(tDmg[i]);
                }
            }

            if (DamageMeter == null)
                DamageMeter = new();
            DamageMeter.Clear();
            for(int i = 0; i < MaxHealth; i++)
            {
                switch(Damage[i])
                {
                    case ' ':
                        DamageMeter.Add(new EnumeratedBrush(i, ' ', transparent));
                        break;
                    case '/':
                        DamageMeter.Add(new EnumeratedBrush(i, '/', slash));
                        break;
                    case 'X':
                        DamageMeter.Add(new EnumeratedBrush(i, 'X', half));
                        break;
                    case '*':
                    default:
                        DamageMeter.Add(new EnumeratedBrush(i, '*', dark));
                        break;
                }
            }
        }

        private string Penalties()
        {
            if(CalculatePenalties)
            {
                if (Damage[^1] != ' ')
                    return "-3";
                if (Damage.Count >= 2 && Damage[^2] != ' ')
                    return "-2";
                if (Damage.Count >= 3 && Damage[^3] != ' ')
                    return "-1";
            }
            return "0";
        }
    }
}

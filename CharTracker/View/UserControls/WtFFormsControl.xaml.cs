using RetiraTracker.Core;
using SheetDrama;
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
    /// Interaction logic for WtFFormsControl.xaml
    /// </summary>
    public partial class WtFFormsControl : UserControl
    {
        private readonly SolidColorBrush transparent = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        private readonly SolidColorBrush activated = (SolidColorBrush)Application.Current.Resources["Orange"];
        private readonly SolidColorBrush light = (SolidColorBrush)Application.Current.Resources["Light"];
        private Border[] Buttons;

        public EventHandler FormsChanged;

        public WtFFormsControl()
        {
            InitializeComponent();

            Buttons = new Border[5] { HishuButton, DaluButton, GauruButton, UrshulButton, UrhanButton };

            foreach(Border border in Buttons)
                border.BorderBrush = activated;

            SetButtonsColors(WerewolfForms.Hishu);

            FormsChanged += (sender, e) =>
            {
                SetButtonsColors(Forms);
                ValueChanged?.Execute(null);
            };
        }

        public static DependencyProperty FormsProperty = DependencyProperty.Register("Forms", typeof(WerewolfForms), typeof(WtFFormsControl), new PropertyMetadata(WerewolfForms.Hishu, OnFormsChanged));
        public WerewolfForms Forms
        {
            get
            {
                WerewolfForms val = (WerewolfForms)GetValue(FormsProperty);
                return val;
            }
            set { SetValue(FormsProperty, value); }
        }

        private static void OnFormsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WtFFormsControl source = (WtFFormsControl)d;

            if(source.FormsChanged != null)
                source.FormsChanged(source, EventArgs.Empty);
        }

        public static DependencyProperty ValueChangedProperty = DependencyProperty.Register("ValueChanged", typeof(ICommand), typeof(WtFFormsControl));
        public ICommand ValueChanged
        {
            get { return (ICommand)GetValue(ValueChangedProperty); }
            set { SetValue(ValueChangedProperty, value); }
        }

        public ICommand Button00Command { get { return new RelayCommand((e) => { Forms = WerewolfForms.Hishu; }); } }
        public ICommand Button01Command { get { return new RelayCommand((e) => { Forms = WerewolfForms.Dalu; }); } }
        public ICommand Button02Command { get { return new RelayCommand((e) => { Forms = WerewolfForms.Gauru; }); } }
        public ICommand Button03Command { get { return new RelayCommand((e) => { Forms = WerewolfForms.Urshul; }); } }
        public ICommand Button04Command { get { return new RelayCommand((e) => { Forms = WerewolfForms.Urhan; }); } }

        private void SetButtonsColors(WerewolfForms form)
        {
            for(int i = 0; i < Buttons.Length; i++)
            {
                Buttons[i].Background = transparent;
                GetButtonText(Buttons[i]).Foreground = activated;
            }

            for(int i = 0; i < Buttons.Length; i++)
            {
                if(i == (int)form)
                {
                    Buttons[i].Background = activated;
                    GetButtonText(Buttons[i]).Foreground = light;
                }
            }
        }

        private TextBlock GetButtonText(Border button)
        {
            return (TextBlock)button.Child;
        }
    }
}

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
        private readonly SolidColorBrush m_activated = (SolidColorBrush)Application.Current.Resources["Gruen"];
        private readonly SolidColorBrush m_original = (SolidColorBrush)Application.Current.Resources["Blau"];
        private readonly SolidColorBrush m_mouseOver = (SolidColorBrush)Application.Current.Resources["Orange"];
        private readonly SolidColorBrush m_disabled = (SolidColorBrush)Application.Current.Resources["DarkLight"];
        private readonly Border[] m_buttons;

        public EventHandler FormsChanged;

        public WtFFormsControl()
        {
            InitializeComponent();

            m_buttons = new Border[5] { HishuButton, DaluButton, GauruButton, UrshulButton, UrhanButton };

            SetButtonMouseOver();

            FormsChanged += (sender, e) =>
            {
                SetButtonsColors();
                ValueChanged?.Execute(null);
            };

            IsEnabledChanged += (sender, e) =>
            {
                bool enabled = (bool)e.NewValue;
                if(enabled)
                {
                    SetButtonsColors();
                    return;
                }

                for(int i = 0; i < m_buttons.Length;i++)
                {
                    m_buttons[i].Background = m_disabled;
                    m_buttons[i].Cursor = Cursors.Arrow;
                }
            };
            
            SetButtonsColors();
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

        private void SetButtonsColors()
        {
            for(int i = 0; i < m_buttons.Length; i++)
                SetButtonColors(i);
        }

        private void SetButtonColors(int index)
        {
            if (index == (int)Forms)
            {
                m_buttons[index].Background = m_activated;
                m_buttons[index].Cursor = Cursors.Arrow;
                return;
            }

            m_buttons[index].Background = m_original;
            m_buttons[index].Cursor = Cursors.Hand;
        }

        private void SetButtonMouseOver()
        {
            for(int i = 0; i < m_buttons.Length; i++)
            {
                m_buttons[i].IsMouseDirectlyOverChanged += (sender, e) =>
                {
                    Border btn = (Border)sender;
                    TextBlock text = (TextBlock)btn.Child;
                    if (text.Text != Forms.ToString())
                    {
                        bool mouseOver = (bool)e.NewValue;
                        if (mouseOver)
                        {
                            btn.Background = m_mouseOver;
                            return;
                        }

                        btn.Background = m_original;
                    }
                };

                m_buttons[i].Child.IsMouseDirectlyOverChanged += (sender, e) =>
                {
                    TextBlock text = (TextBlock)sender;
                    if (text.Text != Forms.ToString())
                    {
                        Border parent = (Border)text.Parent;
                        bool mouseOver = (bool)e.NewValue;
                        if (mouseOver)
                        {
                            parent.Background = m_mouseOver;
                            return;
                        }

                        parent.Background = m_original;
                    }
                };
            }
        }
    }
}

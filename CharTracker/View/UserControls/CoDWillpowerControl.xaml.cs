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
    /// Interaction logic for CoDWillpowerControl.xaml
    /// </summary>
    public partial class CoDWillpowerControl : UserControl
    {
        private readonly SolidColorBrush m_transparent = new SolidColorBrush(Color.FromArgb(30, 0, 0, 0));
        private readonly SolidColorBrush m_dark = (SolidColorBrush)Application.Current.Resources["Dark"];
        private readonly LinearGradientBrush m_slash;

        private readonly Ellipse[] m_dots;
        private readonly Rectangle[] m_rects;

        public EventHandler MaxWillpowerChanged;
        public EventHandler WillpowerChanged;

        public CoDWillpowerControl()
        {
            InitializeComponent();

            Color darkColor = m_dark.Color;
            Color transparentColor = m_transparent.Color;

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

            m_dots = new Ellipse[10] { Dot0, Dot1, Dot2, Dot3, Dot4, Dot5, Dot6, Dot7, Dot8, Dot9 };
            m_rects = new Rectangle[10] { Rect0, Rect1, Rect2, Rect3, Rect4, Rect5, Rect6, Rect7, Rect8, Rect9 };

            MaxWillpowerChanged += (sender, e) =>
            {
                if (CurrentWillpower > MaxWillpower)
                    CurrentWillpower = MaxWillpower;

                SetDotFills(MaxWillpower);
                SetRectFills(CurrentWillpower);
            };

            WillpowerChanged += (sender, e) =>
            {
                SetRectFills(CurrentWillpower);
                ValueChanged?.Execute(null);
            };
        }

        public static DependencyProperty MaxWillpowerProperty = DependencyProperty.Register("MaxWillpower", typeof(int), typeof(CoDWillpowerControl), new PropertyMetadata(1, OnMaxWillpowerChanged));
        public int MaxWillpower
        {
            get
            {
                int val = (int)GetValue(MaxWillpowerProperty);
                return val;
            }
            set { SetValue(MaxWillpowerProperty, value); }
        }

        private static void OnMaxWillpowerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CoDWillpowerControl source = (CoDWillpowerControl)d;

            if (source.MaxWillpowerChanged != null)
                source.MaxWillpowerChanged(source, EventArgs.Empty);
        }

        public static DependencyProperty CurrentWillpowerProperty = DependencyProperty.Register("CurrentWillpower", typeof(int), typeof(CoDWillpowerControl), new PropertyMetadata(0, OnCurrentWillpowerChanged));
        public int CurrentWillpower
        {
            get
            {
                int val = (int)GetValue(CurrentWillpowerProperty);
                return val;
            }
            set { SetValue(CurrentWillpowerProperty, value); }
        }

        private static void OnCurrentWillpowerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CoDWillpowerControl source = (CoDWillpowerControl)d;

            if (source.WillpowerChanged != null)
                source.WillpowerChanged(source, EventArgs.Empty);
        }

        public static DependencyProperty ValueChangedProperty = DependencyProperty.Register("ValueChanged", typeof(ICommand), typeof(CoDWillpowerControl));
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
                    if(CurrentWillpower >= 1)
                    {
                        CurrentWillpower = 0;
                        return;
                    }

                    SetCurrentWillpower(1);
                });
            }
        }
        public ICommand Rect1Command { get { return new RelayCommand((e) => { SetCurrentWillpower(2); ; }); } }
        public ICommand Rect2Command { get { return new RelayCommand((e) => { SetCurrentWillpower(3); ; }); } }
        public ICommand Rect3Command { get { return new RelayCommand((e) => { SetCurrentWillpower(4); ; }); } }
        public ICommand Rect4Command { get { return new RelayCommand((e) => { SetCurrentWillpower(5); ; }); } }
        public ICommand Rect5Command { get { return new RelayCommand((e) => { SetCurrentWillpower(6); ; }); } }
        public ICommand Rect6Command { get { return new RelayCommand((e) => { SetCurrentWillpower(7); ; }); } }
        public ICommand Rect7Command { get { return new RelayCommand((e) => { SetCurrentWillpower(8); ; }); } }
        public ICommand Rect8Command { get { return new RelayCommand((e) => { SetCurrentWillpower(9); ; }); } }
        public ICommand Rect9Command { get { return new RelayCommand((e) => { SetCurrentWillpower(10); ; }); } }

        private void SetDotFills(int val = 0)
        {
            for (int i = 0; i < m_dots.Length; i++)
                m_dots[i].Fill = m_transparent;

            for (int i = 0; i < val; i++)
                m_dots[i].Fill = m_dark;
        }

        private void SetRectFills(int val = 0)
        {
            for (int i = 0; i < m_rects.Length; i++)
                m_rects[i].Fill = m_transparent;

            for (int i = 0; i < val; i++)
                m_rects[i].Fill = m_slash;
        }

        private void SetCurrentWillpower(int val)
        {
            if (MaxWillpower >= val)
                CurrentWillpower = val;
        }
    }
}

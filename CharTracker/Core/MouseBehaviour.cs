using System.Windows;
using System.Windows.Input;

namespace RetiraTracker.Core
{
    public class MouseBehaviour
    {
        public static readonly DependencyProperty MouseLeftUpCommandProperty = DependencyProperty.RegisterAttached("MouseLeftUpCommand", typeof(ICommand), typeof(MouseBehaviour), new FrameworkPropertyMetadata(new PropertyChangedCallback(MouseLeftUpCommandChanged)));
        public static readonly DependencyProperty MouseLeftDownCommandProperty = DependencyProperty.RegisterAttached("MouseLeftDownCommand", typeof(ICommand), typeof(MouseBehaviour), new FrameworkPropertyMetadata(new PropertyChangedCallback(MouseLeftDownCommandChanged)));
        public static readonly DependencyProperty MouseDragCommandProperty = DependencyProperty.RegisterAttached("MouseDragCommand", typeof(ICommand), typeof(MouseBehaviour), new FrameworkPropertyMetadata(new PropertyChangedCallback(MouseDragCommandChanged)));
        public static readonly DependencyProperty MouseLeaveCommandProperty = DependencyProperty.RegisterAttached("MouseLeaveCommand", typeof(ICommand), typeof(MouseBehaviour), new FrameworkPropertyMetadata(new PropertyChangedCallback(MouseLeaveCommandChanged)));
        public static readonly DependencyProperty MouseRightUpCommandProperty = DependencyProperty.RegisterAttached("MouseRightUpCommand", typeof(ICommand), typeof(MouseBehaviour), new FrameworkPropertyMetadata(new PropertyChangedCallback(MouseRightUpCommandChanged)));
        public static readonly DependencyProperty MouseRightDownCommandProperty = DependencyProperty.RegisterAttached("MouseRightDownCommand", typeof(ICommand), typeof(MouseBehaviour), new FrameworkPropertyMetadata(new PropertyChangedCallback(MouseRightDownCommandChanged)));

        private static void MouseLeftUpCommandChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;

            element.MouseLeftButtonUp += new MouseButtonEventHandler(MouseLeftUpCommand);
        }

        private static void MouseLeftUpCommand(object sender, MouseEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;
            ICommand command = GetMouseLeftUpCommand(element);
            command.Execute(e);
        }

        public static void SetMouseLeftUpCommand(UIElement element, ICommand command)
        {
            element.SetValue(MouseLeftUpCommandProperty, command);
        }

        public static ICommand GetMouseLeftUpCommand(UIElement element)
        {
            return (ICommand)element.GetValue(MouseLeftUpCommandProperty);
        }

        private static void MouseLeftDownCommandChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;

            element.MouseLeftButtonDown += new MouseButtonEventHandler(MouseLeftDownCommand);
        }

        private static void MouseLeftDownCommand(object sender, MouseEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;
            ICommand command = GetMouseLeftDownCommand(element);
            command.Execute(e);
        }

        public static void SetMouseLeftDownCommand(UIElement element, ICommand command)
        {
            element.SetValue(MouseLeftDownCommandProperty, command);
        }

        public static ICommand GetMouseLeftDownCommand(UIElement element)
        {
            return (ICommand)element.GetValue(MouseLeftDownCommandProperty);
        }

        private static void MouseDragCommandChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;

            element.MouseMove += new MouseEventHandler(MouseDragCommand);
        }

        private static void MouseDragCommand(object sender, MouseEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;
            ICommand command = GetMouseDragCommand(element);
            command.Execute(e);
        }

        public static void SetMouseDragCommand(UIElement element, ICommand command)
        {
            element.SetValue(MouseDragCommandProperty, command);
        }

        public static ICommand GetMouseDragCommand(UIElement element)
        {
            return (ICommand)element.GetValue(MouseDragCommandProperty);
        }

        private static void MouseLeaveCommandChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;

            element.MouseLeave += new MouseEventHandler(MouseLeaveCommand);
        }

        private static void MouseLeaveCommand(object sender, MouseEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;
            ICommand command = GetMouseLeaveCommand(element);
            command.Execute(e);
        }

        public static void SetMouseLeaveCommand(UIElement element, ICommand command)
        {
            element.SetValue(MouseLeaveCommandProperty, command);
        }

        public static ICommand GetMouseLeaveCommand(UIElement element)
        {
            return (ICommand)element.GetValue(MouseLeaveCommandProperty);
        }

        private static void MouseRightUpCommandChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;

            element.MouseRightButtonUp += new MouseButtonEventHandler(MouseRightUpCommand);
        }

        private static void MouseRightUpCommand(object sender, MouseEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;
            ICommand command = GetMouseRightUpCommand(element);
            command.Execute(e);
        }

        public static void SetMouseRightUpCommand(UIElement element, ICommand command)
        {
            element.SetValue(MouseRightUpCommandProperty, command);
        }

        public static ICommand GetMouseRightUpCommand(UIElement element)
        {
            return (ICommand)element.GetValue(MouseRightUpCommandProperty);
        }

        private static void MouseRightDownCommandChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;

            element.MouseRightButtonDown += new MouseButtonEventHandler(MouseRightDownCommand);
        }

        private static void MouseRightDownCommand(object sender, MouseEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;
            ICommand command = GetMouseRightDownCommand(element);
            command.Execute(e);
        }

        public static void SetMouseRightDownCommand(UIElement element, ICommand command)
        {
            element.SetValue(MouseRightUpCommandProperty, command);
        }

        public static ICommand GetMouseRightDownCommand(UIElement element)
        {
            return (ICommand)element.GetValue(MouseRightDownCommandProperty);
        }
    }
}

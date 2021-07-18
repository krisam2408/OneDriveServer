using System.Windows;
using System.Windows.Input;

namespace CharTracker.Core
{
    public class MouseBehaviour
    {
        public static readonly DependencyProperty MouseUpCommandProperty = DependencyProperty.RegisterAttached("MouseUpCommand", typeof(ICommand), typeof(MouseBehaviour), new FrameworkPropertyMetadata(new PropertyChangedCallback(MouseUpCommandChanged)));
        public static readonly DependencyProperty MouseDownCommandProperty = DependencyProperty.RegisterAttached("MouseDownCommand", typeof(ICommand), typeof(MouseBehaviour), new FrameworkPropertyMetadata(new PropertyChangedCallback(MouseDownCommandChanged)));
        public static readonly DependencyProperty MouseDragCommandProperty = DependencyProperty.RegisterAttached("MouseDragCommand", typeof(ICommand), typeof(MouseBehaviour), new FrameworkPropertyMetadata(new PropertyChangedCallback(MouseDragCommandChanged)));
        public static readonly DependencyProperty MouseLeaveCommandProperty = DependencyProperty.RegisterAttached("MouseLeaveCommand", typeof(ICommand), typeof(MouseBehaviour), new FrameworkPropertyMetadata(new PropertyChangedCallback(MouseLeaveCommandChanged)));

        private static void MouseUpCommandChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;

            element.MouseUp += new MouseButtonEventHandler(MouseUpCommand);
        }

        private static void MouseUpCommand(object sender, MouseEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;
            ICommand command = GetMouseUpCommand(element);
            command.Execute(e);
        }

        public static void SetMouseUpCommand(UIElement element, ICommand command)
        {
            element.SetValue(MouseUpCommandProperty, command);
        }

        public static ICommand GetMouseUpCommand(UIElement element)
        {
            return (ICommand)element.GetValue(MouseUpCommandProperty);
        }

        private static void MouseDownCommandChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;

            element.MouseDown += new MouseButtonEventHandler(MouseDownCommand);
        }

        private static void MouseDownCommand(object sender, MouseEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;
            ICommand command = GetMouseDownCommand(element);
            command.Execute(e);
        }

        public static void SetMouseDownCommand(UIElement element, ICommand command)
        {
            element.SetValue(MouseDownCommandProperty, command);
        }

        public static ICommand GetMouseDownCommand(UIElement element)
        {
            return (ICommand)element.GetValue(MouseDownCommandProperty);
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
    }
}

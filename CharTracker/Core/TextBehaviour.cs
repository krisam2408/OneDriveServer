using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CharTracker.Core
{
    public class TextBehaviour
    {
        public static readonly DependencyProperty TextChangedCommandProperty = DependencyProperty.RegisterAttached("TextChangedCommand", typeof(ICommand), typeof(TextBehaviour), new FrameworkPropertyMetadata(new PropertyChangedCallback(TextChangedCommandChanged)));

        private static void TextChangedCommandChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TextBox element = (TextBox)sender;

            element.TextChanged += new TextChangedEventHandler(TextChangedCommand);
        }

        private static void TextChangedCommand(object sender, TextChangedEventArgs e)
        {
            TextBox element = (TextBox)sender;
            ICommand command = GetTextChangedCommand(element);
            command.Execute(e);
        }

        public static void SetTextChangedCommand(TextBox element, ICommand command)
        {
            element.SetValue(TextChangedCommandProperty, command);
        }

        public static ICommand GetTextChangedCommand(TextBox element)
        {
            return (ICommand)element.GetValue(TextChangedCommandProperty);
        }
    }
}

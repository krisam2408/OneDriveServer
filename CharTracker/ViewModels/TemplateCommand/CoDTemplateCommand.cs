using RetiraTracker.Core;
using RetiraTracker.Core.Abstracts;
using RetiraTracker.Model;
using SheetDrama.DataTransfer;
using SheetDrama.Templates.ChroniclesOfDarkness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RetiraTracker.ViewModels.TemplateCommand
{
    public class CoDTemplateCommand : ITemplateCommand
    {
        private BaseViewModel Parent { get; init; }

        public ICommand AddIntValueCommand { get { return new RelayCommand(e => AddIntValueToList((MouseEventArgs)e)); } }
        public ICommand RemoveIntValueCommand { get { return new RelayCommand((e) => RemoveIntValueFromList((MouseEventArgs)e)); } }
        public ICommand AddStringValueCommand { get { return new RelayCommand(e => AddStringValueToList((MouseEventArgs)e)); } }
        public ICommand RemoveStringValueCommand { get { return new RelayCommand((e) => RemoveStringValueFromList((MouseEventArgs)e)); } }

        public CoDTemplateCommand(BaseViewModel parent)
        {
            Parent = parent;
        }

        private void AddIntValueToList(MouseEventArgs e)
        {
            ListView listControl = null;
            FrameworkElement source = (FrameworkElement)e.OriginalSource;

            do
            {
                PropertyInfo propInfo = source.GetType()
                    .GetProperty("CustomParameter");

                if (propInfo != null)
                    listControl = (ListView)propInfo.GetValue(source);

                source = (FrameworkElement)source.Parent;

            } while (listControl == null && source != null);

            List<KeyIntValue> list = new();

            if (listControl.ItemsSource != null)
                list = (List<KeyIntValue>)listControl.ItemsSource;

            list.Add(new KeyIntValue());

            listControl.ItemsSource = null;
            listControl.ItemsSource = list;
        }

        private void RemoveIntValueFromList(MouseEventArgs e)
        {
            ListView listControl = null;
            FrameworkElement source = (FrameworkElement)e.OriginalSource;

            do
            {
                PropertyInfo propInfo = source.GetType()
                    .GetProperty("CustomParameter");

                if (propInfo != null)
                    listControl = (ListView)propInfo.GetValue(source);

                source = (FrameworkElement)source.Parent;

            } while (listControl == null && source != null);

            List<KeyIntValue> list = (List<KeyIntValue>)listControl.ItemsSource;

            string key = ((TextBox)((StackPanel)source).Children[1]).Text;

            KeyIntValue valueToRemove = list.FirstOrDefault(k => k.Key == key);

            if (valueToRemove == null)
                return;

            list.Remove(valueToRemove);

            listControl.ItemsSource = null;
            listControl.ItemsSource = list;

            ((CampaignViewModel)Parent).UpdateSheetCommand.Execute(null);
        }

        private void AddStringValueToList(MouseEventArgs e)
        {
            ListView listControl = null;
            FrameworkElement source = (FrameworkElement)e.OriginalSource;

            do
            {
                PropertyInfo propInfo = source.GetType()
                    .GetProperty("CustomParameter");

                if (propInfo != null)
                    listControl = (ListView)propInfo.GetValue(source);

                source = (FrameworkElement)source.Parent;

            } while (listControl == null && source != null);

            List<KeyStringValue> list = new();

            if (listControl.ItemsSource != null)
                list = (List<KeyStringValue>)listControl.ItemsSource;

            list.Add(new KeyStringValue());

            listControl.ItemsSource = null;
            listControl.ItemsSource = list;
        }

        private void RemoveStringValueFromList(MouseEventArgs e)
        {
            ListView listControl = null;
            FrameworkElement source = (FrameworkElement)e.OriginalSource;

            do
            {
                PropertyInfo propInfo = source.GetType()
                    .GetProperty("CustomParameter");

                if (propInfo != null)
                    listControl = (ListView)propInfo.GetValue(source);

                source = (FrameworkElement)source.Parent;

            } while (listControl == null && source != null);

            List<KeyStringValue> list = (List<KeyStringValue>)listControl.ItemsSource;

            string key = ((TextBox)((StackPanel)source).Children[1]).Text;

            KeyStringValue valueToRemove = list.FirstOrDefault(k => k.Key == key);

            if (valueToRemove == null)
                return;

            list.Remove(valueToRemove);

            listControl.ItemsSource = null;
            listControl.ItemsSource = list;

            ((CampaignViewModel)Parent).UpdateSheetCommand.Execute(null);
        }

    }
}

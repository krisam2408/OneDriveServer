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

        public ICommand AddMeritCommand { get { return new RelayCommand(e => AddMerit((MouseEventArgs)e)); } }
        public ICommand RemoveMeritCommand { get { return new RelayCommand((e) => RemoveMerit((MouseEventArgs)e)); } }
        public ICommand AddConditionCommand { get { return new RelayCommand(e => AddCondition((MouseEventArgs)e)); } }
        public ICommand RemoveConditionCommand { get { return new RelayCommand((e) => RemoveCondition((MouseEventArgs)e)); } }


        public CoDTemplateCommand(BaseViewModel parent)
        {
            Parent = parent;
        }

        private void AddMerit(MouseEventArgs e)
        {
            ListView meritListControl = null;
            FrameworkElement source = (FrameworkElement)e.OriginalSource;

            do
            {
                PropertyInfo propInfo = source.GetType()
                    .GetProperty("CustomParameter");

                if (propInfo != null)
                    meritListControl = (ListView)propInfo.GetValue(source);

                source = (FrameworkElement)source.Parent;

            } while (meritListControl == null && source != null);

            List<KeyIntValue> meritList = new();

            if (meritListControl.ItemsSource != null)
                meritList = (List<KeyIntValue>)meritListControl.ItemsSource;

            meritList.Add(new KeyIntValue());

            meritListControl.ItemsSource = null;
            meritListControl.ItemsSource = meritList;
        }

        private void RemoveMerit(MouseEventArgs e)
        {
            ListView meritListControl = null;
            FrameworkElement source = (FrameworkElement)e.OriginalSource;

            do
            {
                PropertyInfo propInfo = source.GetType()
                    .GetProperty("CustomParameter");

                if (propInfo != null)
                    meritListControl = (ListView)propInfo.GetValue(source);

                source = (FrameworkElement)source.Parent;

            } while (meritListControl == null && source != null);

            List<KeyIntValue> meritList = (List<KeyIntValue>)meritListControl.ItemsSource;

            string key = ((TextBox)((StackPanel)source).Children[1]).Text;

            KeyIntValue valueToRemove = meritList.FirstOrDefault(k => k.Key == key);

            if (valueToRemove == null)
                return;

            meritList.Remove(valueToRemove);

            meritListControl.ItemsSource = null;
            meritListControl.ItemsSource = meritList;

            ((CampaignViewModel)Parent).UpdateSheetCommand.Execute(null);
        }

        private void AddCondition(MouseEventArgs e)
        {
            ListView conditionListControl = null;
            FrameworkElement source = (FrameworkElement)e.OriginalSource;

            do
            {
                PropertyInfo propInfo = source.GetType()
                    .GetProperty("CustomParameter");

                if (propInfo != null)
                    conditionListControl = (ListView)propInfo.GetValue(source);

                source = (FrameworkElement)source.Parent;

            } while (conditionListControl == null && source != null);

            List<string> conditionList = new();

            if (conditionListControl.ItemsSource != null)
                conditionList = (List<string>)conditionListControl.ItemsSource;

            conditionList.Add(string.Empty);

            conditionListControl.ItemsSource = null;
            conditionListControl.ItemsSource = conditionList;
        }

        private void RemoveCondition(MouseEventArgs e)
        {

        }
    }
}

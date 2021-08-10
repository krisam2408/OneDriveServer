using RetiraTracker.Core;
using RetiraTracker.Core.Abstracts;
using RetiraTracker.Model;
using SheetDrama.DataTransfer;
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
                meritList = ((KeyIntValue[])meritListControl.ItemsSource).ToList();

            meritList.Add(new KeyIntValue() { Key = "Recursos", Value = 3 });

            meritListControl.ItemsSource = meritList.ToArray();

            Parent.NotifyPropertyChanged("SheetData");
        }
    }
}

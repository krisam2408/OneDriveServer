using Newtonsoft.Json;
using RetiraTracker.Core;
using RetiraTracker.Extensions;
using RetiraTracker.Model;
using RetiraTracker.Model.DataTransfer;
using RetiraTracker.Model.Domain;
using SheetDrama;
using SheetDrama.Abstracts;
using SheetDrama.Extensions;
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
using System.Windows.Shapes;
using ListItem = RetiraTracker.Core.ListItem;

namespace RetiraTracker.View.Popups
{
    /// <summary>
    /// Interaction logic for ChangeSheetTemplatePopup.xaml
    /// </summary>
    public partial class ChangeSheetTemplatePopup : Window
    {
        public ChangeSheetTemplatePopup(GameTemplates[] templates)
        {
            InitializeComponent();
            DataContext = this;

            TemplateComboBox.SelectionChanged += TemplateChangeCommand;

            ListItem noSelection = new("0", "- No Selection -");
            TemplateComboBox.Items.Add(noSelection);

            foreach(GameTemplates gt in templates)
            {
                ListItem li = new(gt.ToString(), gt.WriteEnum());
                li.SetContent(gt);
                TemplateComboBox.Items.Add(li);
            }

            TemplateComboBox.SelectedIndex = 0;
            
        }

        private void TemplateChangeCommand(object sender, SelectionChangedEventArgs e)
        {
            if (TemplateComboBox.SelectedIndex == 0)
            {
                ChangeButton.IsEnabled = false;
                return;
            }

            ChangeButton.IsEnabled = true;
        }

        public ICommand CloseCommand
        {
            get
            {
                return new RelayCommand(e =>
                {
                    Terminal.Instance.Navigation.IsLoading(false);
                    Close();
                });
            }
        }

        public ICommand ChangeCommand
        {
            get
            {
                return new RelayCommand(e =>
                { 
                    ConfirmationPanel.Visibility = Visibility.Visible;
                    ChangeButton.IsEnabled = false;
                    TemplateComboBox.IsEnabled = false;
                    Height = 238;
                });
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                return new RelayCommand(e =>
                { 
                    ConfirmationPanel.Visibility= Visibility.Collapsed;
                    TemplateComboBox.IsEnabled = true;
                    Height = 168;
                    TemplateComboBox.SelectedIndex = 0;
                });
            }
        }

        public ICommand YesCommand
        {
            get
            {
                return new RelayCommand(e =>
                { 
                    AppSheet selectedSheet = Terminal.Instance.Campaign.SelectedSheet.GetContent<AppSheet>();
                    selectedSheet.Initialize();

                    GameTemplates newGameTemplate = ((ListItem)TemplateComboBox.SelectedItem).GetContent<GameTemplates>();

                    ISheet newSheet = SheetFactory.ChangeSheetTemplate(newGameTemplate.GetTemplate(), selectedSheet.Sheet, newGameTemplate.GetTemplatePage(), null, newGameTemplate.TemplateGame().GetGameCommands());

                    Player newPlayer = new()
                    {
                        EmailAddress = newSheet.PlayerName,
                        SheetTemplate = newGameTemplate.GetTemplate(),
                        SheetJson = JsonConvert.SerializeObject(newSheet)
                    };

                    AppSheet newAppSheet = new(newPlayer);

                    Terminal.Instance.Campaign.SelectedSheet.SetContent(newAppSheet);
                    Terminal.Instance.Campaign.SelectedSheet = Terminal.Instance.Campaign.SelectedSheet;

                    Terminal.Instance.Navigation.IsLoading(false);
                    Close();
                });
            }
        }
    }
}

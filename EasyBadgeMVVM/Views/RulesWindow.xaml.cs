using EasyBadgeMVVM.Models;
using EasyBadgeMVVM.ViewModels.impl;
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

namespace EasyBadgeMVVM.Views
{
    /// <summary>
    /// Interaction logic for RulesWindow.xaml
    /// </summary>
    public partial class RulesWindow : Window
    {
        Dictionary<string, bool> stopProcessSelectionChanged;
        public static readonly string RULE_TARGET_FIELDLIST_NAME = "ruletargetfieldname_";
        public static readonly string RULE_COLORPICKER_NAME = "rulecolorpickername_";
        public static readonly string RULE_BADGEEVENT_FIELDLIST_NAME = "rulebadgeeventfieldlistname_";
        private int lastRowNumber = 1;

        private RuleVM ruleVM;
        private int filterId;
        private Rule newRule;    // new rule not yet saved

        public RulesWindow(int filterId)
        {
            this.filterId = filterId;
            this.ruleVM = new RuleVM(filterId);
            this.stopProcessSelectionChanged = new Dictionary<string, bool>();
            InitializeComponent();
            InitializeRuleScreen();
        }

        /// <summary>
        /// Fetch the existing rules from DB for this filter.
        /// Build the UI.
        /// </summary>
        private void InitializeRuleScreen()
        {
            Label titleBarLabel = FindName("TitleBarLabel") as Label;
            titleBarLabel.Content = ruleVM.Field.Name + " " + ruleVM.Filter.LogicalOperator + " " + ruleVM.Filter.Value;
            foreach(Rule item in ruleVM.Rules)
            {
                AddNewRuleRow(item);
            }
        }

        /// <summary>
        /// Create and display an empty rule row in UI
        /// </summary>
        private void AddNewRuleRow()
        {
            lastRowNumber += 1;
            AddRuleButton.IsEnabled = false;
            newRule = new Rule
            {
                FilterID_Filter = filterId
            };

            RowDefinition rowDefinition = new RowDefinition();
            rowDefinition.Height = new GridLength(80);
            this.RulesGrid.RowDefinitions.Add(rowDefinition);

            ComboBox targetsList = new ComboBox();
            targetsList.Name = RULE_TARGET_FIELDLIST_NAME + lastRowNumber;
            targetsList.ItemsSource = ruleVM.Targets;
            targetsList.DisplayMemberPath = "Name";
            targetsList.SetValue(Grid.ColumnProperty, 1);
            targetsList.SetValue(Grid.RowProperty, lastRowNumber);
            targetsList.VerticalAlignment = VerticalAlignment.Top;
            targetsList.FontSize = 16;
            targetsList.HorizontalContentAlignment = HorizontalAlignment.Center;
            targetsList.SelectionChanged += new SelectionChangedEventHandler(
                (sender, e) => {
                    bool b;
                    if (stopProcessSelectionChanged.TryGetValue(targetsList.Name, out b) == false)
                    {
                        int targetId = (targetsList.SelectedValue as Target).ID_Target;
                        newRule.TargetID_Target = targetId;
                        SwitchAssociatedField(targetId);
                    }
                }
            );
            RegisterName(RULE_TARGET_FIELDLIST_NAME + lastRowNumber, targetsList);
            this.RulesGrid.Children.Add(targetsList);
        }

        private void SwitchAssociatedField(int targetId)
        {
            switch(ruleVM.Targets.First(t => t.ID_Target == targetId).Name)
            {
                case "Badge":
                    // DropDown
                    ComboBox templatesList = new ComboBox();
                    templatesList.Name = RULE_BADGEEVENT_FIELDLIST_NAME + lastRowNumber;
                    templatesList.ItemsSource = ruleVM.BadgeEvents;
                    templatesList.DisplayMemberPath = "Name";
                    templatesList.SetValue(Grid.ColumnProperty, 2);
                    templatesList.SetValue(Grid.RowProperty, lastRowNumber);
                    templatesList.VerticalAlignment = VerticalAlignment.Top;
                    templatesList.FontSize = 16;
                    templatesList.HorizontalContentAlignment = HorizontalAlignment.Center;
                    templatesList.SelectionChanged += new SelectionChangedEventHandler(
                        (sender, e) => {
                            bool b;
                            if (stopProcessSelectionChanged.TryGetValue(templatesList.Name, out b) == false)
                            {
                                newRule.TargetID_Target = (templatesList.SelectedValue as Target).ID_Target;
                            }
                        }
                    );
                    RegisterName(RULE_TARGET_FIELDLIST_NAME + lastRowNumber, templatesList);
                    this.RulesGrid.Children.Add(templatesList);
                    break;
                case "List":
                case "Window":
                    // ColorPicker
                    break;
            }
        }

        /// <summary>
        /// Create and display the given rule row in UI
        /// </summary>
        private void AddNewRuleRow(Rule rule)
        {
            throw new NotImplementedException();
        }


        /************ LISTENERS ************/

        private void OnClick_AddRule(object sender, RoutedEventArgs e)
        {
            AddNewRuleRow();
        }

        private void OnClick_Save(object sender, RoutedEventArgs e)
        {
        }
    }
}

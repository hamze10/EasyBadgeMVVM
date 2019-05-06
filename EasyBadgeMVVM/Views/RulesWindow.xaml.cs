using EasyBadgeMVVM.Models;
using EasyBadgeMVVM.ViewModels.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace EasyBadgeMVVM.Views
{
    /// <summary>
    /// Interaction logic for RulesWindow.xaml
    /// </summary>
    public partial class RulesWindow : Window
    {
        Dictionary<string, bool> stopProcessSelectionChanged;
        public static readonly string RULE_TARGET_FIELDLIST_NAME = "ruletargetfieldlistname_";
        public static readonly string RULE_COLORPICKER_NAME = "rulecolorpickername_";
        public static readonly string RULE_BADGEEVENT_FIELDLIST_NAME = "rulebadgeeventfieldlistname_";
        public static readonly string RULE_DELETE_RULE_NAME = "ruledelete_";
        public static readonly string RULE_ID = "rule_id_";
        private int lastRowNumber = 1;
        HashSet<int> deletedRules;  // name ids

        private RuleVM ruleVM;
        private int filterId;
        private RuleSet newRule;    // new rule not yet saved

        public RulesWindow(int filterId)
        {
            this.filterId = filterId;
            this.ruleVM = new RuleVM(filterId);
            this.stopProcessSelectionChanged = new Dictionary<string, bool>();
            this.deletedRules = new HashSet<int>();
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
            foreach(RuleSet item in ruleVM.Rules)
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
            newRule = new RuleSet
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
                    int targetId = (targetsList.SelectedValue as TargetSet).ID_Target;
                    ComboBox clicked = e.Source as ComboBox;
                    int position = Convert.ToInt32(clicked.Name.Substring(RULE_TARGET_FIELDLIST_NAME.Length));
                    if (stopProcessSelectionChanged.TryGetValue(targetsList.Name, out b) == false)
                    {                        
                        newRule.TargetID_Target = targetId;
                    }
                    SwitchAssociatedField(targetId, position);
                }
            );
            RegisterName(RULE_TARGET_FIELDLIST_NAME + lastRowNumber, targetsList);
            this.RulesGrid.Children.Add(targetsList);

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
                        newRule.BadgeEventID_BadgeEvent = (templatesList.SelectedValue as BadgeEventSet).ID_BadgeEvent;
                    }
                }
            );
            //templatesList.Visibility = Visibility.Hidden;
            templatesList.IsEnabled = false;
            RegisterName(RULE_BADGEEVENT_FIELDLIST_NAME + lastRowNumber, templatesList);
            this.RulesGrid.Children.Add(templatesList);

            ColorPicker colorPicker = new ColorPicker();
            colorPicker.Name = RULE_COLORPICKER_NAME + lastRowNumber;
            colorPicker.SetValue(Grid.ColumnProperty, 3);
            colorPicker.SetValue(Grid.RowProperty, lastRowNumber);
            colorPicker.VerticalAlignment = VerticalAlignment.Top;
            colorPicker.HorizontalContentAlignment = HorizontalAlignment.Center;
            colorPicker.FontSize = 16;
            colorPicker.SelectedColorChanged += ColorPicker_SelectedColorChanged;
            //colorPicker.Visibility = Visibility.Hidden;
            colorPicker.IsEnabled = false;
            RegisterName(RULE_COLORPICKER_NAME + lastRowNumber, colorPicker);
            this.RulesGrid.Children.Add(colorPicker);

            Button deleteRule = new Button();
            deleteRule.Content = "X";
            deleteRule.IsEnabled = false;
            deleteRule.Name = RULE_DELETE_RULE_NAME + lastRowNumber;
            deleteRule.SetValue(Grid.ColumnProperty, 4);
            deleteRule.Width = 50;
            deleteRule.SetValue(Grid.RowProperty, lastRowNumber);
            deleteRule.VerticalAlignment = VerticalAlignment.Top;
            deleteRule.Click += new RoutedEventHandler(OnClick_DeleteRule);
            RegisterName(RULE_DELETE_RULE_NAME + lastRowNumber, deleteRule);
            this.RulesGrid.Children.Add(deleteRule);
        }

        private void SwitchAssociatedField(int targetId, int position)
        {
            ComboBox templates;
            ColorPicker colorPicker;
            switch (ruleVM.Targets.First(t => t.ID_Target == targetId).Name)
            {
                case "Badge":
                    // DropDown
                    templates = FindName(RULE_BADGEEVENT_FIELDLIST_NAME + position) as ComboBox;
                    //templates.Visibility = Visibility.Visible;
                    templates.IsEnabled = true;
                    colorPicker = FindName(RULE_COLORPICKER_NAME + position) as ColorPicker;
                    colorPicker.IsEnabled = false;
                    colorPicker.SelectedColor = null;
                    if (newRule != null) newRule.HexaCode = null;
                    break;
                case "List":
                case "Window":
                    // ColorPicker
                    templates = FindName(RULE_BADGEEVENT_FIELDLIST_NAME + position) as ComboBox;
                    //templates.Visibility = Visibility.Hidden;
                    templates.IsEnabled = false;
                    templates.SelectedValue = null;
                    if (newRule != null) newRule.BadgeEventID_BadgeEvent = 0;
                    colorPicker = FindName(RULE_COLORPICKER_NAME + position) as ColorPicker;
                    //colorPicker.Visibility = Visibility.Visible;
                    colorPicker.IsEnabled = true;
                    break;
            }
        }

        /// <summary>
        /// Create and display the given rule row in UI
        /// </summary>
        private void AddNewRuleRow(RuleSet rule)
        {
            lastRowNumber += 1;

            RowDefinition rowDefinition = new RowDefinition();
            rowDefinition.Height = new GridLength(80);
            this.RulesGrid.RowDefinitions.Add(rowDefinition);

            TextBox ruleId = new TextBox();
            ruleId.Name = RULE_ID + lastRowNumber;
            ruleId.Text = rule.ID_Rule.ToString();
            ruleId.SetValue(Grid.ColumnProperty, 0);
            ruleId.SetValue(Grid.RowProperty, lastRowNumber);
            ruleId.Visibility = Visibility.Hidden;
            RegisterName(RULE_ID + lastRowNumber, ruleId);
            this.RulesGrid.Children.Add(ruleId);

            ComboBox targetsList = new ComboBox();
            targetsList.Name = RULE_TARGET_FIELDLIST_NAME + lastRowNumber;
            targetsList.ItemsSource = ruleVM.Targets;
            targetsList.DisplayMemberPath = "Name";
            targetsList.SetValue(Grid.ColumnProperty, 1);
            targetsList.SetValue(Grid.RowProperty, lastRowNumber);
            targetsList.VerticalAlignment = VerticalAlignment.Top;
            targetsList.FontSize = 16;
            targetsList.SelectedValue = ruleVM.Targets.LastOrDefault(t => t.ID_Target == rule.TargetID_Target);
            targetsList.SelectionChanged += new SelectionChangedEventHandler(
                (sender, e) => {
                    int targetId = (targetsList.SelectedValue as TargetSet).ID_Target;
                    ComboBox clicked = e.Source as ComboBox;
                    int position = Convert.ToInt32(clicked.Name.Substring(RULE_TARGET_FIELDLIST_NAME.Length));
                    SwitchAssociatedField(targetId, position);
                }
            );
            targetsList.HorizontalContentAlignment = HorizontalAlignment.Center;
            RegisterName(RULE_TARGET_FIELDLIST_NAME + lastRowNumber, targetsList);
            this.RulesGrid.Children.Add(targetsList);

            ComboBox templatesList = new ComboBox();
            templatesList.Name = RULE_BADGEEVENT_FIELDLIST_NAME + lastRowNumber;
            templatesList.ItemsSource = ruleVM.BadgeEvents;
            templatesList.DisplayMemberPath = "Name";
            templatesList.SetValue(Grid.ColumnProperty, 2);
            templatesList.SetValue(Grid.RowProperty, lastRowNumber);
            templatesList.VerticalAlignment = VerticalAlignment.Top;
            templatesList.FontSize = 16;
            templatesList.HorizontalContentAlignment = HorizontalAlignment.Center;
            //if (rule.BadgeEventID_BadgeEvent == 0) templatesList.Visibility = Visibility.Hidden;
            if (rule.BadgeEventID_BadgeEvent == 0)
                templatesList.IsEnabled = false;
            else
                targetsList.SelectedValue = ruleVM.BadgeEvents.LastOrDefault(be => be.ID_BadgeEvent== rule.BadgeEventID_BadgeEvent);
            RegisterName(RULE_BADGEEVENT_FIELDLIST_NAME + lastRowNumber, templatesList);
            this.RulesGrid.Children.Add(templatesList);

            ColorPicker colorPicker = new ColorPicker();
            colorPicker.Name = RULE_COLORPICKER_NAME + lastRowNumber;
            colorPicker.SetValue(Grid.ColumnProperty, 3);
            colorPicker.SetValue(Grid.RowProperty, lastRowNumber);
            colorPicker.VerticalAlignment = VerticalAlignment.Top;
            colorPicker.HorizontalContentAlignment = HorizontalAlignment.Center;
            colorPicker.FontSize = 16;
            //if (rule.HexaCode == null) colorPicker.Visibility = Visibility.Hidden;
            if (rule.HexaCode == null)
                colorPicker.IsEnabled = false;
            else
                colorPicker.SelectedColor = (Color)ColorConverter.ConvertFromString(rule.HexaCode);
            RegisterName(RULE_COLORPICKER_NAME + lastRowNumber, colorPicker);
            this.RulesGrid.Children.Add(colorPicker);

            Button deleteRule = new Button();
            deleteRule.Content = "X";
            deleteRule.Name = RULE_DELETE_RULE_NAME + lastRowNumber;
            deleteRule.SetValue(Grid.ColumnProperty, 4);
            deleteRule.Width = 50;
            deleteRule.SetValue(Grid.RowProperty, lastRowNumber);
            deleteRule.VerticalAlignment = VerticalAlignment.Top;
            deleteRule.Click += new RoutedEventHandler(OnClick_DeleteRule);
            RegisterName(RULE_DELETE_RULE_NAME + lastRowNumber, deleteRule);
            this.RulesGrid.Children.Add(deleteRule);
        }


        /************ LISTENERS ************/

        private void OnClick_DeleteRule(object sender, RoutedEventArgs e)
        {
            Button clicked = e.Source as Button;
            int rowNumber = Convert.ToInt32(clicked.Name.Substring(RULE_DELETE_RULE_NAME.Length));

            TextBox ruleId = FindName(RULE_ID + rowNumber) as TextBox;
            if (ruleId == null) return; // not yet persisted

            ComboBox targets = FindName(RULE_TARGET_FIELDLIST_NAME + rowNumber) as ComboBox;
            ComboBox templates = FindName(RULE_BADGEEVENT_FIELDLIST_NAME + rowNumber) as ComboBox;
            ColorPicker picker = FindName(RULE_COLORPICKER_NAME + rowNumber) as ColorPicker;
            Button deleteButton = FindName(RULE_DELETE_RULE_NAME + rowNumber) as Button;
            this.RulesGrid.Children.Remove(targets);
            this.RulesGrid.Children.Remove(templates);
            this.RulesGrid.Children.Remove(picker);
            this.RulesGrid.Children.Remove(deleteButton);
            //this.RulesGrid.RowDefinitions.Remove(RulesGrid.RowDefinitions[rowNumber]);
            ruleVM.DeleteRule(Convert.ToInt32(ruleId.Text));
            deletedRules.Add(rowNumber);
        }

        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
        {
            bool b;
            if (stopProcessSelectionChanged.TryGetValue((e.Source as ColorPicker).Name, out b) == false)
            {
                newRule.HexaCode = (e.Source as ColorPicker).SelectedColor.Value.ToString();
            }
        }

        private void OnClick_AddRule(object sender, RoutedEventArgs e)
        {
            AddNewRuleRow();
        }

        private void OnClick_Save(object sender, RoutedEventArgs e)
        {
            // There is a new filter to save
            if (newRule != null)
            {
                if (newRule.HexaCode == null && newRule.BadgeEventID_BadgeEvent == 0) return;
                if (newRule.TargetID_Target == 0) return;
                RuleSet newR = ruleVM.SaveNewRule(newRule);

                // remove events for the added filter
                stopProcessSelectionChanged.Add(RULE_TARGET_FIELDLIST_NAME + lastRowNumber, true);
                stopProcessSelectionChanged.Add(RULE_BADGEEVENT_FIELDLIST_NAME + lastRowNumber, true);
                stopProcessSelectionChanged.Add(RULE_COLORPICKER_NAME + lastRowNumber, true);

                newRule = null;
                AddRuleButton.IsEnabled = true;

                Button deleteButton = FindName(RULE_DELETE_RULE_NAME + lastRowNumber) as Button;
                deleteButton.IsEnabled = true;

                TextBox ruleId = new TextBox();
                ruleId.Name = RULE_ID + lastRowNumber;
                ruleId.Text = newR.ID_Rule.ToString();
                ruleId.SetValue(Grid.ColumnProperty, 0);
                ruleId.SetValue(Grid.RowProperty, lastRowNumber);
                ruleId.Visibility = Visibility.Hidden;
                RegisterName(RULE_ID + lastRowNumber, ruleId);
                this.RulesGrid.Children.Add(ruleId);
            }

            // Update the existing filters
            int id_rule_name = 2;
            foreach (RuleSet item in ruleVM.Rules)
            {
                if (deletedRules.Contains(id_rule_name))
                {
                    id_rule_name += 1;
                    continue;
                };

                ComboBox targets = FindName(RULE_TARGET_FIELDLIST_NAME + id_rule_name) as ComboBox;
                int current_target_id = (targets.SelectedValue as TargetSet).ID_Target;
                ComboBox templates = FindName(RULE_BADGEEVENT_FIELDLIST_NAME + id_rule_name) as ComboBox;
                int current_template_id = 0;
                string current_hexa_code = null;
                if ((templates.SelectedValue as BadgeEventSet) != null)
                    current_template_id = (templates.SelectedValue as BadgeEventSet).ID_BadgeEvent;
                ColorPicker picker = FindName(RULE_COLORPICKER_NAME + id_rule_name) as ColorPicker;
                if ((picker.SelectedColor) != null)
                    current_hexa_code = picker.SelectedColor.Value.ToString();

                item.TargetID_Target = current_target_id;
                item.BadgeEventID_BadgeEvent = current_template_id;
                item.HexaCode = current_hexa_code;
                id_rule_name += 1;
            }
            ruleVM.UpdateAllRules();
        }

    }
}

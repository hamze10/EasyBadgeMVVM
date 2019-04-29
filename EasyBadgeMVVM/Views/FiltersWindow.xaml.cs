﻿using EasyBadgeMVVM.Models;
using EasyBadgeMVVM.ViewModels.impl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EasyBadgeMVVM.Views
{
    /// <summary>
    /// Interaction logic for FiltersWindow.xaml
    /// </summary>
    public partial class FiltersWindow : Window
    {
        private SolidColorBrush[] brushes = new SolidColorBrush[2] { System.Windows.Media.Brushes.White, System.Windows.Media.Brushes.WhiteSmoke };
        Dictionary<string, bool> stopProcessSelectionChanged;
        Dictionary<int, int> filterIds;
        private static readonly IEnumerable<string> LOGICAL_OPERATORS = new string[] {
            "<", "<=", "=", ">=", ">", "starts with", "contains", "<>"
        };
        public static readonly string FILTER_FIELDSLIST_NAME = "filterfieldslist";
        public static readonly string FILTER_LOGICALOPERATORLIST_NAME = "filterlogicaloperatorlist";
        public static readonly string FILTER_VALUE_NAME = "filtervalue";
        public static readonly string RULES_BUTTON_NAME = "rulesbuttonname_";

        private FilterVM filterVM;
        private int eventId;
        private int lastRowNumber = 1;
        private Filter newFilter;    // new filter not yet saved
        private Button newFilterRulesButton;

        public FiltersWindow(int eventId)
        {
            this.eventId = eventId;
            this.filterVM = new FilterVM(eventId);
            this.newFilter = null;
            this.filterIds = new Dictionary<int, int>();
            this.stopProcessSelectionChanged = new Dictionary<string, bool>();
            InitializeComponent();
            InitializeFilterScreen();
        }

        /// <summary>
        /// Fetch the existing filters from DB for this event.
        /// Build the UI.
        /// </summary>
        private void InitializeFilterScreen()
        {
            foreach(Filter item in filterVM.Filters)
            {
                AddNewFilterRow(item);
            }
        }
                
        /// <summary>
        /// Create and display an empty filter row in UI
        /// </summary>
        private void AddNewFilterRow()
        {
            lastRowNumber += 1;
            AddFilterButton.IsEnabled = false;
            newFilter = new Filter
            {
                EventFieldEventID_Event = eventId
            };

            RowDefinition rowDefinition = new RowDefinition();
            rowDefinition.Height = new GridLength(80);
            this.FiltersGrid.RowDefinitions.Add(rowDefinition);

            ComboBox fieldsList = new ComboBox();
            fieldsList.Name = FILTER_FIELDSLIST_NAME + lastRowNumber;
            fieldsList.ItemsSource = filterVM.Fields;
            fieldsList.DisplayMemberPath = "Name";
            fieldsList.SetValue(Grid.ColumnProperty, 1);
            fieldsList.SetValue(Grid.RowProperty, lastRowNumber);
            fieldsList.VerticalAlignment = VerticalAlignment.Top;
            fieldsList.FontSize = 16;
            fieldsList.HorizontalContentAlignment = HorizontalAlignment.Center;
            fieldsList.SelectionChanged += new SelectionChangedEventHandler(
                (sender, e) => {
                    bool b;
                    if (stopProcessSelectionChanged.TryGetValue(fieldsList.Name, out b) == false)
                    {
                        newFilter.EventFieldFieldID_Field = (fieldsList.SelectedValue as Field).ID_Field;
                    }
                }
            );
            RegisterName(FILTER_FIELDSLIST_NAME + lastRowNumber, fieldsList);
            this.FiltersGrid.Children.Add(fieldsList);

            ComboBox logicalOperatorList = new ComboBox();
            logicalOperatorList.Name = FILTER_LOGICALOPERATORLIST_NAME + lastRowNumber;
            logicalOperatorList.ItemsSource = LOGICAL_OPERATORS;
            logicalOperatorList.SetValue(Grid.ColumnProperty, 2);
            logicalOperatorList.SetValue(Grid.RowProperty, lastRowNumber);
            logicalOperatorList.VerticalAlignment = VerticalAlignment.Top;
            logicalOperatorList.FontSize = 16;
            logicalOperatorList.HorizontalContentAlignment = HorizontalAlignment.Center;
            logicalOperatorList.SelectionChanged += new SelectionChangedEventHandler(
                (sender, e) => {
                    bool b;
                    if(stopProcessSelectionChanged.TryGetValue(logicalOperatorList.Name, out b) == false)
                    {
                        newFilter.LogicalOperator = (sender as ComboBox).SelectedItem as string;
                    }
                }
            );
            RegisterName(FILTER_LOGICALOPERATORLIST_NAME + lastRowNumber, logicalOperatorList);
            this.FiltersGrid.Children.Add(logicalOperatorList);

            TextBox valueTextBox = new TextBox();
            valueTextBox.Name = FILTER_VALUE_NAME + lastRowNumber;
            valueTextBox.SetValue(Grid.ColumnProperty, 3);
            valueTextBox.SetValue(Grid.RowProperty, lastRowNumber);
            valueTextBox.VerticalAlignment = VerticalAlignment.Top;
            valueTextBox.FontSize = 16;
            valueTextBox.HorizontalContentAlignment = HorizontalAlignment.Center;
            valueTextBox.TextChanged += new TextChangedEventHandler(
                (sender, e) => {
                    bool b;
                    if (stopProcessSelectionChanged.TryGetValue(valueTextBox.Name, out b) == false)
                    {
                        newFilter.Value = valueTextBox.Text;
                    }
                }
            );
            RegisterName(FILTER_VALUE_NAME + lastRowNumber, valueTextBox);
            this.FiltersGrid.Children.Add(valueTextBox);

            Button configureRulesButton = new Button();
            configureRulesButton.IsEnabled = false;
            configureRulesButton.Content = "Rules";
            configureRulesButton.Name = RULES_BUTTON_NAME + lastRowNumber;
            configureRulesButton.SetValue(Grid.ColumnProperty, 4);
            configureRulesButton.Width = 120;
            configureRulesButton.SetValue(Grid.RowProperty, lastRowNumber);
            configureRulesButton.VerticalAlignment = VerticalAlignment.Top;
            configureRulesButton.Click += new RoutedEventHandler(OnClick_Rules);
            RegisterName(RULES_BUTTON_NAME + lastRowNumber, configureRulesButton);
            this.FiltersGrid.Children.Add(configureRulesButton);

            newFilterRulesButton = configureRulesButton;
        }

        /// <summary>
        /// Create and display the given filter row in UI
        /// </summary>
        private void AddNewFilterRow(Filter filter)
        {
            lastRowNumber += 1;

            RowDefinition rowDefinition = new RowDefinition();
            rowDefinition.Height = new GridLength(80);
            this.FiltersGrid.RowDefinitions.Add(rowDefinition);

            ComboBox fieldsList = new ComboBox();
            fieldsList.Name = FILTER_FIELDSLIST_NAME + lastRowNumber;
            fieldsList.ItemsSource = filterVM.Fields;
            fieldsList.DisplayMemberPath = "Name";
            fieldsList.SetValue(Grid.ColumnProperty, 1);
            fieldsList.SetValue(Grid.RowProperty, lastRowNumber);
            fieldsList.VerticalAlignment = VerticalAlignment.Top;
            fieldsList.FontSize = 16;
            fieldsList.SelectedValue = filterVM.Fields.LastOrDefault(f => f.ID_Field == filter.EventFieldFieldID_Field);
            fieldsList.HorizontalContentAlignment = HorizontalAlignment.Center;
            RegisterName(FILTER_FIELDSLIST_NAME + lastRowNumber, fieldsList);
            this.FiltersGrid.Children.Add(fieldsList);

            ComboBox logicalOperatorList = new ComboBox();
            logicalOperatorList.Name = FILTER_LOGICALOPERATORLIST_NAME + lastRowNumber;
            logicalOperatorList.ItemsSource = LOGICAL_OPERATORS;
            logicalOperatorList.SetValue(Grid.ColumnProperty, 2);
            logicalOperatorList.SetValue(Grid.RowProperty, lastRowNumber);
            logicalOperatorList.VerticalAlignment = VerticalAlignment.Top;
            logicalOperatorList.FontSize = 16;
            logicalOperatorList.SelectedValue = filter.LogicalOperator;
            logicalOperatorList.HorizontalContentAlignment = HorizontalAlignment.Center;
            RegisterName(FILTER_LOGICALOPERATORLIST_NAME + lastRowNumber, logicalOperatorList);
            this.FiltersGrid.Children.Add(logicalOperatorList);

            TextBox valueTextBox = new TextBox();
            valueTextBox.Name = FILTER_VALUE_NAME + lastRowNumber;
            valueTextBox.SetValue(Grid.ColumnProperty, 3);
            valueTextBox.SetValue(Grid.RowProperty, lastRowNumber);
            valueTextBox.VerticalAlignment = VerticalAlignment.Top;
            valueTextBox.FontSize = 16;
            valueTextBox.Text = filter.Value;
            valueTextBox.HorizontalContentAlignment = HorizontalAlignment.Center;
            RegisterName(FILTER_VALUE_NAME + lastRowNumber, valueTextBox);
            this.FiltersGrid.Children.Add(valueTextBox);

            Button configureRulesButton = new Button();
            configureRulesButton.Content = "Rules";
            configureRulesButton.Name = RULES_BUTTON_NAME + lastRowNumber;
            configureRulesButton.SetValue(Grid.ColumnProperty, 4);
            configureRulesButton.Width = 120;
            configureRulesButton.SetValue(Grid.RowProperty, lastRowNumber);
            configureRulesButton.VerticalAlignment = VerticalAlignment.Top;
            configureRulesButton.Click += new RoutedEventHandler(OnClick_Rules);
            RegisterName(RULES_BUTTON_NAME + lastRowNumber, configureRulesButton);
            this.FiltersGrid.Children.Add(configureRulesButton);

            this.filterIds.Add(lastRowNumber, filter.ID_Filter);
        }


        /************ LISTENERS ************/

        private void OnClick_AddFilter(object sender, RoutedEventArgs e)
        {
            AddNewFilterRow();
        }

        private void OnClick_Rules(object sender, RoutedEventArgs e)
        {
            Button clicked = e.Source as Button;
            int rowNumber = Convert.ToInt32(clicked.Name.Substring(RULES_BUTTON_NAME.Length));
            int filterId;
            filterIds.TryGetValue(rowNumber, out filterId);
            RulesWindow rulesWindow = new RulesWindow(filterId);
            rulesWindow.Show();
        }

        private void OnClick_Save(object sender, RoutedEventArgs e)
        {
            // There is a new filter to save
            if (newFilter != null)
            {
                if (newFilter.Value == null) return;
                if (newFilter.Value.Length == 0) return;
                if (newFilter.LogicalOperator == null) return;
                if (newFilter.EventFieldFieldID_Field == 0) return;
                Filter newF = filterVM.SaveNewFilter(newFilter);

                // remove events for the added filter
                stopProcessSelectionChanged.Add(FILTER_FIELDSLIST_NAME + lastRowNumber, true);
                stopProcessSelectionChanged.Add(FILTER_LOGICALOPERATORLIST_NAME + lastRowNumber, true);
                stopProcessSelectionChanged.Add(FILTER_VALUE_NAME + lastRowNumber, true);   
                
                newFilter = null;
                AddFilterButton.IsEnabled = true;
                newFilterRulesButton.IsEnabled = true;
                
                this.filterIds.Add(lastRowNumber, newF.ID_Filter);
            }

            // Update the existing filters
            int id_filter_name = 2;
            foreach (Filter item in filterVM.Filters)
            {
                ComboBox fields = FindName(FILTER_FIELDSLIST_NAME + id_filter_name) as ComboBox;
                int current_field_id = (fields.SelectedValue as Field).ID_Field;
                ComboBox logicalOperators = FindName(FILTER_LOGICALOPERATORLIST_NAME + id_filter_name) as ComboBox;
                string current_op = logicalOperators.SelectedItem as string;
                TextBox value = FindName(FILTER_VALUE_NAME + id_filter_name) as TextBox;
                string current_value = value.Text;

                item.EventFieldFieldID_Field = current_field_id;
                item.LogicalOperator = current_op;
                item.Value = current_value;
                id_filter_name += 1;
            }
            filterVM.UpdateAllFilters();
        }
    }
}

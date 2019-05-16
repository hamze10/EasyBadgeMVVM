using EasyBadgeMVVM.Filters;
using EasyBadgeMVVM.Models;
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
        Dictionary<string, bool> stopProcessSelectionChanged;
        Dictionary<int, int> filterIds;
        private static readonly IEnumerable<string> LOGICAL_OPERATORS = FiltersHelper.AllLogicalOperators;
        public static readonly string FILTER_FIELDSLIST_NAME = "filterfieldslist";
        public static readonly string FILTER_LOGICALOPERATORLIST_NAME = "filterlogicaloperatorlist";
        public static readonly string FILTER_VALUE_NAME = "filtervalue";
        public static readonly string RULES_BUTTON_NAME = "rulesbuttonname_";
        HashSet<int> deletedFilters;  // name ids
        public static readonly string FILTER_DELETE_FILTER_NAME = "filterdelete_";
        public static readonly string FILTER_ID = "filter_id_";

        private FilterVM filterVM;
        private int eventId;
        private int lastRowNumber = 1;
        private FilterSet newFilter;    // new filter not yet saved
        private Button newFilterRulesButton;

        public FiltersWindow(int eventId)
        {
            this.eventId = eventId;
            this.filterVM = new FilterVM(eventId);
            this.newFilter = null;
            this.filterIds = new Dictionary<int, int>();
            this.stopProcessSelectionChanged = new Dictionary<string, bool>();
            this.deletedFilters = new HashSet<int>();
            InitializeComponent();
            InitializeFilterScreen();
        }

        /// <summary>
        /// Fetch the existing filters from DB for this event.
        /// Build the UI.
        /// </summary>
        private void InitializeFilterScreen()
        {
            foreach(FilterSet item in filterVM.Filters)
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
            newFilter = new FilterSet
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
                        newFilter.EventFieldFieldID_Field = (fieldsList.SelectedValue as FieldSet).ID_Field;
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
            configureRulesButton.Content = "Edit Rules";
            configureRulesButton.Name = RULES_BUTTON_NAME + lastRowNumber;
            configureRulesButton.SetValue(Grid.ColumnProperty, 4);
            configureRulesButton.Width = 120;
            configureRulesButton.SetValue(Grid.RowProperty, lastRowNumber);
            configureRulesButton.VerticalAlignment = VerticalAlignment.Top;
            configureRulesButton.Click += new RoutedEventHandler(OnClick_Rules);
            RegisterName(RULES_BUTTON_NAME + lastRowNumber, configureRulesButton);
            this.FiltersGrid.Children.Add(configureRulesButton);

            Button deleteFilter = new Button();
            deleteFilter.Content = "X";
            deleteFilter.IsEnabled = false;
            deleteFilter.Name = FILTER_DELETE_FILTER_NAME + lastRowNumber;
            deleteFilter.SetValue(Grid.ColumnProperty, 5);
            deleteFilter.Width = 50;
            deleteFilter.SetValue(Grid.RowProperty, lastRowNumber);
            deleteFilter.VerticalAlignment = VerticalAlignment.Top;
            deleteFilter.Click += new RoutedEventHandler(OnClick_DeleteFilter);
            RegisterName(FILTER_DELETE_FILTER_NAME + lastRowNumber, deleteFilter);
            this.FiltersGrid.Children.Add(deleteFilter);

            newFilterRulesButton = configureRulesButton;
        }

        /// <summary>
        /// Create and display the given filter row in UI
        /// </summary>
        private void AddNewFilterRow(FilterSet filter)
        {
            lastRowNumber += 1;

            RowDefinition rowDefinition = new RowDefinition();
            rowDefinition.Height = new GridLength(80);
            this.FiltersGrid.RowDefinitions.Add(rowDefinition);

            TextBox filterId = new TextBox();
            filterId.Name = FILTER_ID + lastRowNumber;
            filterId.Text = filter.ID_Filter.ToString();
            filterId.SetValue(Grid.ColumnProperty, 0);
            filterId.SetValue(Grid.RowProperty, lastRowNumber);
            filterId.Visibility = Visibility.Hidden;
            RegisterName(FILTER_ID + lastRowNumber, filterId);
            this.FiltersGrid.Children.Add(filterId);

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
            configureRulesButton.Content = "Edit Rules";
            configureRulesButton.Name = RULES_BUTTON_NAME + lastRowNumber;
            configureRulesButton.SetValue(Grid.ColumnProperty, 4);
            configureRulesButton.Width = 120;
            configureRulesButton.SetValue(Grid.RowProperty, lastRowNumber);
            configureRulesButton.VerticalAlignment = VerticalAlignment.Top;
            configureRulesButton.Click += new RoutedEventHandler(OnClick_Rules);
            RegisterName(RULES_BUTTON_NAME + lastRowNumber, configureRulesButton);
            this.FiltersGrid.Children.Add(configureRulesButton);

            Button deleteFilter = new Button();
            deleteFilter.Content = "X";
            deleteFilter.Name = FILTER_DELETE_FILTER_NAME + lastRowNumber;
            deleteFilter.SetValue(Grid.ColumnProperty, 5);
            deleteFilter.Width = 50;
            deleteFilter.SetValue(Grid.RowProperty, lastRowNumber);
            deleteFilter.VerticalAlignment = VerticalAlignment.Top;
            deleteFilter.Click += new RoutedEventHandler(OnClick_DeleteFilter);
            RegisterName(FILTER_DELETE_FILTER_NAME + lastRowNumber, deleteFilter);
            this.FiltersGrid.Children.Add(deleteFilter);

            this.filterIds.Add(lastRowNumber, filter.ID_Filter);
        }


        /************ LISTENERS ************/

        private void OnClick_DeleteFilter(object sender, RoutedEventArgs e)
        {
            Button clicked = e.Source as Button;
            int rowNumber = Convert.ToInt32(clicked.Name.Substring(FILTER_DELETE_FILTER_NAME.Length));

            TextBox filterId = FindName(FILTER_ID + rowNumber) as TextBox;
            if (filterId == null) return; // not yet persisted

            ComboBox fields = FindName(FILTER_FIELDSLIST_NAME + rowNumber) as ComboBox;
            ComboBox logicalOperators = FindName(FILTER_LOGICALOPERATORLIST_NAME + rowNumber) as ComboBox;
            TextBox value = FindName(FILTER_VALUE_NAME + rowNumber) as TextBox;
            Button deleteButton = FindName(FILTER_DELETE_FILTER_NAME + rowNumber) as Button;
            Button configureRulesButton = FindName(RULES_BUTTON_NAME + rowNumber) as Button;
            this.FiltersGrid.Children.Remove(fields);
            this.FiltersGrid.Children.Remove(logicalOperators);
            this.FiltersGrid.Children.Remove(value);
            this.FiltersGrid.Children.Remove(deleteButton);
            this.FiltersGrid.Children.Remove(configureRulesButton);
            filterVM.DeleteFilter(Convert.ToInt32(filterId.Text));
            deletedFilters.Add(rowNumber);
        }

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
                FilterSet newF = filterVM.SaveNewFilter(newFilter);

                // remove events for the added filter
                stopProcessSelectionChanged.Add(FILTER_FIELDSLIST_NAME + lastRowNumber, true);
                stopProcessSelectionChanged.Add(FILTER_LOGICALOPERATORLIST_NAME + lastRowNumber, true);
                stopProcessSelectionChanged.Add(FILTER_VALUE_NAME + lastRowNumber, true);   
                
                newFilter = null;
                AddFilterButton.IsEnabled = true;
                newFilterRulesButton.IsEnabled = true;

                Button deleteButton = FindName(FILTER_DELETE_FILTER_NAME + lastRowNumber) as Button;
                deleteButton.IsEnabled = true;

                TextBox filterId = new TextBox();
                filterId.Name = FILTER_ID + lastRowNumber;
                filterId.Text = newF.ID_Filter.ToString();
                filterId.SetValue(Grid.ColumnProperty, 0);
                filterId.SetValue(Grid.RowProperty, lastRowNumber);
                filterId.Visibility = Visibility.Hidden;
                RegisterName(FILTER_ID + lastRowNumber, filterId);
                this.FiltersGrid.Children.Add(filterId);

                this.filterIds.Add(lastRowNumber, newF.ID_Filter);
            }

            // Update the existing filters
            int id_filter_name = 2;
            foreach (FilterSet item in filterVM.Filters)
            {
                if (deletedFilters.Contains(id_filter_name))
                {
                    id_filter_name += 1;
                    continue;
                };

                ComboBox fields = FindName(FILTER_FIELDSLIST_NAME + id_filter_name) as ComboBox;
                int current_field_id = (fields.SelectedValue as FieldSet).ID_Field;
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

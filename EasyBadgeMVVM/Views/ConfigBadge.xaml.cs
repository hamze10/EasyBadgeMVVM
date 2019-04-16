using EasyBadgeMVVM.Models;
using EasyBadgeMVVM.ViewModels;

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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;

namespace EasyBadgeMVVM.Views
{
    /// <summary>
    /// Interaction logic for PrintBadge.xaml
    /// </summary>
    public partial class ConfigBadge : Window
    {
        private IBadgeVM _badgeVM;
        private int _idEvent;
        private bool isAlreadyCalled = false; //TO SHOW ONCE THE LABEL IN BADGESCREEN WHEN DRAG AND DROP
        private static readonly IEnumerable<FontFamily> ALLFONTS = Fonts.SystemFontFamilies.OrderBy(x => x.Source);
        private static readonly IEnumerable<int> ALLSIZE = Enumerable.Range(1, 120 - 1);
        private const string LABELFIELDNAME = "labelfieldname";
        private const string COMBOBOXFONTFAMILYNAME = "comboboxfontfamilyname";
        private const string COMBOBOXFONTSIZENAME = "comboboxfontsizename";
        private const string GRIDLABEL = "grindlabel";

        public ConfigBadge(int idEvent)
        {
            this._idEvent = idEvent;
            this._badgeVM = new BadgeVM(idEvent);
            this.DataContext = this._badgeVM;
            InitializeComponent();
            CreateLabels();
        }

        private void CreateLabels()
        {
            List<string> listFields = this._badgeVM.GetAllFields();
            for (int i = 0; i < listFields.Count; i++)
            {
                Label l = new Label();
                l.Content = listFields.ElementAt(i);
                l.VerticalAlignment = VerticalAlignment.Center;
                l.VerticalContentAlignment = VerticalAlignment.Center;
                l.HorizontalAlignment = HorizontalAlignment.Left;
                l.HorizontalContentAlignment = HorizontalAlignment.Center;
                l.Name = LABELFIELDNAME + i;
                l.MouseMove += new MouseEventHandler(Label_MouseMove);
                RegisterName(LABELFIELDNAME + i, l);


                ComboBox comboBoxFontFamily = new ComboBox();
                comboBoxFontFamily.Width = 200;
                comboBoxFontFamily.SetValue(VirtualizingStackPanel.IsVirtualizingProperty, true);
                comboBoxFontFamily.ItemsSource = ALLFONTS;
                comboBoxFontFamily.VerticalAlignment = VerticalAlignment.Center;
                comboBoxFontFamily.VerticalContentAlignment = VerticalAlignment.Center;
                comboBoxFontFamily.HorizontalAlignment = HorizontalAlignment.Center;
                comboBoxFontFamily.HorizontalContentAlignment = HorizontalAlignment.Center;
                comboBoxFontFamily.Name = COMBOBOXFONTFAMILYNAME + i;
                comboBoxFontFamily.SelectionChanged += new SelectionChangedEventHandler(FamilySelectionChanged);
                RegisterName(COMBOBOXFONTFAMILYNAME + i, comboBoxFontFamily);

                ComboBox comboxBoxFontSize = new ComboBox();
                comboxBoxFontSize.Width = 100;
                comboxBoxFontSize.SetValue(VirtualizingStackPanel.IsVirtualizingProperty, true);
                comboxBoxFontSize.ItemsSource = ALLSIZE;
                comboxBoxFontSize.VerticalAlignment = VerticalAlignment.Center;
                comboxBoxFontSize.VerticalContentAlignment = VerticalAlignment.Center;
                comboxBoxFontSize.HorizontalAlignment = HorizontalAlignment.Right;
                comboxBoxFontSize.HorizontalContentAlignment = HorizontalAlignment.Center;
                comboxBoxFontSize.Name = COMBOBOXFONTSIZENAME + i;
                comboxBoxFontSize.SelectionChanged += new SelectionChangedEventHandler(SizeSelectionChanged);
                RegisterName(COMBOBOXFONTSIZENAME + i, comboxBoxFontSize);

                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(50);
                this.BadgeLabels.RowDefinitions.Add(rowDefinition);

                Grid grid = new Grid();
                grid.SetValue(Grid.RowProperty, i);
                grid.Margin = new Thickness(10, 0, 10, 0);
                grid.Children.Add(l);
                grid.Children.Add(comboBoxFontFamily);
                grid.Children.Add(comboxBoxFontSize);
                grid.Name = GRIDLABEL + i;
                RegisterName(GRIDLABEL + i, grid);

                this.BadgeLabels.Children.Add(grid);
            }
        }

        private void FamilySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            string fontFamily = cmb.SelectedValue.ToString();
            int index = Int32.Parse(cmb.Name.Split(new string[] { COMBOBOXFONTFAMILYNAME }, StringSplitOptions.None)[1]);
            Label label = (Label) this.FindName(LABELFIELDNAME + index);
            label.FontFamily = ALLFONTS.Where(f => f.ToString().Equals(fontFamily)).FirstOrDefault();
        }

        private void SizeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            int index = Int32.Parse(cmb.Name.Split(new string[] { COMBOBOXFONTSIZENAME }, StringSplitOptions.None)[1]);
            Label label = (Label)this.FindName(LABELFIELDNAME + index);
            label.FontSize = Double.Parse(cmb.SelectedValue.ToString());
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            Canvas c = this.BadgeScreen;

            //http://classicalprogrammer.wikidot.com/don-t-delete-an-element-from-collection-in-foreach-loop
            for (int inx = c.Children.Count - 1; inx >= 0; inx--)
            {
                if (c.Children[inx] is Label) c.Children.Remove(c.Children[inx]);
            }

            BadgeDTO selected = this._badgeVM.SelectedBadge;
            this.BadgeScreen.Background = Brushes.White;
            this.BadgeScreen.AllowDrop = true;
            this.BadgeScreen.Drop += new DragEventHandler(Drag_Drop);
            this.BadgeScreen.DragEnter += new DragEventHandler(Drag_DragEnter);
            this.BadgeScreen.Width = selected.Width;
            this.BadgeScreen.Height = selected.Height;

            //CHECK IN THE DB IF A BADGE IN POSITION NOT ALREADY EXISTS, IF EXISTS SHOW IT
            List<Position> myPositions = this._badgeVM.GetPositions(selected.ID, this._idEvent);
            foreach(Position p in myPositions)
            {
                Label label = new Label();
                label.Content = p.Field.Name;
                label.FontFamily = ALLFONTS.Where(font => font.ToString().Equals(p.FontFamily)).FirstOrDefault();
                label.FontSize = p.FontSize;
                label.MouseMove += new MouseEventHandler(Label_MouseMove);
                label.PreviewMouseRightButtonDown += new MouseButtonEventHandler(Label_RightClick);
                label.AllowDrop = true;
                label.Drop += new DragEventHandler(Drag_Drop);
                label.DragEnter += new DragEventHandler(Drag_DragEnter);

                var child = this.BadgeScreen.Children.OfType<Label>().Where(la => la == label).FirstOrDefault();
                if (child != null) c.Children.Remove(child);

                c.Children.Add(label);
                Canvas.SetLeft(label, p.Position_X);
                Canvas.SetTop(label, p.Position_Y);
            }
        }

        private void Label_MouseMove(object sender, MouseEventArgs e)
        {
            Label l = sender as Label;
            if (l != null && e.LeftButton == MouseButtonState.Pressed)
            {
                isAlreadyCalled = true;
                var dragData = new DataObject(typeof(Label), l);
                DragDrop.DoDragDrop(l, dragData, DragDropEffects.Move);
            }
        }

        int i = 0;
        private void Drag_Drop(object sender, DragEventArgs e)
        {
            try
            {
                Canvas c = sender as Canvas;
                var dataObj = e.Data as DataObject;
                c.Width = this.BadgeScreen.Width;
                c.Height = this.BadgeScreen.Height;
                if (c != null)
                {
                    // If the DataObject contains string data, extract it.
                    if (dataObj != null && isAlreadyCalled)
                    {
                        Label la1 = dataObj.GetData(typeof(Label)) as Label;
                        Label l = new Label();
                        l.Name = "labelDrop" + i++;
                        l.FontFamily = la1.FontFamily;
                        l.FontSize = la1.FontSize;
                        l.Content = la1.Content;
                        l.MouseMove += new MouseEventHandler(Label_MouseMove);
                        l.PreviewMouseRightButtonDown += new MouseButtonEventHandler(Label_RightClick);
                        l.AllowDrop = true;
                        l.Drop += new DragEventHandler(Drag_Drop);
                        l.DragEnter += new DragEventHandler(Drag_DragEnter);

                        Point position = e.GetPosition(c);

                        var child = this.BadgeScreen.Children.OfType<Label>().Where(la => la == la1).FirstOrDefault();
                        if (child != null) c.Children.Remove(child);

                        c.Children.Add(l);
                        Canvas.SetLeft(l, position.X);
                        Canvas.SetTop(l, position.Y);
                        isAlreadyCalled = false;
                    }
                }
            }
            catch(Exception) { }
        }

        private void Label_RightClick(object sender, MouseButtonEventArgs e)
        {
            this.BadgeScreen.Children.Remove(sender as Label);
        }

        private void Drag_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = !e.Data.GetDataPresent(DataFormats.StringFormat) || sender == e.Source
                        ? DragDropEffects.None
                        : DragDropEffects.All;
        }

        private void Save_Positions(object sender, RoutedEventArgs e)
        {
            //Save on BadgeEvent
            BadgeEvent insertedBe = this._badgeVM.SaveOnBadgeEvent();

            //Save on Position
            //But first delete if there are updates
            this._badgeVM.RemoveRowsPosition();
            foreach (FrameworkElement child in this.BadgeScreen.Children)
            {
                Label label = child as Label;

                string fontFamily = label.FontFamily.ToString(); //FontFamily
                int fontSize = Convert.ToInt32(label.FontSize); //FontSize
                double posY = (double)child.GetValue(Canvas.TopProperty); //Pos_Y
                double posX = (double)child.GetValue(Canvas.LeftProperty); //Pos_X
                Field field = this._badgeVM.GetFieldByName(label.Content.ToString()); //Field

                this._badgeVM.SaveOnPosition(insertedBe, field, posX, posY, fontFamily, fontSize);
            }

            var messageQueue = this.SnackbarBadge.MessageQueue;
            Task.Factory.StartNew(() => messageQueue.Enqueue("Your badge has been saved"));
        }

        //https://www.wundervisionenvisionthefuture.com/blog/wpf-c-drag-and-drop-icon-adorner
        /*private class DraggableAdorner : Adorner
        {

            public DraggableAdorner(PrintBadge adornedElement) : base(adornedElement)
            {
                this.IsHitTestVisible = false;
            }

            protected override void OnRender(DrawingContext drawingContext)
            {
                base.OnRender(drawingContext);
            }
        }*/
    }
}

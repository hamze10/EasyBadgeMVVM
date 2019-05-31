using EasyBadgeMVVM.Models;
using EasyBadgeMVVM.ViewModels;

using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Media.Animation;

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
        private const string ANGLEPLUS = "anglePlus";
        private const string ANGLEMINUS = "angleMinus";
        private const string LABELDROP = "labelDrop";

        private const string PRIMARY_COLOR = "#0a3d62";
        private const string ERROR_COLOR = "#c0392b";

        private double widthBadge = -1;
        private double heightBadge = -1;

        private IDictionary<string, double> angleOfLabel; //USED FOR ANGLE ROTATION

        public ConfigBadge(int idEvent)
        {
            this._idEvent = idEvent;
            this._badgeVM = new BadgeVM(idEvent);
            this.DataContext = this._badgeVM;
            this.angleOfLabel = new Dictionary<string, double>();
            InitializeComponent();
            CreateLabels();
        }

        private void CreateLabels()
        {
            this.angleOfLabel.Clear();
            List<string> listFields = this._badgeVM.GetAllFields();
            for (int i = 0; i < listFields.Count; i++)
            {

                DockPanel dockPanel = new DockPanel();

                Label angleMinus = new Label();
                angleMinus.Name = ANGLEMINUS + i;
                angleMinus.Content = "-";
                angleMinus.MouseLeftButtonUp += new MouseButtonEventHandler(DecrementRotation);
                RegisterName(ANGLEMINUS + i, angleMinus);

                Label l = new Label();
                l.Content = listFields.ElementAt(i);
                l.Name = LABELFIELDNAME + i;
                l.MouseMove += new MouseEventHandler(Label_MouseMove);
                /*Binding binding = new Binding("RotationAngle");
                RotateTransform rotateTransform = new RotateTransform();
                BindingOperations.SetBinding(rotateTransform, RotateTransform.AngleProperty, binding);
                l.LayoutTransform = rotateTransform;*/
                RegisterName(LABELFIELDNAME + i, l);

                this.angleOfLabel.Add(l.Name, 0);

                Label anglePlus = new Label();
                anglePlus.Name = ANGLEPLUS + i;
                anglePlus.Content = "+";
                anglePlus.MouseLeftButtonUp += new MouseButtonEventHandler(IncrementRotation);
                RegisterName(ANGLEPLUS + i, anglePlus);

                dockPanel.Children.Add(angleMinus);
                dockPanel.Children.Add(l);
                dockPanel.Children.Add(anglePlus);

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
                grid.Children.Add(dockPanel);
                grid.Children.Add(comboBoxFontFamily);
                grid.Children.Add(comboxBoxFontSize);
                grid.Name = GRIDLABEL + i;
                RegisterName(GRIDLABEL + i, grid);

                this.BadgeLabels.Children.Add(grid);
            }
        }

        private void IncrementRotation(object sender, MouseButtonEventArgs e)
        {
            string name = (sender as Label).Name.Split(new string[] { ANGLEPLUS }, StringSplitOptions.None)[1];
            Label labelName = (Label) (this.FindName(LABELFIELDNAME + name) as Label);
            ++this.angleOfLabel[labelName.Name];
            labelName.LayoutTransform = new RotateTransform(this.angleOfLabel[labelName.Name]);
        }

        private void DecrementRotation(object sender, MouseButtonEventArgs e)
        {
            string name = (sender as Label).Name.Split(new string[] { ANGLEMINUS }, StringSplitOptions.None)[1];
            Label labelName = (Label)(this.FindName(LABELFIELDNAME + name) as Label);
            --this.angleOfLabel[labelName.Name];
            labelName.LayoutTransform = new RotateTransform(this.angleOfLabel[labelName.Name]);
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
                if (c.Children[inx] is Label || c.Children[inx] is Line) c.Children.Remove(c.Children[inx]);
            }

            this.imgCanvas.Source = new BitmapImage();

            BadgeDTO selected = this._badgeVM.SelectedBadge;
            this.BadgeScreen.Background = Brushes.White;
            this.BadgeScreen.AllowDrop = true;
            this.BadgeScreen.Drop += new DragEventHandler(Drag_Drop);
            this.BadgeScreen.DragEnter += new DragEventHandler(Drag_DragEnter);
            this.BadgeScreen.Width = selected.Width;
            this.BadgeScreen.Height = selected.Height;

            this.widthBadge = selected.Width;
            this.heightBadge = selected.Height;

            this._badgeVM.SelectedTemplate = selected.Template;

            //CHECK IN THE DB IF A BADGE IN POSITION NOT ALREADY EXISTS, IF EXISTS SHOW IT
            List<PositionSet> myPositions = this._badgeVM.GetPositions(selected.ID, this._idEvent, selected.Template);
            foreach(PositionSet p in myPositions)
            {
                Label label = new Label();
                label.Content = p.FieldSet.Name;
                label.FontFamily = ALLFONTS.Where(font => font.ToString().Equals(p.FontFamily)).FirstOrDefault();
                label.FontSize = p.FontSize;
                label.LayoutTransform = new RotateTransform(p.AngleRotation.Value);
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
                        l.Name = LABELDROP + i++;
                        l.FontFamily = la1.FontFamily;
                        l.FontSize = la1.FontSize;
                        l.Content = la1.Content;
                        l.LayoutTransform = la1.LayoutTransform;
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
            string templateName = this.TemplateBadgeName.Text;
            var messageQueue = this.SnackbarBadge.MessageQueue;

            if (templateName == string.Empty && this._badgeVM.SelectedBadgeEvent == 0)
            {
                this.ShowNotification("Please enter a valid template name", ERROR_COLOR);
                return;
            }

            if (this._badgeVM.SelectedBadge == null && this._badgeVM.SelectedBadgeEvent == 0)
            {
                this.ShowNotification("Please select a template", ERROR_COLOR);
                return;
            }

            if ( (templateName == string.Empty || this._badgeVM.SelectedBadge == null) && this._badgeVM.SelectedBadgeEvent != 0)
            {
                this._badgeVM.UpdateDefaultPrint();
                this.ShowNotification("Default print updated.");
                return;
            }

            //Save on BadgeEvent
            BitmapSource imageSrc = this.BackgroundChecked.IsChecked.Value ? ((TransformedBitmap)this.imgCanvas.Source).Source : null;
            BadgeEventSet insertedBe = this._badgeVM.SaveOnBadgeEvent(templateName, imageSrc);

            //Save on Position
            //But first delete if there are updates
            this._badgeVM.RemoveRowsPosition(templateName);

            foreach (FrameworkElement child in this.BadgeScreen.Children)
            {
                Label label = child as Label;
                if (label == null) continue;
                string fontFamily = label.FontFamily.ToString(); //FontFamily
                int fontSize = Convert.ToInt32(label.FontSize); //FontSize
                double posY = (double)child.GetValue(Canvas.TopProperty); //Pos_Y
                double posX = (double)child.GetValue(Canvas.LeftProperty); //Pos_X
                FieldSet field = this._badgeVM.GetFieldByName(label.Content.ToString()); //Field
                Label getLabel = (Label) this.FindName(LABELFIELDNAME + label.Name.Split(new string[] { LABELDROP, LABELFIELDNAME }, StringSplitOptions.None)[1]); 
                double layoutTransform = this.angleOfLabel[getLabel.Name];

                this._badgeVM.SaveOnPosition(insertedBe, field, posX, posY, fontFamily, fontSize, layoutTransform);
            }

            this._badgeVM.RefreshListBadgeType();
            this._badgeVM.UpdateDefaultPrint();

            this.ShowNotification("Your badge has been saved");
        }

        private void Load_Picture(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog op = new System.Windows.Forms.OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var bitmapToChange = new BitmapImage(new Uri(op.FileName));
                var bitmapTrans = new TransformedBitmap(bitmapToChange, new ScaleTransform(
                        this.BadgeScreen.Width / bitmapToChange.PixelWidth,
                        this.BadgeScreen.Height / bitmapToChange.PixelHeight
                    ));
                this.imgCanvas.Source = bitmapTrans;
            }
        }

        private void Add_NewBadge(object sender, RoutedEventArgs e)
        {
            string name = this.NewBadgeName.Text;
            string typeBadge = this.NewBadgeTypeBadge.Text;

            if (name == string.Empty || typeBadge == string.Empty)
            {
                MessageBox.Show("Fields must not be empty");
                return;
            }

            int dimensionX = -1;
            int dimensionY = -1;

            try
            {
                dimensionX = Convert.ToInt32(this.NewBadgeDimensionX.Text);
                dimensionY = Convert.ToInt32(this.NewBadgeDimensionY.Text);
            }catch(Exception)
            {
                MessageBox.Show("Please enter a valid number for dimensionX/dimensionY");
                return;
            }

            this._badgeVM.SaveOnBadge(name, typeBadge, dimensionX, dimensionY);
        }

        private void Draw_Gridlines(object sender, RoutedEventArgs e)
        {
            if (this.DataGridBadge.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a template before");
                return;
            }

            int x = -1;
            int y = -1;

            try
            {
                x = Convert.ToInt32(this.GridShowX.Text);
                y = Convert.ToInt32(this.GridShowY.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Please enter a valid number for dimensions");
                return;
            }

            Canvas c = this.BadgeScreen;

            //http://classicalprogrammer.wikidot.com/don-t-delete-an-element-from-collection-in-foreach-loop
            for (int inx = c.Children.Count - 1; inx >= 0; inx--)
            {
                if (c.Children[inx] is Line) c.Children.Remove(c.Children[inx]);
            }

            for (double i = 0; i <= this.heightBadge; i+= this.heightBadge / x)
            {
                //Horizontal Line
                Line lineHorizontal = new Line();
                lineHorizontal.X1 = 0;
                lineHorizontal.Y1 = i;
                lineHorizontal.X2 = this.widthBadge;
                lineHorizontal.Y2 = i;
                lineHorizontal.Stroke = Brushes.Gray;
                lineHorizontal.StrokeThickness = 0.5;

                this.BadgeScreen.Children.Add(lineHorizontal);
            }

            for (double j = 0; j <= this.widthBadge; j += this.widthBadge / y)
            {
                //Vertical Line
                Line lineVertical = new Line();
                lineVertical.X1 = j;
                lineVertical.Y1 = 0;
                lineVertical.X2 = j;
                lineVertical.Y2 = this.heightBadge;
                lineVertical.Stroke = Brushes.Gray;
                lineVertical.StrokeThickness = 0.5;

                this.BadgeScreen.Children.Add(lineVertical);
            }
        }

        private bool HideDataGrid = false;
        private void HideShowDataGrid(object sender, RoutedEventArgs e)
        {
            this.HideDataGrid = !this.HideDataGrid;
            Visibility visibility = this.HideDataGrid ? Visibility.Hidden : Visibility.Visible;
            this.DataGridData.SetValue(Grid.VisibilityProperty, visibility);
            int rowProp = this.HideDataGrid ? 0 : 2;
            this.BadgeScreenGrid.SetValue(Grid.RowProperty, rowProp);
            this.ButtonAddBadgeGrid.SetValue(Grid.RowProperty, rowProp == 0 ? 0 : 1);
            this.ButtonGridLinesGrid.SetValue(Grid.RowProperty, rowProp == 0 ? 0 : 1);
            this.ButtonHideShowGrid.SetValue(Grid.RowProperty, rowProp == 0 ? 0 : 1);
            this.BadgeScreenGrid.SetValue(Grid.RowSpanProperty, rowProp == 0 ? 3 : 1);
            this.ButtonAddBadgeGrid.SetValue(Grid.VisibilityProperty, rowProp == 0 ? Visibility.Hidden : Visibility.Visible);
            this.ButtonGridLinesGrid.SetValue(Grid.MarginProperty, rowProp == 0 ? new Thickness(0, 10, 0, 0) : new Thickness(0, 0, 0, 0));
            this.ButtonHideShowGrid.SetValue(Grid.MarginProperty, rowProp == 0 ? new Thickness(0, 0, 0, 150) : new Thickness(0, -8, 10, 28));
            this.BadgeScreenGrid.SetValue(Grid.MarginProperty, rowProp == 0 ? new Thickness(0, 50, 0, 0) : new Thickness(0, 0, 0, 0));
            this.OtherInformation.SetValue(Grid.RowProperty, rowProp == 0 ? 0 : 1);
            this.OtherInformation.SetValue(Grid.MarginProperty, rowProp == 0 ? new Thickness(20, 50, 0, 0) : new Thickness(20, 0, 0, 0));
            this.FieldsBadgingGrid.SetValue(Grid.MarginProperty, rowProp == 0 ? new Thickness(20, 20, 20, 146) : new Thickness(20, 175, 20, 146));
            this.DefaultPrintGrid.SetValue(Grid.MarginProperty, rowProp == 0 ? new Thickness(0, -10, 0, 0) : new Thickness(0, 0, 0, 0));
            this.ButtonHideShow.Content = this.HideDataGrid ? "Show" : "Hide";
        }

        private void ShowNotification(string message, string color = PRIMARY_COLOR)
        {
            var messageQueue = this.SnackbarBadge.MessageQueue;
            this.SnackbarBadge.Background = (Brush)new BrushConverter().ConvertFrom(color);
            Task.Factory.StartNew(() => messageQueue.Enqueue(message));
        }
    }
}

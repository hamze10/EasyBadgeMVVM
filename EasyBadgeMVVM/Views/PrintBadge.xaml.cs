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
    /// Interaction logic for PrintBadge.xaml
    /// </summary>
    public partial class PrintBadge : Window
    {

        public List<BadgeDTO> ListBadgeType { get; set; }
        public BadgeDTO SelectedBadge { get; set; }
        private bool isAlreadyCalled = false; //TO SHOW ONCE THE LABEL IN BADGESCREEN WHEN DRAG AND DROP
        private List<FontFamily> ALLFONTS = Fonts.SystemFontFamilies.OrderBy(x => x.Source).ToList();

        public PrintBadge()
        {
            this.DataContext = this;
            CreateDataGrid();
            InitializeComponent();
            CreateLabels();
        }

        private void CreateDataGrid()
        {
            List<string> listName = new List<string>(new string[] { "PVC", "butte" });
            List<string> listType = new List<string>(new string[] { "PVC", "A4", "A3", "A5" });

            BadgeDTO[] badgeDTOs = new BadgeDTO[40];
            Random r = new Random();
            for (int i = 1; i <= 40; i++)
            {
                BadgeDTO b = new BadgeDTO();
                b.id = i;
                b.name = listName.ElementAt(r.Next(0, listName.Count));
                b.height = r.NextDouble() * (600 - 200) + 200;
                b.width = r.NextDouble() * (600 - 200) + 200;
                b.type = listType.ElementAt(r.Next(0, listType.Count));
                badgeDTOs[i - 1] = b;
            }

            this.ListBadgeType = new List<BadgeDTO>(badgeDTOs);
        }

        private void CreateLabels()
        {
            string[] content = new string[] { "id", "name", "height", "width", "type" };
            for (int i = 0; i < 5; i++)
            {
                Label l = new Label();
                l.Content = content[i];
                l.MouseMove += new MouseEventHandler(label_MouseMove);

                ComboBox comboBox = new ComboBox();
                comboBox.Width = 100;
                comboBox.SetValue(VirtualizingStackPanel.IsVirtualizingProperty, true);
                comboBox.ItemsSource = ALLFONTS;


                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(50);
                this.BadgeLabels.RowDefinitions.Add(rowDefinition);

                Grid grid = new Grid();
                grid.SetValue(Grid.RowProperty, i);
                grid.Children.Add(l);
                grid.Children.Add(comboBox);

                this.BadgeLabels.Children.Add(grid);
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            //var child = this.BadgeScreen.Children.OfType<Label>().FirstOrDefault(l => l.Name == "labelDrop");
            //if (child != null) this.BadgeScreen.Children.Remove(child);

            BadgeDTO selected = this.SelectedBadge;
            this.BadgeScreen.Background = Brushes.White;
            this.BadgeScreen.AllowDrop = true;
            this.BadgeScreen.Drop += (sender2, e2) => drag_Drop(sender2, e2, false, null);
            this.BadgeScreen.DragEnter += new DragEventHandler(drag_DragEnter);
            this.BadgeScreen.Width = selected.width;
            this.BadgeScreen.Height = selected.height;
        }

        private void label_MouseMove(object sender, MouseEventArgs e)
        {
            Label l = sender as Label;
            if (l != null && e.LeftButton == MouseButtonState.Pressed)
            {
                isAlreadyCalled = true;
                DragDrop.DoDragDrop(l, l.Content.ToString(), DragDropEffects.All);
            }
        }

        private void drag_Drop(object sender, DragEventArgs e, bool isOnBadgeScreen, Label label)
        {
            try
            {
                Canvas c = sender as Canvas;
                c.Width = this.BadgeScreen.Width;
                c.Height = this.BadgeScreen.Height;
                if (c != null)
                {
                    // If the DataObject contains string data, extract it.
                    if (e.Data.GetDataPresent(DataFormats.StringFormat) && isAlreadyCalled)
                    {
                        Label l = new Label();
                        l.Name = "labelDrop";
                        l.Content = e.Data.GetData(DataFormats.StringFormat);
                        l.MouseMove += new MouseEventHandler(label_MouseMove);
                        l.PreviewMouseRightButtonDown += new MouseButtonEventHandler(label_RightClick);
                        l.AllowDrop = true;
                        l.Drop += (sender2, e2) => drag_Drop(sender2, e2, true, l);
                        l.DragEnter += new DragEventHandler(drag_DragEnter);

                        Point position = e.GetPosition(c);
                        //Console.WriteLine("bool : {0} | label : {1}", isOnBadgeScreen, label == null ? "null" : label.Name);
                        if (isOnBadgeScreen == true)
                        {
                            var child = this.BadgeScreen.Children.OfType<Label>().Where(la => la == label).FirstOrDefault();
                            if (child != null) c.Children.Remove(child);
                        }

                        c.Children.Add(l);
                        Canvas.SetLeft(l, position.X);
                        Canvas.SetTop(l, position.Y);
                        isAlreadyCalled = false;
                    }
                }
            }
            catch(Exception) { }
        }

        private void label_RightClick(object sender, MouseButtonEventArgs e)
        {
            this.BadgeScreen.Children.Remove(sender as Label);
        }

        private void drag_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.StringFormat) || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
            else
            {
                e.Effects = DragDropEffects.All;
            }
        }
    }

    public class BadgeDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public double height { get; set; }
        public double width { get; set; }
        public string type { get; set; }
    }
}

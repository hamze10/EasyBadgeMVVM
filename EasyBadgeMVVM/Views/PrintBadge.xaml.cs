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

                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(50);
                this.BadgeLabels.RowDefinitions.Add(rowDefinition);

                Grid grid = new Grid();
                grid.SetValue(Grid.RowProperty, i);
                grid.Children.Add(l);

                this.BadgeLabels.Children.Add(grid);
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            BadgeDTO selected = this.SelectedBadge;
            this.BadgeScreen.Background = Brushes.White;
            this.BadgeScreen.AllowDrop = true;
            this.BadgeScreen.Drop += new DragEventHandler(drag_Drop);
            this.BadgeScreen.DragEnter += new DragEventHandler(drag_DragEnter);
            this.BadgeScreen.Width = selected.width;
            this.BadgeScreen.Height = selected.height;
        }

        private void label_MouseMove(object sender, MouseEventArgs e)
        {
            Label l = sender as Label;
            if (l != null && e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(l, l.Content, DragDropEffects.Move);
            }
        }

        private void drag_Drop(object sender, DragEventArgs e)
        {
            Canvas c = sender as Canvas;
            if (c != null)
            {
                // If the DataObject contains string data, extract it.
                if (e.Data.GetDataPresent(DataFormats.StringFormat))
                {
                    Label l = new Label();
                    l.Content = e.Data.GetData(DataFormats.StringFormat);

                    Point position = e.GetPosition(this);

                    c.Children.Add(l);
                    Canvas.SetTop(l, position.X);
                    Canvas.SetLeft(l, position.Y);
                   
                }
            }
        }

        private void drag_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.StringFormat) || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
            else
            {
                e.Effects = DragDropEffects.Copy;
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

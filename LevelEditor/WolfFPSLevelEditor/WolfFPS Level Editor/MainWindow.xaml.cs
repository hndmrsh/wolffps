using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WolfFPS_Level_Editor
{
    public enum Direction {
        Up, Down, Left, Right
    }

    public struct TileTag
    {
        public int X { get; set; }
        public int Y { get; set; }

        public override string ToString()
        {
            return "Tile (" + X + "," + Y + ")";
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private TileType selectedTileType = TileType.Door;
        private Level level;

        public MainWindow()
        {
            InitializeComponent();
            GenerateTiles();

            level = new Level();
        }

        private void GenerateTiles()
        {
            for (int x = 0; x < gridTiles.ColumnDefinitions.Count; x++)
            {
                for (int y = 0; y < gridTiles.RowDefinitions.Count; y++)
                {
                    Button tile = new Button();

                    // auto-size
                    tile.Width = Double.NaN;
                    tile.Height = Double.NaN;

                    tile.Click += new RoutedEventHandler(tile_Click);

                    TileTag tag = new TileTag
                    {
                        X = x,
                        Y = y
                    };

                    tile.Tag = tag;

                    Grid.SetRow(tile, y);
                    Grid.SetColumn(tile, x);

                    gridTiles.Children.Add(tile);
                }
            }
        }

        private void tile_Click(object sender, RoutedEventArgs args)
        {
            Button btn = (Button)sender;
            TileTag tag = (TileTag)btn.Tag;

            btn.Content = CreateImageForResource(selectedTileType.GetImageResourceName());

            level.SetTileTypeForTag(tag, selectedTileType);
        }

        private void btn_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton btn = (RadioButton)sender;
            switch (btn.Name)
            {
                case "btnD":
                    selectedTileType = TileType.Door;
                    break;
                case "btn0":
                    selectedTileType = TileType.Floor0;
                    break;
                default:
                    selectedTileType = TileType.Door;
                    break;
            }
        }

        private void PanMap(Direction dir) 
        {
            Debug.WriteLine("Panning map in direction: " + dir.ToString());

            foreach (UIElement child in gridTiles.Children)
            {
                if (child is Button)
                {
                    Button btn = (Button)child;
                    TileTag tag = (TileTag)btn.Tag;
                    
                    tag = UpdateTileTag(tag, dir);
                    btn.Tag = tag;

                    RenderTile(btn, level.GetTileTypeForTag(tag));
                }
            }
        }

        private void RenderTile(Button btn, TileType tileType)
        {
            string res = tileType.GetImageResourceName();
            if (res != null)
            {
                if (!(btn.Content is Image))
                {
                    btn.Content = CreateImageForResource(res);
                }
                else
                {
                    ((Image)btn.Content).Visibility = Visibility.Visible;
                    ((Image)btn.Content).Source = (BitmapImage)FindResource(res);
                }
            }
            else
            {
                if (!(btn.Content is Image))
                {
                    btn.Content = null;
                }
                else
                {
                    ((Image)btn.Content).Visibility = Visibility.Hidden;
                }
            }
            
        }

        private Image CreateImageForResource(string res)
        {
            Image image = new Image();
            image.Source = (BitmapImage)FindResource(res);
            return image;
        }

        private TileTag UpdateTileTag(TileTag tag, Direction dir)
        {        
            switch (dir)
            {
                case Direction.Up:
                    tag.Y--;
                    break;
                case Direction.Down:
                    tag.Y++;
                    break;
                case Direction.Left:
                    tag.X--;
                    break;
                case Direction.Right:
                    tag.X++;
                    break;
            }

            return tag;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W)
            {
                PanMap(Direction.Up);
            }
            else if (e.Key == Key.A)
            {
                PanMap(Direction.Left);
            }
            else if (e.Key == Key.S)
            {
                PanMap(Direction.Down);
            }
            else if (e.Key == Key.D)
            {
                PanMap(Direction.Right);
            }
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            InputOutputUtil.Save(level);
        }


    }
}

using System;
using System.Diagnostics;
using System.IO;
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

                    tile.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(tile_MouseLeftDown);
                    tile.PreviewMouseRightButtonDown += new MouseButtonEventHandler(tile_MouseRightDown);    

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

        void tile_MouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            Button btn = (Button)sender;
            TileTag tag = (TileTag)btn.Tag;

            btn.Content = CreateImageForResource(selectedTileType.GetImageResourceName());

            bool levelChanged = level.SetTileTypeForTag(tag, selectedTileType);
            if (levelChanged)
            {
                UpdateUnsavedIndicator(false);
            }
        }

        void tile_MouseRightDown(object sender, MouseButtonEventArgs e)
        {
            Button btn = (Button)sender;
            TileTag tag = (TileTag)btn.Tag;

            if (btn.Content != null)
            {
                ((Image)btn.Content).Visibility = Visibility.Hidden;
            }

            bool levelChanged = level.DeleteTileAtTag(tag);
            if (levelChanged)
            {
                UpdateUnsavedIndicator(false);
            }
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

        private void ResetViewport()
        {
            foreach (UIElement child in gridTiles.Children)
            {
                if (child is Button)
                {
                    int x = Grid.GetColumn(child);
                    int y = Grid.GetRow(child);

                    Button btn = (Button)child;
                    TileTag tag = new TileTag();
                    tag.X = x;
                    tag.Y = y;
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

        private void UpdateUnsavedIndicator(bool saved)
        {
            if (!saved && !Title.EndsWith("*"))
            {
                Title += "*";
            }
            else if (saved && Title.EndsWith("*"))
            {
                Title = Title.Substring(0, Title.Length - 1);
            }
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            string path = @"E:\Projects\Unity\WolfFPS\Assets\Levels\generated_test.lvl";
            InputOutputUtil.Save(level, path);
            Title = path;

            UpdateUnsavedIndicator(true);
        }

        private void load_Click(object sender, RoutedEventArgs e)
        {
            this.level = InputOutputUtil.Load(@"E:\Projects\Unity\WolfFPS\Assets\Levels\generated_test.lvl");
            ResetViewport();

            UpdateUnsavedIndicator(true);
        }




    }
}

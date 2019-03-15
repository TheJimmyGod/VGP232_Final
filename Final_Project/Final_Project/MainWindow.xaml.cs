using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace Final_Project
{
    /// <summary>
    /// // Student Name : Minjang Jin
    /// // Student Number : 1820109
    /// </summary>
    public partial class MainWindow : Window
    {
        int Map_Number = 0;
        int index = 0;
        int Array_index = 0;
        List<Is_Map> New_Map1 = new List<Is_Map>(100);
        List<Is_Map> New_Map2 = new List<Is_Map>(100);
        List<Is_Map> New_Map3 = new List<Is_Map>(100);

        string Tile_name1;
        string Tile_name2;
        string Tile_name3;
        string Tile_name4;

        int[,] myGrid = new int[100,100];

        // Base string and enums to seek what tile is
        public enum Types
        {
            First, Second, Third, Fourth, Erase
        }

        public enum Tile_Type
        {
            Empty, One, Two, Three, Four
        }
        public Types type { get; private set; }
        public string First_tile { get; set; }
        public string Second_tile { get; set; }
        public string Third_tile { get; set; }
        public string Fourth_tile { get; set; }
        public string ImagePath1 { get; private set; }
        public string ImagePath2 { get; private set; }
        public string ImagePath3 { get; private set; }
        public List<Is_Map> Maps { get; private set; }
        public class Is_Map
        {
            public int Columns { get; set; }
            public int Rows { get; set; }
            public Tile_Type GetTileNumber { get; set; }
            public string TileName { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();
            DisplayGrid(); // Spawn girds on Canvas
            
        }
        // Change type when you select each radio button
        private void First_Select(object sender, RoutedEventArgs e)
        {
            type = Types.First;
        }

        private void Second_Select(object sender, RoutedEventArgs e)
        {
            type = Types.Second;
        }

        private void Third_Select(object sender, RoutedEventArgs e)
        {
            type = Types.Third;
        }

        private void Fourth_Select(object sender, RoutedEventArgs e)
        {
            type = Types.Fourth;
        }
        private void Eraser(object sender, RoutedEventArgs e)
        {
            type = Types.Erase;
        }
        // Generate what you made a custom grid to png file
        private void Generate(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "png files (*.png)|*.png|All files (*.*)|*.*";
            if (saveFile.ShowDialog() == true)
            {
                try
                {
                    string path = saveFile.FileName;
                    
                    if (File.Exists(path)) { File.Delete(path); }
                    Rect rect = new Rect(Map.RenderSize);
                    // RenderTargetBitmap would follow how to convert specific grid to a single image
                    RenderTargetBitmap render = new RenderTargetBitmap((int)rect.Width, (int)rect.Height, 96d, 96d, PixelFormats.Default);
                    for (int i = 0; i < Map.Children.Count; i++)
                    {
                        render.Render(Map.Children[i]);
                    }
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(render));
                    MemoryStream ms = new MemoryStream();
                    encoder.Save(ms);
                    ms.Close();
                    File.WriteAllBytes(saveFile.FileName, ms.ToArray());
                    if(Map_Number == 0)
                    {
                        string FileName = "First_Map.json";
                        var JsonSerializer = JsonConvert.SerializeObject(New_Map1, Formatting.Indented);
                        System.IO.File.WriteAllText(FileName, JsonSerializer);
                        First_Check.IsChecked = true;
                    }
                    if (Map_Number == 1)
                    {
                        string FileName = "Second_Map.json";
                        var JsonSerializer = JsonConvert.SerializeObject(New_Map2, Formatting.Indented);
                        System.IO.File.WriteAllText(FileName, JsonSerializer);
                        Second_Check.IsChecked = true;
                    }
                    Map_Number++;
                    if (Map_Number == 2)
                    {
                        string FileName = "Third_Map.json";
                        var JsonSerializer = JsonConvert.SerializeObject(New_Map3, Formatting.Indented);
                        System.IO.File.WriteAllText(FileName, JsonSerializer);
                        Third_Check.IsChecked = true;
                        Map_Number = 0;
                    }

                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error is appeared: {0}", ex.Message);
                }

            }
        }
        // Import tile set. The size must be 32 X 32
        private void Import_One(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "jpg files (*.jpg)|*.jpg|png files (*.png)|*.png|All files (*.*)|*.*";
            if (openFile.ShowDialog() == true)
            {
                if (System.IO.Path.GetExtension(openFile.FileName) != ".jpg" && System.IO.Path.GetExtension(openFile.FileName) != ".png")
                {
                    MessageBox.Show("Must import jpg or png");
                }
                else
                {
                    // Display what tile have you chosen
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(openFile.FileName);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    Tile1.Source = bitmap;
                    First_tile = openFile.FileName;
                    Tile_name1 = System.IO.Path.GetFileName(openFile.FileName);
                }
            }
        }
        private void Import_Two(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "jpg files (*.jpg)|*.jpg|png files (*.png)|*.png|All files (*.*)|*.*";
            if (openFile.ShowDialog() == true)
            {
                if (System.IO.Path.GetExtension(openFile.FileName) != ".jpg" && System.IO.Path.GetExtension(openFile.FileName) != ".png")
                {
                    MessageBox.Show("Must import jpg or png");
                }
                else
                {
                    // Display what tile have you chosen
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(openFile.FileName);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    Tile2.Source = bitmap;
                    Second_tile = openFile.FileName;
                    Tile_name2 = System.IO.Path.GetFileName(openFile.FileName);
                }
            }
        }
        private void Import_Three(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "jpg files (*.jpg)|*.jpg|png files (*.png)|*.png|All files (*.*)|*.*";
            if (openFile.ShowDialog() == true)
            {
                if (System.IO.Path.GetExtension(openFile.FileName) != ".jpg" && System.IO.Path.GetExtension(openFile.FileName) != ".png")
                {
                    MessageBox.Show("Must import jpg or png");
                }
                else
                {
                    // Display what tile have you chosen
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(openFile.FileName);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    Tile3.Source = bitmap;
                    Third_tile = openFile.FileName;
                    Tile_name3 = System.IO.Path.GetFileName(openFile.FileName);
                }
            }
        }
        private void Import_Four(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "jpg files (*.jpg)|*.jpg|png files (*.png)|*.png|All files (*.*)|*.*";
            if (openFile.ShowDialog() == true)
            {
                if (System.IO.Path.GetExtension(openFile.FileName) != ".jpg" && System.IO.Path.GetExtension(openFile.FileName) != ".png")
                {
                    MessageBox.Show("Must import jpg or png");
                }
                else
                {
                    // Display what tile have you chosen
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(openFile.FileName);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    Tile4.Source = bitmap;
                    Fourth_tile = openFile.FileName;
                    Tile_name4 = System.IO.Path.GetFileName(openFile.FileName);
                }
            }
        }
        // Display grid to work
        private void DisplayGrid()
        {
            int size = 32;
            int x = 0;
            int y = 0;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Is_Map map = new Is_Map();
                    Rectangle rect = new Rectangle();
                    rect.Width = size;
                    rect.Height = size;
                    rect.Fill = Brushes.White;
                    rect.Stroke = Brushes.Black;

                    Canvas.SetTop(rect, x);
                    Canvas.SetLeft(rect, y);
                    Map.Children.Add(rect);
                    myGrid[i, j] = Array_index;
                    y += (size + 1);
                    New_Map1.Add(map);
                    New_Map1[Array_index].GetTileNumber = Tile_Type.Empty;
                    New_Map1[Array_index].Columns = j;
                    New_Map1[Array_index].Rows = i;

                    New_Map2.Add(map);
                    New_Map2[Array_index].GetTileNumber = Tile_Type.Empty;
                    New_Map2[Array_index].Columns = j;
                    New_Map2[Array_index].Rows = i;

                    New_Map3.Add(map);
                    New_Map3[Array_index].GetTileNumber = Tile_Type.Empty;
                    New_Map3[Array_index].Columns = j;
                    New_Map3[Array_index].Rows = i;

                    Array_index++;

                }
                y = 0;
                x += (size + 1);
            }
            Array_index = 0;
        }
        // When you click each grid, the grid would change what you selected tileset.
        private void Click(object sender, MouseButtonEventArgs e)
        {
            Point pt = e.GetPosition((UIElement)sender);
            HitTestResult result = VisualTreeHelper.HitTest(Map, pt);

            if (result != null)
            {
                Rectangle rect = (Rectangle)result.VisualHit;
                for (int i = 0; i < Map.Children.Count; i++)
                {
                    if (Map.Children[i] == rect)
                    {
                        index = i;
                    }
                }
                // Switch statement will help to convert a grid into an image
                switch (type)
                {
                    case Types.First:
                        rect.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(First_tile, UriKind.Absolute)) };
                        if (Map_Number == 0)
                        {
                            New_Map1[index - 1].GetTileNumber = Tile_Type.One;
                            New_Map1[index - 1].TileName = Tile_name1;
                        }
                        if (Map_Number == 1)
                        {
                            New_Map2[index - 1].GetTileNumber = Tile_Type.One;
                            New_Map2[index - 1].TileName = Tile_name1;
                        }
                        if (Map_Number == 2)
                        {
                            New_Map3[index - 1].GetTileNumber = Tile_Type.One;
                            New_Map3[index - 1].TileName = Tile_name1;
                        }
                        break;
                    case Types.Second:
                        rect.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(Second_tile, UriKind.Absolute)) };
                        if (Map_Number == 0)
                        {
                            New_Map1[index - 1].GetTileNumber = Tile_Type.Two;
                            New_Map1[index - 1].TileName = Tile_name2;
                        }
                        if (Map_Number == 1)
                        {
                            New_Map2[index - 1].GetTileNumber = Tile_Type.Two;
                            New_Map2[index - 1].TileName = Tile_name2;
                        }
                        if (Map_Number == 2)
                        {
                            New_Map2[index - 1].GetTileNumber = Tile_Type.Two;
                            New_Map2[index - 1].TileName = Tile_name2;
                        }
                        break;
                    case Types.Third:
                        rect.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(Third_tile, UriKind.Absolute)) };
                        if (Map_Number == 0)
                        {
                            New_Map1[index - 1].GetTileNumber = Tile_Type.Three;
                            New_Map1[index - 1].TileName = Tile_name3;
                        }
                        if (Map_Number == 1)
                        {
                            New_Map2[index - 1].GetTileNumber = Tile_Type.Three;
                            New_Map2[index - 1].TileName = Tile_name3;
                        }
                        if (Map_Number == 2)
                        {
                            New_Map3[index - 1].GetTileNumber = Tile_Type.Three;
                            New_Map3[index - 1].TileName = Tile_name3;
                        }
                        break;
                    case Types.Fourth:
                        rect.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(Fourth_tile, UriKind.Absolute)) };
                        if (Map_Number == 0)
                        {
                            New_Map1[index - 1].GetTileNumber = Tile_Type.Four;
                            New_Map1[index - 1].TileName = Tile_name4;
                        }
                        if (Map_Number == 1)
                        {
                            New_Map2[index - 1].GetTileNumber = Tile_Type.Four;
                            New_Map2[index - 1].TileName = Tile_name4;
                        }
                        if (Map_Number == 2)
                        {
                            New_Map3[index - 1].GetTileNumber = Tile_Type.Four;
                            New_Map3[index - 1].TileName = Tile_name4;
                        }

                        break;
                    case Types.Erase:
                        rect.Fill = Brushes.White; // Basically, brushes white to assign 'Erase'
                        if (Map_Number == 0)
                        {
                            New_Map1[index - 1].GetTileNumber = Tile_Type.Empty;
                            New_Map1[index - 1].TileName = "Null";
                        }
                        if (Map_Number == 1)
                        {
                            New_Map2[index - 1].GetTileNumber = Tile_Type.Empty;
                            New_Map2[index - 1].TileName = "Null";
                        }
                        if (Map_Number == 2)
                        {
                            New_Map3[index - 1].GetTileNumber = Tile_Type.Empty;
                            New_Map3[index - 1].TileName = "Null";
                        }
                        break;
                }
            }
            try
            {
                
            }
            catch (Exception)
            {
                MessageBox.Show("Import tile first");
            }
        }

        // When you fill all list, Combine generator will help to merge three images into a single image
        // Basically, it has been ultilized sprite generator's functions
        private void Combine(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "png files (*.png)|*.png|All files (*.*)|*.*";
            if (saveFile.ShowDialog() == true)
            {
                string[] Files = { ImagePath1, ImagePath2, ImagePath3 };

                int MaxWidth = 0;
                int MaxHeight = 0;
                int Columns = 3;
                foreach (string Image in Files)
                {
                    System.Drawing.Image image = System.Drawing.Image.FromFile(Image);
                    MaxWidth = Math.Max(MaxWidth, image.Width);
                    MaxHeight = Math.Max(MaxHeight, image.Height);
                    image.Dispose();
                }

                int Width = Columns * MaxWidth;
                int Height = MaxHeight;

                System.Drawing.Bitmap Sheet = new System.Drawing.Bitmap(Width, Height);

                using (System.Drawing.Graphics GFX = System.Drawing.Graphics.FromImage(Sheet))
                {
                    int Col = 0;
                    int Row = 0;
                    foreach (string F in Files)
                    {
                        System.Drawing.Image img = System.Drawing.Image.FromFile(F);
                        int x = (Col * MaxWidth) + (MaxWidth / 2 - img.Width / 2);
                        int y = (Row * MaxHeight) + (MaxHeight / 2 - img.Height / 2);

                        System.Drawing.Rectangle SrcRect = new System.Drawing.Rectangle(0, 0, img.Width, img.Height);
                        System.Drawing.Rectangle DestRect = new System.Drawing.Rectangle(x, y, img.Width, img.Height);
                        GFX.DrawImage(img, DestRect, SrcRect, System.Drawing.GraphicsUnit.Pixel);
                        img.Dispose();

                        Col++;
                        if (Col == Columns)
                        {
                            Col = 0;
                            Row++;
                        }
                    }
                }
                Sheet.Save(saveFile.FileName); // The image will save new file into output path
                Combine_Button.IsEnabled = false;
                ImagePath1 = null;
                ImagePath2 = null;
                ImagePath3 = null;
                Files = null;
                First_Check.IsChecked = false;
                Second_Check.IsChecked = false;
                Third_Check.IsChecked = false;
            }
        }
    }
}

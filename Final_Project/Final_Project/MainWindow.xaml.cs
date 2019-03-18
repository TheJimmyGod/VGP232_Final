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
        // Basic constructs
        int Map_Number = 0;
        int index = 0;
        int Array_index = 0;
        Tile_info Info = new Tile_info();
        List<Is_Map> New_Map1 = new List<Is_Map>(100);
        List<Is_Map> New_Map2 = new List<Is_Map>(100);
        List<Is_Map> New_Map3 = new List<Is_Map>(100);
        List<Is_Map> Load_Map = new List<Is_Map>();

        string Tile_name1;
        string Tile_name2;
        string Tile_name3;
        string Tile_name4;

        int[,] myGrid = new int[100,100];
        int[,] newGrid = new int[100, 300];

        // Base string and enums to seek what tile is
        public Types type { get; private set; }
        public Direction direction { get; private set; }

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
        // Change direction type when you select each radio button
        private void Horizontal(object sender, RoutedEventArgs e)
        {
            direction = Direction.Horizontal;
        }

        private void Vertical(object sender, RoutedEventArgs e)
        {
            direction = Direction.Vertical;
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
                        if (File.Exists(FileName))
                        {
                            File.Delete(FileName);
                        }
                        var JsonSerializer = JsonConvert.SerializeObject(New_Map1, Formatting.Indented);
                        System.IO.File.WriteAllText(FileName, JsonSerializer);
                        Info.ImagePath1 = path;
                        First_Check.IsChecked = true;
                        foreach (var c in Map.Children.OfType<Rectangle>())
                        {
                            c.Fill = Brushes.White;
                        }
                        if (First_Check.IsChecked == true && Second_Check.IsChecked == true && Third_Check.IsChecked == true)
                        {
                            Combine_Button.IsEnabled = true;
                        }
                        this.Box.SelectedItem = Second_List;
                    }
                    else if (Map_Number == 1)
                    {
                        string FileName = "Second_Map.json";
                        if (File.Exists(FileName))
                        {
                            File.Delete(FileName);
                        }
                        var JsonSerializer = JsonConvert.SerializeObject(New_Map2, Formatting.Indented);
                        System.IO.File.WriteAllText(FileName, JsonSerializer);
                        Info.ImagePath2 = path;
                        Second_Check.IsChecked = true;
                        foreach (var c in Map.Children.OfType<Rectangle>())
                        {
                            c.Fill = Brushes.White;
                        }
                        if (First_Check.IsChecked == true && Second_Check.IsChecked == true && Third_Check.IsChecked == true)
                        {
                            Combine_Button.IsEnabled = true;
                        }
                        this.Box.SelectedItem = Third_List;
                    }
                    else if (Map_Number == 2)
                    {
                        string FileName = "Third_Map.json";
                        if (File.Exists(FileName))
                        {
                            File.Delete(FileName);
                        }
                        var JsonSerializer = JsonConvert.SerializeObject(New_Map3, Formatting.Indented);
                        System.IO.File.WriteAllText(FileName, JsonSerializer);
                        Info.ImagePath3 = path;
                        Third_Check.IsChecked = true;
                        foreach (var c in Map.Children.OfType<Rectangle>())
                        {
                            c.Fill = Brushes.White;
                        }
                        if (First_Check.IsChecked == true && Second_Check.IsChecked == true && Third_Check.IsChecked == true)
                        {
                            Combine_Button.IsEnabled = true;
                        }
                        this.Box.SelectedItem = First_Check;
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
                    Info.First_tile = openFile.FileName;
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
                    Info.Second_tile = openFile.FileName;
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
                    Info.Third_tile = openFile.FileName;
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
                    Info.Fourth_tile = openFile.FileName;
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
                    Is_Map map2 = new Is_Map();
                    Is_Map map3 = new Is_Map();
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
                    New_Map1[Array_index].Columns = j + 1;
                    New_Map1[Array_index].Rows = i + 1;

                    New_Map2.Add(map2);
                    New_Map2[Array_index].GetTileNumber = Tile_Type.Empty;
                    New_Map2[Array_index].Columns = j + 1;
                    New_Map2[Array_index].Rows = i + 1;

                    New_Map3.Add(map3);
                    New_Map3[Array_index].GetTileNumber = Tile_Type.Empty;
                    New_Map3[Array_index].Columns = j + 1;
                    New_Map3[Array_index].Rows = i + 1;

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
            try
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
                            rect.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(Info.First_tile, UriKind.Absolute)) };
                            if (Map_Number == 0)
                            {
                                New_Map1[index].GetTileNumber = Tile_Type.One;
                                New_Map1[index].TileName = Tile_name1;
                            }
                            if (Map_Number == 1)
                            {
                                New_Map2[index].GetTileNumber = Tile_Type.One;
                                New_Map2[index].TileName = Tile_name1;
                            }
                            if (Map_Number == 2)
                            {
                                New_Map3[index].GetTileNumber = Tile_Type.One;
                                New_Map3[index].TileName = Tile_name1;
                            }
                            break;
                        case Types.Second:
                            rect.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(Info.Second_tile, UriKind.Absolute)) };
                            if (Map_Number == 0)
                            {
                                New_Map1[index].GetTileNumber = Tile_Type.Two;
                                New_Map1[index].TileName = Tile_name2;
                            }
                            if (Map_Number == 1)
                            {
                                New_Map2[index].GetTileNumber = Tile_Type.Two;
                                New_Map2[index].TileName = Tile_name2;
                            }
                            if (Map_Number == 2)
                            {
                                New_Map3[index].GetTileNumber = Tile_Type.Two;
                                New_Map3[index].TileName = Tile_name2;
                            }
                            break;
                        case Types.Third:
                            rect.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(Info.Third_tile, UriKind.Absolute)) };
                            if (Map_Number == 0)
                            {
                                New_Map1[index].GetTileNumber = Tile_Type.Three;
                                New_Map1[index].TileName = Tile_name3;
                            }
                            if (Map_Number == 1)
                            {
                                New_Map2[index].GetTileNumber = Tile_Type.Three;
                                New_Map2[index].TileName = Tile_name3;
                            }
                            if (Map_Number == 2)
                            {
                                New_Map3[index].GetTileNumber = Tile_Type.Three;
                                New_Map3[index].TileName = Tile_name3;
                            }
                            break;
                        case Types.Fourth:
                            rect.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(Info.Fourth_tile, UriKind.Absolute)) };
                            if (Map_Number == 0)
                            {
                                New_Map1[index].GetTileNumber = Tile_Type.Four;
                                New_Map1[index].TileName = Tile_name4;
                            }
                            if (Map_Number == 1)
                            {
                                New_Map2[index].GetTileNumber = Tile_Type.Four;
                                New_Map2[index].TileName = Tile_name4;
                            }
                            if (Map_Number == 2)
                            {
                                New_Map3[index].GetTileNumber = Tile_Type.Four;
                                New_Map3[index].TileName = Tile_name4;
                            }

                            break;
                        case Types.Erase:
                            rect.Fill = Brushes.White; // Basically, brushes white to assign 'Erase'
                            if (Map_Number == 0)
                            {
                                New_Map1[index].GetTileNumber = Tile_Type.Empty;
                                New_Map1[index].TileName = "Null";
                            }
                            if (Map_Number == 1)
                            {
                                New_Map2[index].GetTileNumber = Tile_Type.Empty;
                                New_Map2[index].TileName = "Null";
                            }
                            if (Map_Number == 2)
                            {
                                New_Map3[index].GetTileNumber = Tile_Type.Empty;
                                New_Map3[index].TileName = "Null";
                            }
                            break;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Import tile first");
            }
        }
        // Basically, it has been ultilized sprite generator's functions
        // When you fill all list, Combine generator will help to merge three images into a single image
        // Then the app would save a map to Json file
        private void Combine(object sender, RoutedEventArgs e)
        {
            List<Is_Map> All_Map = new List<Is_Map>(300);
            int Array_index2 = 0;
            int Array_index_First = 0;
            int Array_index_Second = 0;
            int Array_index_Third = 0;

            if (direction == Direction.Horizontal)
            {
                for (int r = 0; r < 10; r++)
                {
                    for (int c = 0; c < 30; c++)
                    {

                        Is_Map is_Map = new Is_Map();
                        All_Map.Add(is_Map);
                        newGrid[r, c] = Array_index2;
                        All_Map[Array_index2].Columns = c + 1;
                        All_Map[Array_index2].Rows = r + 1;
                        if (c >= 0 && c < 10)
                        {
                            All_Map[Array_index2].GetTileNumber = New_Map1[Array_index_First].GetTileNumber;
                            All_Map[Array_index2].TileName = New_Map1[Array_index_First].TileName;
                            if (Array_index_First == 99)
                            {
                                Array_index_First = 0;
                            }
                            else
                            {
                                Array_index_First++;
                            }
                        }
                        else if (c >= 10 && c < 20)
                        {
                            All_Map[Array_index2].GetTileNumber = New_Map2[Array_index_Second].GetTileNumber;
                            All_Map[Array_index2].TileName = New_Map2[Array_index_Second].TileName;

                            if (Array_index_Second == 99)
                            {
                                Array_index_Second = 0;
                            }
                            else
                            {
                                Array_index_Second++;
                            }
                        }
                        else
                        {
                            All_Map[Array_index2].GetTileNumber = New_Map3[Array_index_Third].GetTileNumber;
                            All_Map[Array_index2].TileName = New_Map3[Array_index_Third].TileName;
                            if (Array_index_Third == 99)
                            {
                                Array_index_Third = 0;
                            }
                            else
                            {
                                Array_index_Third++;
                            }
                        }

                        Array_index2++;
                    }
                }

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.Filter = "png files (*.png)|*.png|All files (*.*)|*.*";
                if (saveFile.ShowDialog() == true)
                {
                    string[] Files = { Info.ImagePath1, Info.ImagePath2, Info.ImagePath3 };
                    if(Info.ImagePath1 == string.Empty && Info.ImagePath2 == string.Empty && Info.ImagePath3 == string.Empty)
                    {
                        MessageBox.Show("An image is empty, since Images have been conflicted. Should choose different name of the file, not same");
                    }
                    else
                    {
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
                        string CompleteName = "Map.json";
                        if (File.Exists(CompleteName))
                        {
                            File.Delete(CompleteName);
                        }
                        var JsonSerilzation = JsonConvert.SerializeObject(All_Map, Formatting.Indented);
                        System.IO.File.WriteAllText(CompleteName, JsonSerilzation);
                        All_Map = null;
                        this.Box.SelectedItem = First_List;
                    }
                }
            }
            else if (direction == Direction.Vertical)
            {
                for (int r = 0; r < 30; r++)
                {
                    for (int c = 0; c < 10; c++)
                    {

                        Is_Map is_Map = new Is_Map();
                        All_Map.Add(is_Map);
                        newGrid[r, c] = Array_index2;
                        All_Map[Array_index2].Columns = c + 1;
                        All_Map[Array_index2].Rows = r + 1;
                        if (r >= 0 && r < 10)
                        {
                            All_Map[Array_index2].GetTileNumber = New_Map1[Array_index_First].GetTileNumber;
                            All_Map[Array_index2].TileName = New_Map1[Array_index_First].TileName;
                            if (Array_index_First == 99)
                            {
                                Array_index_First = 0;
                            }
                            else
                            {
                                Array_index_First++;
                            }
                        }
                        else if (r >= 10 && r < 20)
                        {
                            All_Map[Array_index2].GetTileNumber = New_Map2[Array_index_Second].GetTileNumber;
                            All_Map[Array_index2].TileName = New_Map2[Array_index_Second].TileName;

                            if (Array_index_Second == 99)
                            {
                                Array_index_Second = 0;
                            }
                            else
                            {
                                Array_index_Second++;
                            }
                        }
                        else
                        {
                            All_Map[Array_index2].GetTileNumber = New_Map3[Array_index_Third].GetTileNumber;
                            All_Map[Array_index2].TileName = New_Map3[Array_index_Third].TileName;
                            if (Array_index_Third == 99)
                            {
                                Array_index_Third = 0;
                            }
                            else
                            {
                                Array_index_Third++;
                            }
                        }

                        Array_index2++;
                    }
                }

                SaveFileDialog saveFile2 = new SaveFileDialog();
                saveFile2.Filter = "png files (*.png)|*.png|All files (*.*)|*.*";
                if (saveFile2.ShowDialog() == true)
                {
                    string[] Files = { Info.ImagePath1, Info.ImagePath2, Info.ImagePath3 };
                    if (Info.ImagePath1 == string.Empty && Info.ImagePath2 == string.Empty && Info.ImagePath3 == string.Empty)
                    {
                        MessageBox.Show("An image is empty, since Images have been conflicted. Should choose different name of the file, not same");
                    }
                    else
                    {
                        int MaxWidth = 0;
                        int MaxHeight = 0;
                        int Columns = 1;
                        int Rows = 3;
                        foreach (string Image in Files)
                        {
                            System.Drawing.Image image = System.Drawing.Image.FromFile(Image);
                            MaxWidth = Math.Max(MaxWidth, image.Width);
                            MaxHeight = Math.Max(MaxHeight, image.Height);
                            image.Dispose();
                        }

                        int Width = MaxWidth;
                        int Height = Rows * MaxHeight;

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
                        Sheet.Save(saveFile2.FileName); // The image will save new file into output path
                        string CompleteName = "Map.json";
                        if (File.Exists(CompleteName))
                        {
                            File.Delete(CompleteName);
                        }
                        var JsonSerilzation = JsonConvert.SerializeObject(All_Map, Formatting.Indented);
                        System.IO.File.WriteAllText(CompleteName, JsonSerilzation);
                        All_Map = null;
                        this.Box.SelectedItem = First_List;
                    }
                }
            }
        }
        // Load a map by a Json file
        private void Load_JSON(object sender, RoutedEventArgs e)
        {
            if((Info.First_tile == null) || (Info.Second_tile == null) || (Info.Third_tile == null) || (Info.Fourth_tile == null))
            {
                MessageBox.Show("Should import all tiles first");
            }
            else
            {
                try
                {
                    int U = 0;
                    OpenFileDialog dialog = new OpenFileDialog();
                    dialog.Filter = "JSON format |*.json";
                    if (dialog.ShowDialog() == true)
                    {
                        string Json = System.IO.File.ReadAllText(dialog.FileName);
                        Load_Map = JsonConvert.DeserializeObject<List<Is_Map>>(Json);
                        if (Map_Number == 0)
                        {
                            for (int i = 0; i < New_Map1.Count; i++)
                            {
                                New_Map1[i].Columns = Load_Map[i].Columns;
                                New_Map1[i].Rows = Load_Map[i].Rows;
                                New_Map1[i].GetTileNumber = Load_Map[i].GetTileNumber;
                                New_Map1[i].TileName = Load_Map[i].TileName;
                            }
                            foreach (var c in Map.Children.OfType<Rectangle>())
                            {
                                switch (New_Map1[U].GetTileNumber)
                                {
                                    case Tile_Type.Empty:
                                        c.Fill = Brushes.White;
                                        break;
                                    case Tile_Type.One:
                                        c.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(Info.First_tile, UriKind.Absolute)) };
                                        break;
                                    case Tile_Type.Two:
                                        c.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(Info.Second_tile, UriKind.Absolute)) };
                                        break;
                                    case Tile_Type.Three:
                                        c.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(Info.Third_tile, UriKind.Absolute)) };
                                        break;
                                    case Tile_Type.Four:
                                        c.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(Info.Fourth_tile, UriKind.Absolute)) };
                                        break;
                                }
                                U++;
                            }

                        }
                        else if (Map_Number == 1)
                        {
                            for (int i = 0; i < New_Map2.Count; i++)
                            {
                                New_Map2[i].Columns = Load_Map[i].Columns;
                                New_Map2[i].Rows = Load_Map[i].Rows;
                                New_Map2[i].GetTileNumber = Load_Map[i].GetTileNumber;
                                New_Map2[i].TileName = Load_Map[i].TileName;
                            }
                            foreach (var c in Map.Children.OfType<Rectangle>())
                            {
                                switch (New_Map2[U].GetTileNumber)
                                {
                                    case Tile_Type.Empty:
                                        c.Fill = Brushes.White;
                                        break;
                                    case Tile_Type.One:
                                        c.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(Info.First_tile, UriKind.Absolute)) };
                                        break;
                                    case Tile_Type.Two:
                                        c.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(Info.Second_tile, UriKind.Absolute)) };
                                        break;
                                    case Tile_Type.Three:
                                        c.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(Info.Third_tile, UriKind.Absolute)) };
                                        break;
                                    case Tile_Type.Four:
                                        c.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(Info.Fourth_tile, UriKind.Absolute)) };
                                        break;
                                }
                                U++;
                            }
                        }
                        else if (Map_Number == 2)
                        {
                            for (int i = 0; i < New_Map3.Count; i++)
                            {
                                New_Map3[i].Columns = Load_Map[i].Columns;
                                New_Map3[i].Rows = Load_Map[i].Rows;
                                New_Map3[i].GetTileNumber = Load_Map[i].GetTileNumber;
                                New_Map3[i].TileName = Load_Map[i].TileName;
                            }
                            foreach (var c in Map.Children.OfType<Rectangle>())
                            {
                                switch (New_Map3[U].GetTileNumber)
                                {
                                    case Tile_Type.Empty:
                                        c.Fill = Brushes.White;
                                        break;
                                    case Tile_Type.One:
                                        c.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(Info.First_tile, UriKind.Absolute)) };
                                        break;
                                    case Tile_Type.Two:
                                        c.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(Info.Second_tile, UriKind.Absolute)) };
                                        break;
                                    case Tile_Type.Three:
                                        c.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(Info.Third_tile, UriKind.Absolute)) };
                                        break;
                                    case Tile_Type.Four:
                                        c.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(Info.Fourth_tile, UriKind.Absolute)) };
                                        break;
                                }
                                U++;
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error is appeared: {0}", ex.Message);
                }
            }
        }

        private void Display_Map(object sender, RoutedEventArgs e)
        {
            OpenFileDialog source = new OpenFileDialog();
            source.Filter = "png files (*.png)|*.png|All files (*.*)|*.*";
            if (source.ShowDialog() == true)
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(source.FileName);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                if(bitmap.Height > 700)
                {
                    Screenshot2 screenshot2 = new Screenshot2();
                    screenshot2.Image_Background_Vertical.Source = bitmap;
                    screenshot2.ShowDialog();
                }
                else if (bitmap.Width > 700)
                {
                    Screenshot3 screenshot3 = new Screenshot3();
                    screenshot3.Image_Background_Horizontal.Source = bitmap;
                    screenshot3.ShowDialog();
                }
                else
                {
                    Screenshot screenshot = new Screenshot();
                    screenshot.Image_Background.Source = bitmap;
                    screenshot.ShowDialog();
                }
            }
        }

        private void First_Map(object sender, RoutedEventArgs e)
        {
            int Index_tile = 0;
            Map_Number = 0;
            foreach (var t in Map.Children.OfType<Rectangle>())
            {
                switch (New_Map1[Index_tile].GetTileNumber)
                {
                    case Tile_Type.Empty:
                        t.Fill = Brushes.White;
                        break;
                    case Tile_Type.One:
                        t.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(Info.First_tile, UriKind.Absolute)) };
                        break;
                    case Tile_Type.Two:
                        t.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(Info.Second_tile, UriKind.Absolute)) };
                        break;
                    case Tile_Type.Three:
                        t.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(Info.Third_tile, UriKind.Absolute)) };
                        break;
                    case Tile_Type.Four:
                        t.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(Info.Fourth_tile, UriKind.Absolute)) };
                        break;
                }
                Index_tile++;
            }
        }
        private void Second_Map(object sender, RoutedEventArgs e)
        {
            int Index_tile = 0;
            Map_Number = 1;
            foreach (var t in Map.Children.OfType<Rectangle>())
            {
                switch (New_Map2[Index_tile].GetTileNumber)
                {
                    case Tile_Type.Empty:
                        t.Fill = Brushes.White;
                        break;
                    case Tile_Type.One:
                        t.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(Info.First_tile, UriKind.Absolute)) };
                        break;
                    case Tile_Type.Two:
                        t.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(Info.Second_tile, UriKind.Absolute)) };
                        break;
                    case Tile_Type.Three:
                        t.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(Info.Third_tile, UriKind.Absolute)) };
                        break;
                    case Tile_Type.Four:
                        t.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(Info.Fourth_tile, UriKind.Absolute)) };
                        break;
                }
                Index_tile++;
            }
        }
        private void Third_Map(object sender, RoutedEventArgs e)
        {
            int Index_tile = 0;
            Map_Number = 2;
            foreach (var t in Map.Children.OfType<Rectangle>())
            {
                switch (New_Map3[Index_tile].GetTileNumber)
                {
                    case Tile_Type.Empty:
                        t.Fill = Brushes.White;
                        break;
                    case Tile_Type.One:
                        t.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(Info.First_tile, UriKind.Absolute)) };
                        break;
                    case Tile_Type.Two:
                        t.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(Info.Second_tile, UriKind.Absolute)) };
                        break;
                    case Tile_Type.Three:
                        t.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(Info.Third_tile, UriKind.Absolute)) };
                        break;
                    case Tile_Type.Four:
                        t.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(Info.Fourth_tile, UriKind.Absolute)) };
                        break;
                }
                Index_tile++;
            }
        }

        private void Reset_Map(object sender, RoutedEventArgs e)
        {
            Array_index = 0;
            if(Map_Number == 0)
            {
                New_Map1.Clear();
                First_Check.IsChecked = false;
                Combine_Button.IsEnabled = false;

                foreach (var t in Map.Children.OfType<Rectangle>())
                {
                    t.Fill = Brushes.White;
                }

                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        Is_Map map1 = new Is_Map();
                        New_Map1.Add(map1);
                        New_Map1[Array_index].GetTileNumber = Tile_Type.Empty;
                        New_Map1[Array_index].Columns = j + 1;
                        New_Map1[Array_index].Rows = i + 1;
                    }
                }
                Array_index = 0;
            }
            else if(Map_Number == 1)
            {
                New_Map2.Clear();
                Second_Check.IsChecked = false;
                Combine_Button.IsEnabled = false;
                foreach (var t in Map.Children.OfType<Rectangle>())
                {
                    t.Fill = Brushes.White;
                }

                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        Is_Map map2 = new Is_Map();
                        New_Map2.Add(map2);
                        New_Map2[Array_index].GetTileNumber = Tile_Type.Empty;
                        New_Map2[Array_index].Columns = j + 1;
                        New_Map2[Array_index].Rows = i + 1;
                    }
                }
                Array_index = 0;
            }
            else if (Map_Number == 2)
            {
                New_Map3.Clear();
                Third_Check.IsChecked = false;
                Combine_Button.IsEnabled = false;
                foreach (var t in Map.Children.OfType<Rectangle>())
                {
                    t.Fill = Brushes.White;
                }

                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        Is_Map map3 = new Is_Map();
                        New_Map3.Add(map3);
                        New_Map3[Array_index].GetTileNumber = Tile_Type.Empty;
                        New_Map3[Array_index].Columns = j + 1;
                        New_Map3[Array_index].Rows = i + 1;
                    }
                }
                Array_index = 0;
            }
        }
    }
}

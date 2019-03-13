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

namespace Final_Project
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public enum Types
        {
            First, Second, Third, Fourth, Erase
        }
        public Types type { get; private set; }
        public string First_tile { get; private set; }
        public string Second_tile { get; private set; }
        public string Third_tile { get; private set; }
        public string Fourth_tile { get; private set; }
        public string ImagePath1 { get; private set; }
        public string ImagePath2 { get; private set; }
        public string ImagePath3 { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            DisplayGrid();
        }

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
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error is appeared: {0}", ex.Message);
                }

            }
        }

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
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(openFile.FileName);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    Tile1.Source = bitmap;
                    First_tile = openFile.FileName;
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
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(openFile.FileName);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    Tile2.Source = bitmap;
                    Second_tile = openFile.FileName;
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
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(openFile.FileName);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    Tile3.Source = bitmap;
                    Third_tile = openFile.FileName;
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
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(openFile.FileName);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    Tile4.Source = bitmap;
                    Fourth_tile = openFile.FileName;
                }
            }
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            if(Map_List.Items.Count <= 2)
            {
                Map_List.Items.Add(Map_name.Text);
            }
            else
            {
                MessageBox.Show("Can't add more than 3");
            }
        }

        private void Remove(object sender, RoutedEventArgs e)
        {
            if(!Map_List.Items.IsEmpty)
            {
                if (this.Map_List.SelectedIndex == 0)
                {
                    ImagePath1 = string.Empty;
                }
                if (this.Map_List.SelectedIndex == 1)
                {
                    ImagePath2 = string.Empty;
                }
                if (this.Map_List.SelectedIndex == 2)
                {
                    ImagePath3 = string.Empty;
                }
                Map_List.Items.RemoveAt(Map_List.Items.IndexOf(Map_List.SelectedItem));
            }
            else
            {
                MessageBox.Show("The list is empty");
            }
        }

        private void DisplayGrid()
        {
            int size = 32;
            int x = 0;
            int y = 0;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Rectangle rect = new Rectangle();
                    rect.Width = size;
                    rect.Height = size;
                    rect.Fill = Brushes.White;

                    Canvas.SetTop(rect, x);
                    Canvas.SetLeft(rect, y);
                    Map.Children.Add(rect);
                    y += (size + 1);
                }
                y = 0;
                x += (size + 1);
            }
        }

        private void Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Point pt = e.GetPosition((UIElement)sender);
                HitTestResult result = VisualTreeHelper.HitTest(Map, pt);
                if (result != null)
                {
                    Rectangle rect = (Rectangle)result.VisualHit;
                    switch (type)
                    {
                        case Types.First:
                            rect.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(First_tile, UriKind.Absolute)) };
                            break;
                        case Types.Second:
                            rect.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(Second_tile, UriKind.Absolute)) };
                            break;
                        case Types.Third:
                            rect.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(Third_tile, UriKind.Absolute)) };
                            break;
                        case Types.Fourth:
                            rect.Fill = new ImageBrush { ImageSource = new BitmapImage(new Uri(Fourth_tile, UriKind.Absolute)) };
                            break;
                        case Types.Erase:
                            rect.Fill = Brushes.White;
                            break;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Import tile first");
            }
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "png files (*.png)|*.png|All files (*.*)|*.*";
            if(openFile.ShowDialog() == true)
            {
                if (this.Map_List.SelectedIndex == 0)
                {
                    ImagePath1 = openFile.FileName;
                }
                if (this.Map_List.SelectedIndex == 1)
                {
                    ImagePath2 = openFile.FileName;
                }
                if (this.Map_List.SelectedIndex == 2)
                {
                    ImagePath3 = openFile.FileName;
                }
            }
            
        }

        private void Combine(object sender, RoutedEventArgs e)
        {
            if(ImagePath1 == string.Empty && ImagePath2 == string.Empty)
            {
                MessageBox.Show("At least needs more than two items");
            }
            else
            {
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.Filter = "png files (*.png)|*.png|All files (*.*)|*.*";
                if (saveFile.ShowDialog() == true)
                {
                    string[] Files = { ImagePath1, ImagePath2, ImagePath3 };

                    if (ImagePath3 == string.Empty)
                    {
                        MessageBox.Show("Third file is empty");
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
                                int x = (Col * MaxWidth) + (MaxWidth /2 - img.Width / 2);
                                int y = (Row * MaxHeight) + (MaxHeight /2 - img.Height / 2);

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
                    }
                }
            }
        }
    }
}

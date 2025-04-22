using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;


namespace WpfOCR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Border_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (files != null && files.Length > 0)
                {
                    try
                    {
                        // טעינת התמונה
                        var bitmap = new BitmapImage(new Uri(files[0]));
                        my_img.Source = bitmap;
                        txt_url.Text = System.IO.Path.GetFileNameWithoutExtension(files[0]);
                        Proccess_Image(files[0]);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"שגיאה בטעינת התמונה: {ex.Message}", "שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void Border_DragOver(object sender, DragEventArgs e)
        {
            //if (e.Data.GetDataPresent(DataFormats.FileDrop))
            //{
            //    e.Effects = DragDropEffects.Copy;
            //}
            //else
            //{
            //    e.Effects = DragDropEffects.None;
            //}
        }

        private void Proccess_Image(string image_path)
        {
            CmdServices.Run($"""tesseract "{image_path}" stdout -l eng+heb""",out string output,out string error);

            txt_result.Text = output;

        }
    }
}
using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WpfScreenHelper;

namespace ScreenSplash
{
    /// <summary>
    /// Interaction logic for ScreenSaver.xaml
    /// </summary>
    public partial class ScreenSaver : Window
    {
        DispatcherTimer _imageTimer = new DispatcherTimer();

        public ScreenSaver()
        {
            InitializeComponent();

            SetImage();

            _imageTimer.Tick += ImageTimer_Tick;
            _imageTimer.Interval = new TimeSpan(0,0,10);
            _imageTimer.Start();
        }

        private void SetImage()
        {
            var screen = Screen.PrimaryScreen;
            var uri = new Uri("https://source.unsplash.com/" + screen.Bounds.Width + "x" + screen.Bounds.Height + "?sig=" + DateTime.Now.Ticks);
            img.Source = new BitmapImage(uri);
        }

        private void ImageTimer_Tick(object sender, EventArgs e)
        {
            SetImage();
        }
    }
}

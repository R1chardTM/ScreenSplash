using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WpfScreenHelper;

namespace ScreenSplash
{
    public partial class ScreenSaver : Window
    {
        private DispatcherTimer _imageTimer = new DispatcherTimer();
        private Point? _lastMove;

        public ScreenSaver()
        {
            InitializeComponent();

            SetImage();

            _imageTimer.Tick += ImageTimer_Tick;
            _imageTimer.Interval = new TimeSpan(0,0,10);
            _imageTimer.Start();

            Cursor = Cursors.None;
        }

        private void SetImage()
        {
            var screen = Screen.PrimaryScreen;
            var uri = new Uri("https://source.unsplash.com/" + screen.Bounds.Width + "x" + screen.Bounds.Height + "?sig=" + DateTime.Now.Ticks, UriKind.Absolute);

            var webClient = new WebClient();
            var imageBytes = webClient.DownloadData(uri);

            var newImage = new BitmapImage();
            using (var stream = new MemoryStream(imageBytes))
            {
                newImage.BeginInit();
                newImage.CacheOption = BitmapCacheOption.OnLoad;
                newImage.StreamSource = stream;
                newImage.EndInit();
            }
            newImage.Freeze();

            img.Source = newImage;
        }

        private void ImageTimer_Tick(object sender, EventArgs e)
        {
            SetImage();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            Application.Current.Shutdown();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            Application.Current.Shutdown();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Point currentMove = e.GetPosition(null);

            //Ignore mouse move event triggered at application start
            if (_lastMove == null)
            {
                _lastMove = currentMove;
            }
            else if (_lastMove != currentMove)
            {
                Application.Current.Shutdown();
            }
        }
    }
}

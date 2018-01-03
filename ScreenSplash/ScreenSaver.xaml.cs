using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WpfScreenHelper;

namespace ScreenSplash
{
    public partial class ScreenSaver : Window
    {
        private DispatcherTimer _imageTimer = new DispatcherTimer();
        private Point? _lastMove;
        private BitmapImage _newImage;
        private List<Photo> _photos;
        private int _currentPhotosIndex = 0;

        public ScreenSaver()
        {
            InitializeComponent();

            var screen = Screen.PrimaryScreen;
            var api = new UnsplashApi(screen.Bounds.Width, screen.Bounds.Height);
            _photos = api.RandomPhotos();

            //Set first image
            PreLoadImage();

            //Start timer to refresh image at an interval
            _imageTimer.Tick += ImageTimer_Tick;
            _imageTimer.Interval = new TimeSpan(0,0,10);
            _imageTimer.Start();

            //Do not show cursor
            Cursor = Cursors.None;
        }

        //Preload image and handle fade out
        private void PreLoadImage()
        {
            //Preload new image
            var url = _photos[_currentPhotosIndex].Urls.Custom;
            _currentPhotosIndex = _photos.Count == _currentPhotosIndex + 1 ? 0 : _currentPhotosIndex + 1;

            var webClient = new WebClient();
            var imageBytes = webClient.DownloadData(url);

            var newImage = new BitmapImage();
            using (var stream = new MemoryStream(imageBytes))
            {
                newImage.BeginInit();
                newImage.CacheOption = BitmapCacheOption.OnLoad;
                newImage.StreamSource = stream;
                newImage.EndInit();
            }
            newImage.Freeze();
            _newImage = newImage;

            //Fade out screen saver
            Storyboard fadeOut = (Storyboard)(FindResource("FadeOut"));
            fadeOut.Begin();

            //After fade out is done set new image
            fadeOut.Completed += new EventHandler(SetImage);
        }

        //Set preloaded image and handle fade in
        private void SetImage(object sender, EventArgs e)
        {
            //Set preloaded image to screen saver
            img.Source = _newImage;

            //Fade in screen saver
            Storyboard fadeIn = (Storyboard)(FindResource("FadeIn"));
            fadeIn.Begin();
        }

        //Refresh image at an interval
        private void ImageTimer_Tick(object sender, EventArgs e)
        {
            PreLoadImage();
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

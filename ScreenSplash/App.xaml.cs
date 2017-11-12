using System.Windows;

namespace ScreenSplash
{
    public partial class App : Application
    {
        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            //Argument '/s' or no arguments starts screen saver
            if (e.Args.Length == 0 || e.Args[0].ToLower().StartsWith("/s"))
            {
                var screenSaver = new ScreenSaver();
                screenSaver.Show();
            }
        }
    }
}

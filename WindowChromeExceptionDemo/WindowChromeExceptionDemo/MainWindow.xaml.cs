using System.Windows;
using Microsoft.Windows.Shell;

namespace WindowChromeExceptionDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var windowChrome = new WindowChrome {CaptionHeight = 2};
            windowChrome.CaptionHeight = 1;
        }
    }
}

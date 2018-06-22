using System.Windows;
using YoutubePlayer.ViewModel;

namespace YoutubePlayer.View
{
    /// <summary>
    /// PlayerView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PlayerView : Window
    {
        public PlayerViewModel ViewModel
        {
            get
            {
                return DataContext as PlayerViewModel;
            }
        }

        public PlayerView()
        {
            InitializeComponent();
            Closing += ViewModel.OnClosing;
            Loaded += PlayerView_Loaded;
        }

        private void PlayerView_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.BrowserHandle = (int)PlayBrowser.Handle;
        }
    }
}

using System.Windows;
using YoutubePlayer.ViewModel;

namespace YoutubePlayer.View
{
    /// <summary>
    /// SearchView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SearchView : Window
    {
        public SearchViewModel ViewModel
        {
            get
            {
                return DataContext as SearchViewModel;
            }
        }

        public SearchView()
        {
            InitializeComponent();

            ViewModel.RequestClose += (loginResult) =>
            {
                Close();
            };
        }
    }
}

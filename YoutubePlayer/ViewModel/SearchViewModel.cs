using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using YoutubePlayer.Common;
using YoutubePlayer.Model;

namespace YoutubePlayer.ViewModel
{
    public enum SearchViewParam
    {
        Search,
        Ok,
        Cancel
    }

    public class SearchViewModel : BaseProperty
    {
        #region Private Variables
        private readonly string _projectPath = Directory.GetParent(Directory.GetParent(
            Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)).ToString()).ToString();

        private readonly string _baseYoutubeUrl = "http://youtube.com/watch?v=";

        private readonly string _apiKeyFile = "ApiKey.txt";

        private string _googleApiKey = "";

        private string _searchQuery;

        private ObservableCollection<SearchModel> _searchList;

        private SearchModel _selectedSearch;

        private ICommand _buttonClicks;
        #endregion

        #region Public Properties
        public string SearchQuery
        {
            get
            {
                return _searchQuery;
            }
            set
            {
                _searchQuery = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<SearchModel> SearchList
        {
            get
            {
                return _searchList;
            }
            set
            {
                _searchList = value;
                OnPropertyChanged();
            }
        }

        public SearchModel SelectedSearch
        {
            get
            {
                return _selectedSearch;
            }
            set
            {
                _selectedSearch = value;
                OnPropertyChanged();
            }
        }

        public ICommand ButtonClicks
        {
            get
            {
                return _buttonClicks;
            }
        }

        public event Action<bool> RequestClose;

        public event Action<string, string> RequestSave;
        #endregion

        #region Private Methods
        private void Initialize()
        {
            SearchList = new ObservableCollection<SearchModel>();
        }

        private void ReadApiKey()
        {
            string path = Path.Combine(_projectPath, "ApiKey", _apiKeyFile);
            using (StreamReader reader = new StreamReader(path))
            {
                _googleApiKey = reader.ReadLine();
            }
        }

        private async void SearchMusic()
        {
            if (SearchQuery == null || SearchQuery.Length == 0)
            {
                MessageBox.Show("Please enter title or musician name");
                return;
            }

            var youtube = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = _googleApiKey,
                ApplicationName = "YouTube Player"
            });

            var request = youtube.Search.List("snippet");
            request.Q = SearchQuery;
            request.MaxResults = 25;

            var result = await request.ExecuteAsync();

            SearchList.Clear();

            foreach (var item in result.Items)
            {
                if (item.Id.Kind == "youtube#video")
                {
                    SearchList.Add(new SearchModel(item.Snippet.Title, _baseYoutubeUrl + item.Id.VideoId));
                }
            }
        }

        private void OnButtonClicks(object param)
        {
            SearchViewParam clicked = (SearchViewParam)param;
            switch (clicked)
            {
                case SearchViewParam.Search:
                    SearchMusic();
                    break;
                case SearchViewParam.Ok:
                    if (SelectedSearch != null)
                    {
                        RequestSave(SelectedSearch.Title, SelectedSearch.Address);
                        RequestClose(true);
                    }
                    
                    break;
                case SearchViewParam.Cancel:
                    RequestClose(true);
                    break;
            }
        }

        private void RegisterCommands()
        {
            _buttonClicks = new RelayCommand((param) => OnButtonClicks(param));
        }
        #endregion

        #region Constructor
        public SearchViewModel()
        {
            Initialize();
            RegisterCommands();
            ReadApiKey();
        }
        #endregion
    }
}

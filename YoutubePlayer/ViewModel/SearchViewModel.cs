using System;
using System.Collections.ObjectModel;
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

        private async void SearchMusic()
        {
            if (SearchQuery == null || SearchQuery.Length == 0)
            {
                MessageBox.Show("Please enter title or musician name");
                return;
            }
            
            var items = await YouTubeHelper.Search(SearchQuery);

            SearchList.Clear();

            foreach (var item in items)
            {
                if (item.Id.Kind == "youtube#video")
                {
                    SearchList.Add(new SearchModel(item.Snippet.Title, YouTubeHelper.GetBaseUrl() + item.Id.VideoId));
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
        }
        #endregion
    }
}

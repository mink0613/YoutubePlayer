using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using YoutubePlayer.Common;
using YoutubePlayer.Model;
using YoutubePlayer.View;

namespace YoutubePlayer.ViewModel
{
    public enum PlayerViewParam
    {
        Search,
        Extend,
        Prev,
        Play,
        Next
    }

    public class PlayerViewModel : BaseProperty
    {
        #region Private Variables
        private readonly string _projectPath = Directory.GetParent(Directory.GetParent(
            Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)).ToString()).ToString();

        private readonly string _apiKeyFile = "ApiKey.txt";

        private readonly string _baseYoutubeUrl = "http://youtube.com/watch?v=";

        private string _googleApiKey = "";

        private bool _isExtend;

        private ObservableCollection<PlayerModel> _musicLists;

        private string _youTubeAddress;

        private ICommand _buttonClicks;
        #endregion

        #region Public Properteis
        public bool IsExtend
        {
            get
            {
                return _isExtend;
            }
            set
            {
                _isExtend = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<PlayerModel> MusicLists
        {
            get
            {
                return _musicLists;
            }
            set
            {
                _musicLists = value;
                OnPropertyChanged();
            }
        }

        public string YouTubeAddress
        {
            get
            {
                return _youTubeAddress;
            }
            set
            {
                _youTubeAddress = value;
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
        #endregion

        #region Private Methods
        private async void ReadSavedMusicLists()
        {
            if (_musicLists != null)
            {
                _musicLists.Clear();
            }

            var youtube = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = _googleApiKey,
                ApplicationName = "YouTube Player"
            });
            
            var request = youtube.Search.List("snippet");
            request.Q = "lineage";
            request.MaxResults = 25;

            var result = await request.ExecuteAsync();

            foreach (var item in result.Items)
            {
                if (item.Id.Kind == "youtube#video")
                {
                    MusicLists.Add(new PlayerModel(item.Snippet.Title, _baseYoutubeUrl + item.Id.VideoId));
                }
            }

            YouTubeAddress = MusicLists[0].Address;
        }

        private void Initialize()
        {
            _musicLists = new ObservableCollection<PlayerModel>();
            ReadApiKey();
            ReadSavedMusicLists();
        }

        private void ReadApiKey()
        {
            string path = Path.Combine(_projectPath, "ApiKey", _apiKeyFile);
            using (StreamReader reader = new StreamReader(path))
            {
                _googleApiKey = reader.ReadLine();
            }
        }

        private void ShowSearchView()
        {
            SearchView view = new SearchView();
            view.ViewModel.RequestSave += (string title, string url) =>
            {
                if (title != null && url != null)
                {
                    MusicLists.Add(new PlayerModel(title, url));
                }
            };
            view.ViewModel.RequestClose += (bool result) =>
            {
                view.Close();
            };
        }

        private void OnButtonClicks(object param)
        {
            PlayerViewParam clicked = (PlayerViewParam)param;
            switch (clicked)
            {
                case PlayerViewParam.Search:
                    break;
                case PlayerViewParam.Extend:
                    break;
            }

        }

        private void RegisterCommands()
        {
            _buttonClicks = new RelayCommand((param) => OnButtonClicks(param));
        }
        #endregion

        #region Constructor
        public PlayerViewModel()
        {
            Initialize();
            RegisterCommands();
        }
        #endregion
    }
}

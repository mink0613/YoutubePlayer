using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System.Collections.ObjectModel;
using System.Windows.Input;
using YoutubePlayer.Common;
using YoutubePlayer.Model;

namespace YoutubePlayer.ViewModel
{
    public class PlayerViewModel : BaseProperty
    {
        #region Private Variables
        private readonly string _googleApiKey = "AIzaSyCcfTgTcdQGn9TpVodnaFujTHmqC25Jhlk";

        private readonly string _baseYoutubeUrl = "http://youtube.com/watch?v=";

        private bool _isExtend;

        private ObservableCollection<PlayerModel> _musicLists;

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
        }

        private void Initialize()
        {
            _musicLists = new ObservableCollection<PlayerModel>();
            ReadSavedMusicLists();
        }

        private void OnButtonClicks(object param)
        {
            string clicked = param as string;

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

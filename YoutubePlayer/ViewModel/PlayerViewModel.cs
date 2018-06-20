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

            //YouTubeAddress = MusicLists[0].Address;
        }

        private void Initialize()
        {
            _musicLists = new ObservableCollection<PlayerModel>();
            ReadSavedMusicLists();
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
            view.Show();
        }

        private void OnButtonClicks(object param)
        {
            PlayerViewParam clicked = (PlayerViewParam)param;
            switch (clicked)
            {
                case PlayerViewParam.Search:
                    ShowSearchView();
                    break;
                case PlayerViewParam.Extend:
                    IsExtend = !IsExtend;
                    break;
                case PlayerViewParam.Prev:
                    break;
                case PlayerViewParam.Play:
                    break;
                case PlayerViewParam.Next:
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

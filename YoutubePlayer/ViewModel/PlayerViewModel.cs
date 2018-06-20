using System.Collections.ObjectModel;
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
        Next,
        ListUp,
        ListDown,
        ListDelete
    }

    public class PlayerViewModel : BaseProperty
    {
        #region Private Variables
        private readonly int _originalWidth = 310;

        private readonly int _extendedWidth = 650;

        private int _playerWidth;

        private bool _isExtend;

        private ObservableCollection<PlayerModel> _musicList;

        private PlayerModel _selectedMusic;

        private int _selectedIndex;

        private string _youTubeAddress;

        private ICommand _buttonClicks;
        #endregion

        #region Public Properteis
        public int PlayerWidth
        {
            get
            {
                return _playerWidth;
            }
            set
            {
                _playerWidth = value;
                OnPropertyChanged();
            }
        }

        public bool IsExtend
        {
            get
            {
                return _isExtend;
            }
            set
            {
                _isExtend = value;
                if (_isExtend == true)
                {
                    PlayerWidth = _extendedWidth;
                }
                else
                {
                    PlayerWidth = _originalWidth;
                }

                OnPropertyChanged();
            }
        }

        public ObservableCollection<PlayerModel> MusicList
        {
            get
            {
                return _musicList;
            }
            set
            {
                _musicList = value;
                OnPropertyChanged();
            }
        }

        public PlayerModel SelectedMusic
        {
            get
            {
                return _selectedMusic;
            }
            set
            {
                _selectedMusic = value;
                OnPropertyChanged();
            }
        }

        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                _selectedIndex = value;
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
        private void ReadSavedMusicList()
        {
            if (_musicList != null)
            {
                _musicList.Clear();
            }

            //YouTubeAddress = MusicLists[0].Address;
        }

        private void Initialize()
        {
            IsExtend = false;
            _musicList = new ObservableCollection<PlayerModel>();
            ReadSavedMusicList();
        }

        private void ShowSearchView()
        {
            SearchView view = new SearchView();
            view.ViewModel.RequestSave += (string title, string url) =>
            {
                if (title != null && url != null)
                {
                    MusicList.Add(new PlayerModel(title, url));
                }
            };
            view.ViewModel.RequestClose += (bool result) =>
            {
                view.Close();
            };
            view.Show();
        }

        private void PlayPreviousMusic()
        {
            if (MusicList.Count == 0)
            {
                return;
            }

            if (SelectedIndex == 0)
            {
                return;
            }

            SelectedIndex--;
            YouTubeAddress = MusicList[SelectedIndex].Address;
        }

        private void PlayNextMusic()
        {
            if (MusicList.Count == 0)
            {
                return;
            }

            if (MusicList.Count - 1 == SelectedIndex)
            {
                return;
            }

            SelectedIndex--;
            YouTubeAddress = MusicList[SelectedIndex].Address;
        }

        private void PlayMusic()
        {
            if (MusicList.Count == 0)
            {
                return;
            }

            YouTubeAddress = MusicList[SelectedIndex].Address;
        }

        private void ListUp()
        {
            if (SelectedMusic == null)
            {
                return;
            }

            int index = MusicList.IndexOf(SelectedMusic);
            if (index == 0)
            {
                return;
            }

            PlayerModel model = SelectedMusic.Clone() as PlayerModel;
            MusicList.Remove(SelectedMusic);
            MusicList.Insert(index - 1, model);

            SelectedIndex = MusicList.IndexOf(model);
        }

        private void ListDown()
        {
            if (SelectedMusic == null)
            {
                return;
            }

            int index = MusicList.IndexOf(SelectedMusic);
            if (index == MusicList.Count - 1)
            {
                return;
            }

            PlayerModel model = SelectedMusic.Clone() as PlayerModel;
            MusicList.Remove(SelectedMusic);
            MusicList.Insert(index + 1, model);

            SelectedIndex = MusicList.IndexOf(model);
        }

        private void ListDelete()
        {
            if (SelectedMusic == null)
            {
                return;
            }

            MusicList.Remove(SelectedMusic);
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
                    PlayPreviousMusic();
                    break;
                case PlayerViewParam.Play:
                    PlayMusic();
                    break;
                case PlayerViewParam.Next:
                    PlayNextMusic();
                    break;
                case PlayerViewParam.ListUp:
                    ListUp();
                    break;
                case PlayerViewParam.ListDown:
                    ListDown();
                    break;
                case PlayerViewParam.ListDelete:
                    ListDelete();
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

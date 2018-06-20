using System;
using YoutubePlayer.Common;

namespace YoutubePlayer.Model
{
    public class PlayerModel : BaseProperty, ICloneable
    {
        #region Private Variables
        private string _title;

        private string _address;
        #endregion

        #region Public Properties
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructor
        public PlayerModel()
        {

        }

        public PlayerModel(string title, string address)
        {
            Title = title;
            Address = address;
        }

        public object Clone()
        {
            PlayerModel model = new PlayerModel();
            model.Title = this.Title;
            model.Address = this.Address;

            return model;
        }
        #endregion
    }
}

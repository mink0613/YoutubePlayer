using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubePlayer.Common;

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
        #endregion

        #region Public Properties
        public event Action<bool> RequestClose;

        public event Action<string, string> RequestSave;
        #endregion

        #region Private Methods
        #endregion

        #region Constructor
        #endregion
    }
}

using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace YoutubePlayer.Common
{
    public class YouTubeHelper
    {
        private static readonly Regex _youTubeURLIDRegex = new Regex(@"[\?&]v=(?<v>[^&]+)");

        private static readonly string _apiKeyFile = "ApiKey.txt";

        private static string _googleApiKey = string.Empty;

        private static readonly string _youTubeEmbedUrl = "https://www.youtube.com/embed/";

        private static void ReadApiKey()
        {
            string path = Path.Combine(Helper.ProjectPath, "ApiKey", _apiKeyFile);
            using (StreamReader reader = new StreamReader(path))
            {
                _googleApiKey = reader.ReadLine();
            }
        }

        private static YouTubeService GetYouTubeService()
        {
            if (_googleApiKey == string.Empty)
            {
                ReadApiKey();
            }

            var youtube = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = _googleApiKey,
                ApplicationName = "YouTube Player"
            });

            return youtube;
        }

        /// <summary>
        /// Deprecated: YouTube does not support anymore
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string GetYouTubeScript(string id)
        {
            string scr = @"<object width='320' height='240'> " + "\r\n";
            scr = scr + @"<param name='movie' value='http://www.youtube.com/v/" + id + "'></param> " + "\r\n";
            scr = scr + @"<param name='allowFullScreen' value='true'></param> " + "\r\n";
            scr = scr + @"<param name='allowscriptaccess' value='always'></param> " + "\r\n";
            scr = scr + @"<embed src='http://www.youtube.com/v/" + id + "' ";
            scr = scr + @"type='application/x-shockwave-flash' allowscriptaccess='always' ";
            scr = scr + @"allowfullscreen='true' width='320' height='240'> " + "\r\n";
            scr = scr + @"</embed></object>" + "\r\n";
            return scr;
        }

        /// <summary>
        /// Deprecated: YouTube does not support anymore
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetYouTubeVideoUrl(string url)
        {
            Match m = _youTubeURLIDRegex.Match(url);
            string id = m.Groups["v"].Value;

            string page =
                    "<!DOCTYPE html PUBLIC \" -//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\" >\r\n"
                    + @"<!-- saved from url=(0022)http://www.youtube.com -->" + "\r\n"
                    + "<html><body scroll=\"no\" leftmargin=\"0px\" topmargin=\"0px\" marginwidth=\"0px\" marginheight=\"0px\" >" + "\r\n"
                    + GetYouTubeScript(id)
                    + "</body></html>\r\n";

            return page;
        }

        private static string GetEmbeddedYouTubeUrl(string src)
        {
            // Official url: https://developers.google.com/youtube/player_parameters?hl=en
            var embed = "<html><head>" 
                + "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=Edge\"/>" 
                + "</head><body>" 
                + "<iframe id=youtubeplayer width=\"320\" height=\"240\" src=\"{0}?autoplay=1&modestbranding=1&rel=0\""
                + "frameborder = \"0\" encrypted-media\" allowfullscreen></iframe>" 
                + "</body></html>";

            return string.Format(embed, src);
        }

        public static string GetEmbeddedYouTubeVideoUrl(string url)
        {
            Match m = _youTubeURLIDRegex.Match(url);
            string id = m.Groups["v"].Value;

            string source = _youTubeEmbedUrl + id;

            return GetEmbeddedYouTubeUrl(source);
        }

        public static async Task<IList<SearchResult>> Search(string query)
        {
            var youtube = GetYouTubeService();

            var request = youtube.Search.List("snippet");
            request.Q = query;
            request.MaxResults = 25;

            var result = await request.ExecuteAsync();
            return result.Items;
        }

        public static async Task<IList<Playlist>> GetPlayList(string id)
        {
            var youtube = GetYouTubeService();

            var request = youtube.Playlists.List("snippet");
            request.Id = id;
            request.MaxResults = 25;

            var result = await request.ExecuteAsync();
            return result.Items;
        }

        public static async Task<IList<Video>> GetVideos(string id)
        {
            var youtube = GetYouTubeService();

            var request = youtube.Videos.List("snippet");
            request.Id = id;
            request.MaxResults = 25;

            var result = await request.ExecuteAsync();
            return result.Items;
        }
    }
}

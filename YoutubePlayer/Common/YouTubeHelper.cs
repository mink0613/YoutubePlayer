using System.IO;
using System.Text.RegularExpressions;

namespace YoutubePlayer.Common
{
    public class YouTubeHelper
    {
        private static Regex _youTubeURLIDRegex = new Regex(@"[\?&]v=(?<v>[^&]+)");

        private static readonly string _youTubeEmbedUrl = "https://www.youtube.com/embed/";

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
    }
}

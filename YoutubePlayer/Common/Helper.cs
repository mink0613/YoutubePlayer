using System.IO;

namespace YoutubePlayer.Common
{
    public class Helper
    {
        public static readonly string ProjectPath = Directory.GetParent(Directory.GetParent(
            Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)).ToString()).ToString();
    }
}

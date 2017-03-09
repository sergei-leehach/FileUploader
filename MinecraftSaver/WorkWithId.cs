using System.Configuration;
using System.IO;

namespace MinecraftSaver
{
    public class WorkWithId
    {
        private static string _path;
        public static string Path => _path ?? (_path = ConfigurationManager.AppSettings["PathToErKucho"]);

        public static void SaveId(string id)
        {
            var path = PathToId();
            File.WriteAllText(path, id);
        }

        public static string GetId()
        {
            var path = PathToId();
            var id = File.ReadAllText(path);

            return id;
        }

        private static string PathToId()
        {
            var path = ConfigurationManager.AppSettings["Path"];
            var fileName = path.GetHashCode();
            var fullPath = Directory.GetCurrentDirectory() + fileName;

            return fullPath;
        }
    }
}

using System.IO;
using Microsoft.Win32;

namespace MinecraftSaver
{
    public static class StringExtension
    {
        public static string GetMimeType(this string fileName)
        {
            var mimeType = "application/unknown";
            var ext = Path.GetExtension(fileName)?.ToLower();

            if (ext == null) return mimeType;

            var regKey = Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey?.GetValue("Content Type") != null)
            {
                mimeType = regKey.GetValue("Content Type").ToString();
            }

            return mimeType;
        }
    }
}

using System;
using System.Configuration;
using System.IO;
using System.IO.Compression;

namespace MinecraftSaver
{
    public class Archivator
    {
        public string SourceFileName { get; }
        public string ArchiveFileName { get; }

        public Archivator()
        {
            SourceFileName = ConfigurationManager.AppSettings["Path"];
            ArchiveFileName = SourceFileName + ".zip";
        }

        public string Archiving()
        {            
            if (File.Exists(ArchiveFileName))
            {
                File.Delete(ArchiveFileName);
                Console.WriteLine("Outdated archive was deleted");
            }

            ZipFile.CreateFromDirectory(SourceFileName, ArchiveFileName);
            Console.WriteLine("Archive created");

            return ArchiveFileName;
        }

        public void DeleteArchiv()
        {
            if (File.Exists(ArchiveFileName))
            {
                File.Delete(ArchiveFileName);
                Console.WriteLine("Archive was deleted after work");
            }
            else
            {
                Console.WriteLine("Archive doesn't exists already");
            }           
        }
    }
}

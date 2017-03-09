using System;
namespace MinecraftSaver
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var archivator = new Archivator();
            var archiveFileName = archivator.Archiving();

            var api = new Api(archiveFileName);
            var response = api.FileUploading();

            if (response.Id != null)
            {
                Console.WriteLine("File successfully uploaded to Google Drive!");
            }

            archivator.DeleteArchiv();
        }       
    }
}

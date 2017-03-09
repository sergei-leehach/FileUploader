using System;
using System.IO;
using System.Linq;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using File = Google.Apis.Drive.v3.Data.File;

namespace MinecraftSaver
{
    public class Api
    {
        private static readonly string[] Scopes = { DriveService.Scope.Drive };
        public string ApplicationName { get; }
        public string ArchiveFileName { get; }

        public Api(string archiveFileName)
        {
            ApplicationName = "Er Kucho Uploader";

            if (System.IO.File.Exists(archiveFileName))
            {
                ArchiveFileName = archiveFileName;
            }
            else
            {
                Console.WriteLine("File doesn't exists!!!");
                Console.ReadLine();
            }
        }

        public File FileUploading()
        {
            File response;
            var driveService = Service();
            var id = GetId(driveService);

            if (!string.IsNullOrEmpty(id))
            {
                Console.WriteLine("Id is defined, updating the file");
                response = UpdateFile(driveService, ArchiveFileName, id);

                return response;
            }

            Console.WriteLine("Id is null, creating the file");
            response = CreateFile(driveService, ArchiveFileName);

            return response;
        }

        private string GetId(DriveService driveService)
        {
            var list = driveService.Files.List();
            var files = list.Execute().Files;

            var startIndex = ArchiveFileName.LastIndexOf('\\');
            var fileName = ArchiveFileName.Substring(startIndex + 1);

            var fileId = files.Where(x => x.Name == fileName).Select(x => x.Id).SingleOrDefault();

            return fileId;
        }

        private DriveService Service()
        {
            var service = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = Credentials(),
                ApplicationName = ApplicationName
            });

            return service;
        }

        private static UserCredential Credentials()
        {
            var credPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
           
            using (var stream = new FileStream(Path.Combine(credPath, @".credentials\client_id.json"), FileMode.Open, FileAccess.Read))
            {
                credPath = Path.Combine(credPath, @".credentials\Er Kucho Saver");

                var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "lee",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);

                return credential;
            }
        }

        private static File CreateFile(DriveService service, string uploadFile)
        {
            var body = Body(uploadFile);
            var stream = Stream(uploadFile);

            var request = service.Files.Create(body, stream, body.MimeType);
            request.Upload();

            return request.ResponseBody;
        }

        private static File UpdateFile(DriveService service, string uploadFile, string id)
        {
            var body = Body(uploadFile);
            var stream = Stream(uploadFile);

            var request = service.Files.Update(body, id, stream, body.MimeType);
            request.Upload();

            return request.ResponseBody;
        }

        private static File Body(string uploadFile)
        {
            var body = new File
            {
                Name = Path.GetFileName(uploadFile),
                Description = "Er Kucho Save",
                MimeType = uploadFile.GetMimeType() //"application/vnd.google-apps.file"
            };

            return body;
        }

        private static MemoryStream Stream(string uploadFile)
        {
            var byteArray = System.IO.File.ReadAllBytes(uploadFile);
            var stream = new MemoryStream(byteArray);

            return stream;
        }
    }
}


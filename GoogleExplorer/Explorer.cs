using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GFile = Google.Apis.Drive.v3.Data.File;
using File = System.IO.File;
using GoogleExplorer.Extensions;
using System.Diagnostics;
using Google.Apis.Drive.v3.Data;

namespace GoogleExplorer
{
    public class Explorer
    {
        private readonly string[] scopes = new string[] { DriveService.Scope.Drive };
        private const string credentials = "credentials.json";
        
        public string ApplicationName { get; init; }
        public string UserMail { get; private set; }
        public string CoreFolder { get { return $"{ApplicationName}-{UserMail}"; } }


        private DriveService DriveService { get; set; }
        
        private async Task<DriveService> Authenticate()
        {
            if (DriveService == null)
            {
                UserCredential credential;

                using FileStream fs = new FileStream(credentials, FileMode.Open, FileAccess.Read);

                string credPath = "token.json";

                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(fs).Secrets,
                    scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)
                    );

                DriveService = new(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });

                AboutResource.GetRequest userRequest = DriveService.About.Get();
                userRequest.Fields = "user";
                About user = await userRequest.ExecuteAsync();
                UserMail = user.User.EmailAddress;
            }

            return DriveService;
        }

        private async Task<bool> RevokeToken()
        {
            UserCredential credential;

            using FileStream fs = new FileStream(credentials, FileMode.Open, FileAccess.Read);

            string credPath = "token.json";

            credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.FromStream(fs).Secrets,
                scopes,
                "user",
                CancellationToken.None,
                new FileDataStore(credPath, true)
                );

            bool result = await credential.RevokeTokenAsync(CancellationToken.None);

            if(result)
                DriveService = null;

            return result;
        }

        private Explorer(string name)
        {
            ApplicationName = name;
        }

        public static async Task<Explorer> CreateAsync(string name)
        {
            Explorer explorer;
            try
            {
                await File.ReadAllTextAsync(credentials);
                explorer = new(name);
            }
            catch(FileNotFoundException)
            {
                return null;
            }

            await explorer.Authenticate();

            return explorer;
        }

        public async Task Dispose()
        {
            await RevokeToken();
            DriveService = null;
        }

        public async Task<GFile> GetFile(string name)
        {
            DriveService service = await Authenticate();
            FilesResource.ListRequest list = service.Files.List();

            IList<GFile> files = list.Execute().Files;

            if (files != null && files.Count > 0)
                return files.Where(f => f.Name == name)
                    .FirstOrDefault();

            return null;
        }

        public async Task<GFile> GetFile(string name, string folder)
        {
            DriveService service = await Authenticate();
            FilesResource.ListRequest list = service.Files.List();

            IList<GFile> files = list.Execute().Files;

            if (files != null && files.Count > 0)
                return files.Where(f => f.Name == name && f.Parents != null && f.Parents.Contains(folder))
                    .FirstOrDefault();

            return null;
        }

        public async Task<GFile> GetFile(string name, MimeTypes mime)
        {
            DriveService service = await Authenticate();
            FilesResource.ListRequest list = service.Files.List();

            IList<GFile> files = list.Execute().Files;

            if (files != null && files.Count > 0)
                return files.Where(f => f.Name == name && f.MimeType == mime.GetMimeType())
                    .FirstOrDefault();

            return null;
        }

        public async Task<GFile> GetFile(string name, string folder, MimeTypes mime)
        {
            DriveService service = await Authenticate();
            FilesResource.ListRequest list = service.Files.List();
            list.Fields = "files(id, name, parents, mimeType)";

            GFile gfolder = await GetFile(folder, MimeTypes.GoogleFolder);

            IList<GFile> files = list.Execute().Files;

            if (files != null && files.Count > 0)
            {
                List<GFile> glist = files.Where(f => f.Name == name).ToList();
                glist = glist.Where(f => f.MimeType == mime.GetMimeType()).ToList();
                glist = glist.Where(f => f.Parents != null && f.Parents.Contains(gfolder.Id)).ToList();
                return glist
                    .FirstOrDefault();
            }

            return null;
        }

        public async Task<string[]> ListFilesNames()
        {
            DriveService service = await Authenticate();
            FilesResource.ListRequest list = service.Files.List();

            IList<GFile> files = list.Execute().Files;

            if (files != null && files.Count > 0)
                return files.Select(d => d.Name).ToArray();

            return null;
        }

        public async Task UploadFile(string name, byte[] fileBuffer, MimeTypes mime)
        {
            DriveService service = await Authenticate();
            GFile gfile = new()
            {
                Name = name
            };

            using MemoryStream ms = new(fileBuffer);

            FilesResource.CreateMediaUpload request = service.Files
                .Create(gfile, ms, mime.GetMimeType());

            request.Fields = "id";
            request.Upload();

            Debug.WriteLine(request.ResponseBody);
        }

        public async Task UploadFile(string name, string folder, byte[] fileBuffer, MimeTypes mime)
        {
            DriveService service = await Authenticate();

            GFile gfolder = await GetFile(folder, MimeTypes.GoogleFolder);

            GFile gfile = new()
            {
                Name = name,
                Parents = new List<string> { gfolder.Id }
            };

            using MemoryStream ms = new(fileBuffer);

            FilesResource.CreateMediaUpload request = service.Files
                .Create(gfile, ms, mime.GetMimeType());

            request.Fields = "id";
            request.Upload();

            Debug.WriteLine(request.ResponseBody);
        }

        public async Task<byte[]> DownloadFile(string name, string folder, MimeTypes mime)
        {
            DriveService service = await Authenticate();

            GFile gfile = await GetFile(name, folder, mime);

            FilesResource.GetRequest request = service.Files
                .Get(gfile.Id);

            using MemoryStream ms = new MemoryStream();
            request.Download(ms);

            return ms.ToArray();
        }

        public async Task<byte[]> DownloadFile(string name, MimeTypes mime)
        {
            DriveService service = await Authenticate();

            GFile gfile = await GetFile(name, mime);

            FilesResource.GetRequest request = service.Files
                .Get(gfile.Id);

            using MemoryStream ms = new MemoryStream();
            request.Download(ms);

            return ms.ToArray();
        }

        public async Task OverwriteFile(string name, string folder, byte[] fileBuffer, MimeTypes mime)
        {
            DriveService service = await Authenticate();

            GFile gfile = await GetFile(name, folder, mime);

            GFile nfile = new();

            using MemoryStream ms = new(fileBuffer);

            FilesResource.UpdateMediaUpload request = service.Files
                .Update(nfile, gfile.Id, ms, mime.GetMimeType());

            request.Upload();

            Debug.WriteLine(request.ResponseBody);
        }

        public async Task<string[]> ListFoldersNames()
        {
            DriveService service = await Authenticate();
            FilesResource.ListRequest list = service.Files.List();

            IList<GFile> files = list.Execute().Files;

            if (files != null && files.Count > 0)
                return files.Where(f=>f.MimeType == MimeTypes.GoogleFolder.GetMimeType())
                    .Select(f => f.Name)
                    .ToArray();

            return null;
        }
        
        public async Task<GFile[]> ListFolders()
        {
            DriveService service = await Authenticate();
            FilesResource.ListRequest list = service.Files.List();

            IList<GFile> files = list.Execute().Files;

            if (files != null && files.Count > 0)
                return files.Where(f=>f.MimeType == MimeTypes.GoogleFolder.GetMimeType())
                    .ToArray();

            return null;
        }

        public async Task<string> CreateFolder(string name)
        {
            DriveService service = await Authenticate();

            GFile gfolder = new()
            {
                Name = name,
                MimeType = MimeTypes.GoogleFolder.GetMimeType()
            };

            FilesResource.CreateRequest createRequest = service.Files
                .Create(gfolder);

            createRequest.Fields = "id";

            GFile nfolder = createRequest.Execute();

            return nfolder.Id;
        }

    }
}

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
using GoogleExplorer.DataTransfer;
using Google.Apis.Upload;

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
        
        private async Task Authenticate()
        {
            try
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

            }
            catch(Exception ex)
            {
                Type exType = ex.GetType();
                string strExType = exType.ToString();
                Debug.WriteLine(strExType);
            }

            AboutResource.GetRequest userRequest = DriveService.About.Get();
            userRequest.Fields = "user";
            About user = await userRequest.ExecuteAsync();
            UserMail = user.User.EmailAddress;
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

        private async Task<GFile> GetFileAsync(string name, MimeTypes mime)
        {
            string pageToken = null;
            List<GFile> gfiles = new();

            do
            {
                FilesResource.ListRequest list = DriveService.Files.List();
                list.Fields = "files(id, name, parents, mimeType)";
                list.PageSize = 100;
                list.PageToken = pageToken;

                FileList glist = await list.ExecuteAsync();
                pageToken = glist.NextPageToken;
                gfiles.AddRange(glist.Files);
            } while (pageToken != null && gfiles.Count < 3000);

            if (gfiles != null && gfiles.Count > 0)
            {
                return gfiles.Where(f => f.Name == name && f.MimeType == mime.GetMimeType())
                    .Distinct(new FileEqualityComparer())
                    .FirstOrDefault();
            }

            return null;
        }

        private async Task<GFile> GetFileAsync(string name, string folderId, MimeTypes mime)
        {
            string pageToken = null;
            List<GFile> gfiles = new();

            do
            {
                FilesResource.ListRequest list = DriveService.Files.List();
                list.Fields = "files(id, name, parents, mimeType)";
                list.PageSize = 100;
                list.PageToken = pageToken;

                FileList glist = await list.ExecuteAsync();
                pageToken = glist.NextPageToken;
                gfiles.AddRange(glist.Files);
            } while (pageToken != null && gfiles.Count < 3000);

            if (gfiles != null && gfiles.Count > 0)
            {
                return gfiles.Where(f => f.Name == name && f.MimeType == mime.GetMimeType() && f.Parents != null && f.Parents.Contains(folderId))
                    .Distinct(new FileEqualityComparer())
                    .FirstOrDefault();
            }

            return null;
        }

        public async Task<FileMetadata> GetFileMetaDataAsync(string name, string folderId, MimeTypes mime)
        {
            string pageToken = null;
            List<GFile> gfiles = new();

            do
            {
                FilesResource.ListRequest list = DriveService.Files.List();
                list.Fields = "files(id, name, parents, mimeType)";
                list.PageSize = 100;
                list.PageToken = pageToken;

                FileList glist = await list.ExecuteAsync();
                pageToken = glist.NextPageToken;
                gfiles.AddRange(glist.Files);
            } while (pageToken != null && gfiles.Count < 3000);

            if (gfiles != null && gfiles.Count > 0)
            {
                return gfiles.Where(f => f.Name == name && f.MimeType == mime.GetMimeType() && f.Parents != null && f.Parents.Contains(folderId))
                    .Distinct(new FileEqualityComparer())
                    .Select(g => new FileMetadata { ID = g.Id, Name = g.Name, ParentFolder = g.Parents.ToArray(), MimeType = g.MimeType.GetMimeTypes() })
                    .FirstOrDefault();
            }

            return null;
        }

        private async Task<GFile> GetFileByIdAsync(string fileId, MimeTypes mime)
        {
            string pageToken = null;
            List<GFile> gfiles = new();

            do
            {
                FilesResource.ListRequest list = DriveService.Files.List();
                list.Fields = "files(id, name, parents, mimeType)";
                list.PageSize = 100;
                list.PageToken = pageToken;

                FileList glist = await list.ExecuteAsync();
                pageToken = glist.NextPageToken;
                gfiles.AddRange(glist.Files);
            } while (pageToken != null && gfiles.Count < 3000);

            if (gfiles != null && gfiles.Count > 0)
            {
                return gfiles.Where(f => f.Id == fileId && f.MimeType == mime.GetMimeType())
                    .Distinct(new FileEqualityComparer())
                    .FirstOrDefault();
            }

            return null;
        }

        public async Task<FileMetadata[]> GetAllFilesAsync(string name)
        {
            string pageToken = null;
            List<GFile> gfiles = new();

            do
            {
                FilesResource.ListRequest list = DriveService.Files.List();
                list.Fields = "nextPageToken, files(id, name, parents, mimeType)";
                list.PageSize = 100;
                list.PageToken = pageToken;
                
                FileList glist = await list.ExecuteAsync();
                pageToken = glist.NextPageToken;
                gfiles.AddRange(glist.Files);

            } while (pageToken != null && gfiles.Count < 3000);

            if (gfiles != null && gfiles.Count > 0)
            {
                return gfiles.Where(f => f.Name == name)
                    .Distinct(new FileEqualityComparer())
                    .Select(f => new FileMetadata
                    {
                        ID = f.Id,
                        Name = f.Name,
                        MimeType = f.MimeType.GetMimeTypes(),
                        ParentFolder = f.Parents.ToArray()
                    })
                    .ToArray();
            }

            return Array.Empty<FileMetadata>();
        }

        public async Task<FileMetadata> UploadFileAsync(string name, string folderId, byte[] fileBuffer, MimeTypes mime)
        {
            GFile gfile = new()
            {
                Name = name,
                Parents = new List<string> { folderId }
            };

            using MemoryStream ms = new(fileBuffer);

            FilesResource.CreateMediaUpload request = DriveService.Files
                .Create(gfile, ms, mime.GetMimeType());

            request.Fields = "files(id, name, parents, mimeType)";
            await request.UploadAsync();

            GFile nFile = await GetFileAsync(name, folderId, mime);

            return new FileMetadata { ID = nFile.Id, Name = nFile.Name, MimeType = nFile.MimeType.GetMimeTypes(), ParentFolder = nFile.Parents.ToArray() };
        }

        public async Task<byte[]> DownloadFileAsync(string fileId, MimeTypes mime)
        {
            GFile gfile = await GetFileByIdAsync(fileId, mime);

            FilesResource.GetRequest request = DriveService.Files
                .Get(gfile.Id);

            using MemoryStream ms = new MemoryStream();
            await request.DownloadAsync(ms);

            return ms.ToArray();
        }

        public async Task<FileMetadata> OverwriteFileAsync(string fileId, byte[] fileBuffer, MimeTypes mime)
        {
            GFile gfile = await GetFileByIdAsync(fileId, mime);

            GFile fileBody = new();

            using MemoryStream ms = new(fileBuffer);

            FilesResource.UpdateMediaUpload request = DriveService.Files
                .Update(fileBody, gfile.Id, ms, mime.GetMimeType());

            await request.UploadAsync();

            GFile nFile = await GetFileByIdAsync(fileId, mime);

            return new FileMetadata { ID = nFile.Id, Name = nFile.Name, MimeType = nFile.MimeType.GetMimeTypes(), ParentFolder = nFile.Parents.ToArray() };
        }

        public async Task<FileMetadata> CreateFolderAsync(string name, string folderId)
        {
            GFile gfolder = new()
            {
                Name = name,
                MimeType = MimeTypes.GoogleFolder.GetMimeType()
            };

            if (!string.IsNullOrWhiteSpace(folderId))
                gfolder.Parents = new string[] { folderId };

            FilesResource.CreateRequest createRequest = DriveService.Files
                .Create(gfolder);

            createRequest.Fields = "id";

            GFile nfolder = await createRequest.ExecuteAsync();

            return new FileMetadata { ID = nfolder.Id, Name = name, MimeType = MimeTypes.GoogleFolder };
        }

        public async Task ShareFile(string fileId, string userEmail)
        {
            Permission gPermission = new Permission
            {
                Type = "user",
                Role = "writer",
                EmailAddress = userEmail
            };

            PermissionsResource.CreateRequest request = DriveService.Permissions.Create(gPermission, fileId);
            request.Fields = "id";

            await request.ExecuteAsync();
            
        }
    }
}

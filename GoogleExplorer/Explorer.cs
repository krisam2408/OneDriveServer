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
        private const string fileSearchProperties = "files(id, name, parents, mimeType, permissions)";

        public string ApplicationName { get; init; }
        public string UserMail { get; private set; }
        public string CoreFolder { get { return $"{ApplicationName}-{UserMail}"; } }

        private CancellationTokenSource CancelToken { get; set; }
        private DriveService DriveService { get; set; }
        
        public async Task<RequestResult> Authenticate()
        {
            UserCredential credential;

            using FileStream fs = new FileStream(credentials, FileMode.Open, FileAccess.Read);

            string credPath = "token.json";

            CancelToken = new();
            CancellationToken token = CancelToken.Token;

            try
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(fs).Secrets,
                    scopes,
                    "user",
                    token,
                    new FileDataStore(credPath, true)
                    );
                
                DriveService = new(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });

            }
            catch(OperationCanceledException ex)
            {
                DebuggingExtension.DebugExpectionType(ex);
                return RequestResult.Cancelled;
            }
            catch(Exception ex)
            {
                DebuggingExtension.DebugExpectionType(ex);
                return RequestResult.Error;
            }

            try
            {
                AboutResource.GetRequest userRequest = DriveService.About.Get();
                userRequest.Fields = "user";
                About user = await userRequest.ExecuteAsync();
                UserMail = user.User.EmailAddress;
            }
            catch(Google.Apis.Auth.OAuth2.Responses.TokenResponseException)
            {
                return RequestResult.TokenExpired;
            }
            catch(Exception ex)
            {
                DebuggingExtension.DebugExpectionType(ex);
                return RequestResult.Error;
            }

            return RequestResult.Success;
        }

        public void CancelRequest()
        {
            CancelToken.Cancel();
            CancelToken.Dispose();
        }

        private async Task<bool> RevokeToken()
        {
            UserCredential credential;

            using FileStream fs = new FileStream(credentials, FileMode.Open, FileAccess.Read);

            string credPath = "token.json";

            CancelToken = new();
            CancellationToken token = CancelToken.Token;

            bool result;

            try
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(fs).Secrets,
                    scopes,
                    "user",
                    token,
                    new FileDataStore(credPath, true)
                    );

                result = await credential.RevokeTokenAsync(CancellationToken.None);
            }
            catch(OperationCanceledException ex)
            {
                DebuggingExtension.DebugExpectionType(ex);
                result = false;
            }

            if(result)
                DriveService = null;

            CancelToken.Dispose();

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

            return explorer;
        }

        public async Task Dispose()
        {
            await RevokeToken();
            DriveService = null;
        }

        private async Task<GFile[]> GetAllFilesAsync()
        {
            string pageToken = null;
            List<GFile> gfiles = new();

            do
            {
                FilesResource.ListRequest list = DriveService.Files.List();
                list.Fields = $"nextPageToken, {fileSearchProperties}";
                list.PageSize = 100;
                list.PageToken = pageToken;

                FileList glist = await list.ExecuteAsync();
                pageToken = glist.NextPageToken;
                gfiles.AddRange(glist.Files);
            } while (pageToken != null && gfiles.Count < 3000);

            return gfiles.Distinct(new FileEqualityComparer())
                .ToArray();
        }

        private async Task<GFile> GetFileByIdAsync(string fileId)
        {
            GFile[] data = await GetAllFilesAsync();

            return data
                .Where(f => f.Id == fileId)
                .FirstOrDefault();
        }

        private async Task<GFile[]> GetAllFilesOfNameAsync(string filename)
        {
            GFile[] data = await GetAllFilesAsync();

            return data
                .Where(f => f.Name == filename)
                .ToArray();
        }

        private async Task<GFile> GetFileByNameAsync(string filename, string folderId)
        {
            GFile[] data = await GetAllFilesOfNameAsync(filename);

            return data
                .Where(f => f.Parents != null && f.Parents.Contains(folderId))
                .FirstOrDefault();
        }

        public async Task<FileMetadata> GetFileMetaDataAsync(string filename, string folderId)
        {
            GFile gFile = await GetFileByNameAsync(filename, folderId);

            if (gFile == null)
                return null;

            return new FileMetadata(gFile);
        }

        public async Task<FileMetadata[]> GetAllFilesAsync(string filename)
        {
            GFile[] data = await GetAllFilesOfNameAsync(filename);

            return data
                .Select(f=> new FileMetadata(f))
                .ToArray();
        }

        public async Task<byte[]> DownloadFileAsync(string fileId)
        {
            GFile gfile = await GetFileByIdAsync(fileId);

            if (gfile == null)
                return null;

            FilesResource.GetRequest request = DriveService.Files
                .Get(gfile.Id);

            using MemoryStream ms = new MemoryStream();
            await request.DownloadAsync(ms);

            return ms.ToArray();
        }

        public async Task<FileMetadata> UploadFileAsync(string filename, string folderId, byte[] fileBuffer, MimeTypes mimeType)
        {
            GFile gfile = new()
            {
                Name = filename,
                Parents = new List<string> { folderId }
            };

            using MemoryStream ms = new(fileBuffer);

            FilesResource.CreateMediaUpload request = DriveService.Files
                .Create(gfile, ms, mimeType.GetMimeType());

            request.Fields = fileSearchProperties;
            await request.UploadAsync();

            GFile nFile = await GetFileByNameAsync(filename, folderId);

            return new FileMetadata(nFile);
        }

        public async Task<FileMetadata> OverwriteFileByIdAsync(string fileId, byte[] fileBuffer, MimeTypes mimeType)
        {
            GFile gfile = await GetFileByIdAsync(fileId);

            GFile fileBody = new();

            using MemoryStream ms = new(fileBuffer);

            FilesResource.UpdateMediaUpload request = DriveService.Files
                .Update(fileBody, gfile.Id, ms, mimeType.GetMimeType());

            await request.UploadAsync();

            GFile nFile = await GetFileByIdAsync(fileId);

            return new FileMetadata(nFile);
        }

        public async Task<FileMetadata> OverwriteFileByNameAsync(string filename, string folderId, byte[] fileBuffer, MimeTypes mime)
        {
            GFile gfile = await GetFileByNameAsync(filename, folderId);

            GFile fileBody = new();

            using MemoryStream ms = new(fileBuffer);

            FilesResource.UpdateMediaUpload request = DriveService.Files
                .Update(fileBody, gfile.Id, ms, mime.GetMimeType());

            await request.UploadAsync();

            GFile nFile = await GetFileByNameAsync(filename, folderId);

            return new FileMetadata(nFile);
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

            GFile nFolder = await createRequest.ExecuteAsync();

            return new FileMetadata(nFolder);
        }

        public async Task ShareFile(string fileId, string userEmail, Permissions permissions = Permissions.Reader)
        {
            GFile gFile = await GetFileByIdAsync(fileId);
            FileMetadata metadata = new(gFile);

            if(!metadata.ContainsEmailAddressPermission(UserMail))
            {
                Permission gPermission = new Permission
                {
                    Type = "user",
                    Role = permissions.Display(),
                    EmailAddress = userEmail
                };

                PermissionsResource.CreateRequest request = DriveService.Permissions.Create(gPermission, fileId);
                request.Fields = "id";

                await request.ExecuteAsync();
            }

        }
    }
}

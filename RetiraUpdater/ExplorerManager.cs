using GoogleExplorer;
using GoogleExplorer.DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetiraUpdater
{
    internal class ExplorerManager
    {
        private Explorer Explorer { get; set; }

        private static ExplorerManager m_instance;
        public static ExplorerManager Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new();
                return m_instance;
            }
        }

        private ExplorerManager() { }

        public async Task DisposeAsync()
        {
            await Explorer.Dispose();
            Explorer = null;

            m_instance = null;
        }

        public async Task LogInAsync(string alt = "")
        {
            Explorer = await Explorer.CreateAsync("RetiraUpdater", alt);

            if (Explorer == null)
                throw new NullReferenceException("No credentials found.");

            RequestResult authTry = await Explorer.Authenticate();

            if (authTry != RequestResult.Success)
            {
                throw authTry switch
                {
                    RequestResult.Cancelled => new Exception("Operation canceled."),
                    RequestResult.TokenExpired => new Exception("There was an authentication problem.Please try again."),
                    _ => new Exception("## 100"),
                };
            }
        }

        public async Task<FileMetadata[]> GetOnlineFolderFilesAsync(string foldername)
        {
            FileMetadata[] metadatas = await Explorer.GetAllFileMetadaOfNameAsync(foldername);
            FileMetadata[] output = await Explorer.GetAllFileMetadataFromFolderAsync(metadatas[0].ID);
            return output
                .Where(f => f.MimeType != MimeTypes.GoogleFolder)
                .ToArray();
        }

        public async Task<byte[]> DownloadFile(FileMetadata file)
        {
            byte[] outputBuffer = await Explorer.DownloadFileAsync(file.ID);
            return outputBuffer;
        }
    }
}

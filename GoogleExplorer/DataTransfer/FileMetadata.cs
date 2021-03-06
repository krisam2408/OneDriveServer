using GoogleExplorer.Extensions;
using System.Linq;
using GFile = Google.Apis.Drive.v3.Data.File;

namespace GoogleExplorer.DataTransfer
{
    public class FileMetadata
    {

        public string ID { get; set; }
        public string Name { get; set; }
        public string[] ParentFolder { get; set; }
        public MimeTypes MimeType { get; set; }
        public PermissionMetadata[] Permissions  { get; set; }
        public byte[] FileBuffer { get; set; }
        public bool IsShared
        {
            get
            {
                if (Permissions != null && Permissions.Length > 0)
                    return true;
                return false;
            }
        }
        public int BufferSize
        {
            get
            {
                if (FileBuffer == null)
                    return 0;
                return FileBuffer.Length;
            }
        }

        public FileMetadata() 
        {
            FileBuffer = null;
        }

        public FileMetadata(GFile gFile) : this()
        {
            ID = gFile.Id;
            Name = gFile.Name;
            ParentFolder = gFile.Parents.ToArray();
            MimeType = gFile.MimeType.GetMimeTypes();
            Permissions = gFile.Permissions
                .Select(p => new PermissionMetadata(p))
                .ToArray();
        }

        public bool ContainsEmailAddressPermission(string emailAddress)
        {
            if(IsShared)
                foreach (PermissionMetadata p in Permissions)
                    if (p.EmailAddress == emailAddress)
                        return true;

            return false;
        }
    }
}

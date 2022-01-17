using Google.Apis.Drive.v3.Data;

namespace GoogleExplorer.DataTransfer
{
    public class PermissionMetadata
    {
        public string EmailAddress { get; set; }
        public Permissions Permission { get; set; }

        public PermissionMetadata() { }

        public PermissionMetadata(Permission permission)
        {
            EmailAddress = permission.EmailAddress;
            Permission = permission.Role switch
            {
                "writer" => Permissions.Writer,
                "owner" => Permissions.Owner,
                _ => Permissions.Reader
            };
        }
    }
}

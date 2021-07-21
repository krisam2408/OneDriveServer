using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleExplorer.DataTransfer
{
    public class FileMetadata
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string[] ParentFolder { get; set; }
        public MimeTypes MimeType { get; set; }
    }
}

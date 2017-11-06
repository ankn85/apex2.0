using System;

namespace Apex.Data.Entities.Blobs
{
    public class Blob : BaseEntity
    {
        public string FileName { get; set; }

        public string FileExtension { get; set; }

        public int FileSize { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public string ContentType { get; set; }

        public string Path { get; set; }

        public DateTime? CreatedOn { get; set; }
    }
}

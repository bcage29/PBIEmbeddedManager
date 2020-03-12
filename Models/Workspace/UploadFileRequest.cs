using System;

namespace Models.Workspace
{
    public class UploadFileRequest
    {
        public Guid GroupId { get; set; }

        public string FilePath { get; set; }

        public string DatasetName { get; set; }

        public bool Overwrite { get; set; }
    }
}

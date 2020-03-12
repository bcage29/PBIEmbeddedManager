using System;

namespace Models.Workspace
{
    public class DeleteReportRequest
    {
        public Guid GroupId { get; set; }
        public Guid ReportId { get; set; }
    }
}

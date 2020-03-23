namespace Shoplify.Services.Models.Report
{
    using System;

    public class ReportViewServiceModel
    {
        public string Id { get; set; }

        public string ReportingUserId { get; set; }

        public string ReportedUserId { get; set; }

        public string ReportedAdvertisementId { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}

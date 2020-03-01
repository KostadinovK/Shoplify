namespace Shoplify.Services.Models.Report
{
    public class ReportCreateServiceModel
    {
        public string ReportingUserId { get; set; }

        public string ReportedUserId { get; set; }

        public string ReportedAdvertisementId { get; set; }

        public string Description { get; set; }
    }
}

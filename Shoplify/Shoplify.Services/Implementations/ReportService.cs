using Shoplify.Domain;

namespace Shoplify.Services.Implementations
{
    using System;
    using System.Threading.Tasks;

    using Shoplify.Services.Interfaces;
    using Shoplify.Services.Models.Report;
    using Shoplify.Web.Data;

    public class ReportService : IReportService
    {
        private readonly ShoplifyDbContext context;

        public ReportService(ShoplifyDbContext context)
        {
            this.context = context;
        }

        public async Task CreateAsync(ReportCreateServiceModel report)
        {
            var reportModel = new Report
            {
                ReportedUserId = report.ReportedUserId,
                ReportingUserId = report.ReportingUserId,
                ReportedAdvertisementId = report.ReportedAdvertisementId,
                Description = report.Description,
                IsApprovedByAdmin = false,
                ReportedOn = DateTime.UtcNow
            };

            await context.Reports.AddAsync(reportModel);
            await context.SaveChangesAsync();
        }
    }
}

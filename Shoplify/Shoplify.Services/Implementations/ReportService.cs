namespace Shoplify.Services.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Shoplify.Common;
    using Shoplify.Domain;
    using Shoplify.Services.Interfaces;
    using Shoplify.Services.Models.Report;
    using Shoplify.Web.Data;

    public class ReportService : IReportService
    {
        private const string InvalidIdErrorMesage = "Report with this id does not exist!";

        private readonly ShoplifyDbContext context;
        private readonly IAdvertisementService advertisementService;

        public ReportService(ShoplifyDbContext context, IAdvertisementService advertisementService)
        {
            this.context = context;
            this.advertisementService = advertisementService;
        }

        public async Task CreateAsync(ReportCreateServiceModel report)
        {
            var reportModel = new Report
            {
                ReportedUserId = report.ReportedUserId,
                ReportingUserId = report.ReportingUserId,
                ReportedAdvertisementId = report.ReportedAdvertisementId,
                Description = report.Description,
                IsArchived = false,
                ReportedOn = DateTime.UtcNow
            };

            await context.Reports.AddAsync(reportModel);
            await context.SaveChangesAsync();
        }

        public async Task<bool> ApproveByIdAsync(string id)
        {
            var report = context.Reports.SingleOrDefault(r => r.Id == id);

            if (report == null)
            {
                return false;
            }

            await advertisementService.BanByIdAsync(report.ReportedAdvertisementId);

            await ArchiveByIdAsync(report.Id);

            return true;
        }

        public async Task<bool> RejectByIdAsync(string id)
        {
            var report = context.Reports.SingleOrDefault(r => r.Id == id);

            if (report == null)
            {
                return false;
            }

            await ArchiveByIdAsync(report.Id);

            return true;
        }

        public async Task<bool> ArchiveByIdAsync(string id)
        {
            var report = context.Reports.SingleOrDefault(r => r.Id == id);

            if (report == null)
            {
                return false;
            }

            report.IsArchived = true;

            context.Update(report);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<int> GetAllUnArchivedCountAsync()
        {
            return await context.Reports.CountAsync(r => !r.IsArchived);
        }

        public async Task<IEnumerable<ReportViewServiceModel>> GetAllUnArchivedAsync(int page, int reportsPerPage)
        {
            return await context.Reports
                .Where(r => !r.IsArchived)
                .OrderBy(r => r.ReportedOn)
                .Select(r =>
                    new ReportViewServiceModel
                    {
                        Id = r.Id,
                        CreatedOn = r.ReportedOn.ToLocalTime(),
                        Description = r.Description,
                        ReportedAdvertisementId = r.ReportedAdvertisementId,
                        ReportedUserId = r.ReportedUserId,
                        ReportingUserId = r.ReportingUserId
                    })
                .Take(page * reportsPerPage)
                .Skip((page - 1) * reportsPerPage)
                .ToListAsync();
        }

        public async Task<ReportViewServiceModel> GetByIdAsync(string id)
        {
            var report = await context.Reports.SingleOrDefaultAsync(r => r.Id == id);

            if (report == null)
            {
                throw new ArgumentException(InvalidIdErrorMesage);
            }

            return new ReportViewServiceModel
            {
                Id = report.Id,
                CreatedOn = report.ReportedOn.ToLocalTime(),
                Description = report.Description,
                ReportedAdvertisementId = report.ReportedAdvertisementId,
                ReportingUserId = report.ReportingUserId,
                ReportedUserId = report.ReportedUserId
            };
        }
    }
}

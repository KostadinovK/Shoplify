using System.Collections.Generic;

namespace Shoplify.Services.Interfaces
{
    using System.Threading.Tasks;

    using Shoplify.Services.Models.Report;

    public interface IReportService
    {
        Task CreateAsync(ReportCreateServiceModel report);

        Task<bool> ApproveByIdAsync(string id);

        Task<bool> RejectByIdAsync(string id);

        Task<bool> ArchiveByIdAsync(string id);

        Task<int> GetAllUnArchivedCountAsync();

        Task<IEnumerable<ReportViewServiceModel>> GetAllUnArchivedAsync(int page, int reportsPerPage);

        Task<ReportViewServiceModel> GetByIdAsync(string id);
    }
}

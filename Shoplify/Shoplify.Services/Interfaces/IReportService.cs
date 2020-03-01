namespace Shoplify.Services.Interfaces
{
    using System.Threading.Tasks;

    using Shoplify.Services.Models.Report;

    public interface IReportService
    {
        Task CreateAsync(ReportCreateServiceModel report);
    }
}

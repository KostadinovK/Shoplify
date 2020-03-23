namespace Shoplify.Web.Areas.Administration.ViewModels.Report
{
    using System.Collections.Generic;

    public class ReportListingViewModel
    {
        public ICollection<ReportViewModel> Reports { get; set; } = new List<ReportViewModel>();

        public int TotalReportsCount { get; set; }

        public int CurrentPage { get; set; }

        public int LastPage { get; set; }
    }
}

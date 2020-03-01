namespace Shoplify.Web.BindingModels.Report
{
    using System.ComponentModel.DataAnnotations;
    using Shoplify.Common;

    public class CreateBindingModel
    {
        public string ReportingUserId { get; set; }

        public string ReportedUserId { get; set; }

        public string ReportedAdvertisementId { get; set; }

        [MaxLength(AttributesConstraints.ReportDescriptionMaxLength)]
        public string Description { get; set; }
    }
}

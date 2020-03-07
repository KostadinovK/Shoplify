namespace Shoplify.Web.BindingModels.Advertisement
{
    using System.ComponentModel.DataAnnotations;

    public class PromoteBindingModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string PromotedDays { get; set; }
    }
}

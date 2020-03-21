namespace Shoplify.Web.Areas.Administration.BindingModels.Advertisement
{
    using System.ComponentModel.DataAnnotations;

    public class PromoteBindingModel
    {
        [Required]
        public string Id { get; set; }

        [Required] 
        public string Days { get; set; }
    }
}

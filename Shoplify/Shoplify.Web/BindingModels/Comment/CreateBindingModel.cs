namespace Shoplify.Web.BindingModels.Comment
{
    using System.ComponentModel.DataAnnotations;

    using Shoplify.Common;

    public class CreateBindingModel
    {
        [Required]
        [MaxLength(AttributesConstraints.CommentTextMaxLength)]
        public string Text { get; set; }

        [Required]
        public string AdvertisementId { get; set; }
    }
}

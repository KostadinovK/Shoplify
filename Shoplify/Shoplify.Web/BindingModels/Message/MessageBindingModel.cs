namespace Shoplify.Web.BindingModels.Message
{
    using System.ComponentModel.DataAnnotations;

    using Shoplify.Common;

    public class MessageBindingModel
    {
        [Required]
        [MinLength(AttributesConstraints.MessageMinLength, ErrorMessage = AttributesErrorMessages.MessageInvalidLength)]
        public string Text { get; set; }

        [Required]
        public string SenderId { get; set; }

        [Required]
        public string ReceiverId { get; set; }

        [Required]
        public string ConversationId { get; set; }
    }
}

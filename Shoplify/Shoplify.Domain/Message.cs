using Shoplify.Common;

namespace Shoplify.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Message
    {
        public string Id { get; set; }

        [Required]
        public string SenderId { get; set; }

        public User Sender { get; set; }

        [Required]
        public string ReceiverId { get; set; }

        public User Receiver { get; set; }

        [Required]
        public DateTime SendOn { get; set; }

        [Required]
        [StringLength(AttributesConstraints.MessageTextMaxLength)]
        public string Text { get; set; }
    }
}

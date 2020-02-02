namespace Shoplify.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Common;

    public class Comment
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(AttributesConstraints.CommentTextMaxLength)]
        public string Text { get; set; }

        [Required]
        public string AdvertisementId { get; set; }

        public Advertisement Advertisement { get; set; }

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }

        [Required]
        public DateTime WrittenOn { get; set; }

        public DateTime? EditedOn { get; set; }
    }
}

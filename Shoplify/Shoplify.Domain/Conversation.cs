namespace Shoplify.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Conversation
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string FirstUserId { get; set; }

        public User FirstUser { get; set; }

        [Required]
        public string SecondUserId { get; set; }

        public User SecondUser { get; set; }

        [Required]
        public DateTime StartedOn { get; set; }

        public bool IsReadByFirstUser { get; set; }

        public bool IsReadBySecondUser { get; set; }

        [Required]
        public string AdvertisementId { get; set; }

        public Advertisement Advertisement { get; set; }

        public ICollection<Message> Messages { get; set; } = new HashSet<Message>();
    }
}

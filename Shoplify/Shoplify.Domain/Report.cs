namespace Shoplify.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Common;

    public class Report
    {
        public string Id { get; set; }

        [Required]
        public string ReportingUserId { get; set; }

        public User ReportingUser { get; set; }

        [Required]
        public string ReportedUserId { get; set; }

        public User ReportedUser { get; set; }

        [Required]
        public string ReportedAdvertisementId { get; set; }

        public Advertisement ReportedAdvertisement { get; set; }

        [MaxLength(AttributesConstraints.ReportDescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        public DateTime ReportedOn { get; set; }

        public bool IsApprovedByAdmin { get; set; }

        public DateTime? ApprovedOn { get; set; }
    }
}

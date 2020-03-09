namespace Shoplify.Services.Models.Notification
{
    using System;

    public class NotificationServiceModel
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public string ActionLink { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}

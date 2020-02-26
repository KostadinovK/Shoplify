namespace Shoplify.Services.Models.Comment
{
    using System;
    
    public class CreateServiceModel
    {
        public string UserId { get; set; }

        public string Text { get; set; }

        public DateTime WrittenOn { get; set; }

        public string AdvertisementId { get; set; }
    }
}

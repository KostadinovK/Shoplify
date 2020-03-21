namespace Shoplify.Web.Areas.Administration.ViewModels.Advertisement
{
    using System;

    public class AdvertisementViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string OwnerId { get; set; }

        public string OwnerName { get; set; }

        public int Views { get; set; }

        public string CreatedOn { get; set; }

        public bool IsArchived { get; set; }

        public string ArchivedOn { get; set; }

        public bool IsBanned { get; set; }

        public string BannedOn { get; set; }

        public bool IsPromoted { get; set; }

        public string PromotedUntil { get; set; }
    }
}

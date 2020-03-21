﻿namespace Shoplify.Web.Areas.Administration.ViewModels.Advertisement
{
    using System;

    public class AdvertisementViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string OwnerId { get; set; }

        public string OwnerName { get; set; }

        public int Views { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsArchived { get; set; }

        public DateTime ArchivedOn { get; set; }

        public bool IsBanned { get; set; }

        public DateTime BannedOn { get; set; }

        public bool IsPromoted { get; set; }

        public DateTime PromotedUntil { get; set; }
    }
}

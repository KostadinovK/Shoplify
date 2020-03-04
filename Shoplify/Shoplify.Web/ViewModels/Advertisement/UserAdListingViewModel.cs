namespace Shoplify.Web.ViewModels.Advertisement
{
    public class UserAdListingViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string CategoryName { get; set; }

        public string SubCategoryName { get; set; }

        public string CreatedOn { get; set; }

        public string ArchivedOn { get; set; }

        public int Views { get; set; }

        public string PromotedOn { get; set; }

        public string PromotedUntil { get; set; }

        public bool IsPromoted { get; set; }
    }
}

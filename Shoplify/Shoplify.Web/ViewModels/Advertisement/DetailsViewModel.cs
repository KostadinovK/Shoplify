namespace Shoplify.Web.ViewModels.Advertisement
{
    using System.Collections.Generic;

    public class DetailsViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string SubCategoryId { get; set; }

        public string UserId { get; set; }

        public string Username { get; set; }

        public string SubCategoryName { get; set; }

        public string CreatedOn { get; set; }

        public string TownName { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public List<string> Images { get; set; } = new List<string>();

        public string Phone { get; set; }

        public string Condition { get; set; }
    }
}

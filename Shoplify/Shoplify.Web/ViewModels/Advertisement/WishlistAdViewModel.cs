using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shoplify.Web.ViewModels.Advertisement
{
    public class WishlistAdViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public decimal Price { get; set; }

        public string Category { get; set; }

        public string CreatedOn { get; set; }
    }
}

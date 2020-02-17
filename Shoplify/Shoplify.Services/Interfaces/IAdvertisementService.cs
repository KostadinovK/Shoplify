namespace Shoplify.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using Shoplify.Services.Models;

    public interface IAdvertisementService
    {
        Task CreateAsync(AdvertisementServiceModel advertisement);
    }
}

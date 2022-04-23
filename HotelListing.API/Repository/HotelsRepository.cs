using HotelListing.API.Contracts;
using HotelListing.API.Data;

namespace HotelListing.API.Repository;

class HotelsRepository : GenericRepository<Hotel>, IHotelsRepository
{
    public HotelsRepository(HotelListingDbContext context) : base(context)
    {
    }
}
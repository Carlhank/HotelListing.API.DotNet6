using HotelListing.API.Models.Hotel;

namespace HotelListing.API.Models.Country;

public class CountryDto : BaseCountryDto
{
    public int Id { get; set; }
    public IEnumerable<HotelDto> Hotels { get; set; }
}
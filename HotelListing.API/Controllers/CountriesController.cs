using HotelListing.API.Core.Contracts;
using HotelListing.API.Core.Exceptions;
using HotelListing.API.Core.Models;
using HotelListing.API.Core.Models.Country;
using HotelListing.API.Core.Models.Hotel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Data;
using Microsoft.AspNetCore.Authorization;

namespace HotelListing.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/countries")]
[ApiVersion("1.0", Deprecated = true)]
public class CountriesController : ControllerBase
{
    private readonly ICountriesRepository _countriesRepository;
    private readonly ILogger<CountriesController> _logger;

    public CountriesController(ICountriesRepository countriesRepository,
        ILogger<CountriesController> logger)
    {
        _countriesRepository = countriesRepository;
        _logger = logger;
    }

    // GET: api/Countries
    [HttpGet("GetAll")]
    public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
    {
        var countries = await _countriesRepository.GetAllAsync<GetCountryDto>();
        return Ok(countries);
    }
    
    // GET: api/Countries?StartIndex=0&pageSize=25&pageNumber=1
    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetPagedCountries(
        [FromQuery] QueryParameters queryParameters)
    {
        var pagedCountriesResult = await _countriesRepository.GetAllAsync<GetCountryDto>(queryParameters);
        return Ok(pagedCountriesResult);
    }

    // GET: api/Countries/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CountryDto>> GetCountry(int id)
    {
        var country = await _countriesRepository.GetDetails(id);
        return Ok(country);
    }

    // PUT: api/Countries/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountryDto)
    {
        if (id != updateCountryDto.Id)
        {
            return BadRequest("Invalid Record Id"); 
        }
        
        try
        {
            await _countriesRepository.UpdateAsync(id, updateCountryDto);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _countriesRepository.Exists(id))
            {
                throw new NotFoundException(nameof(PutCountry), id);
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Countries
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Country>> PostCountry(CreateCountryDto createCountryDto)
    {
        var country = await _countriesRepository.AddAsync<CreateCountryDto, GetCountryDto>(createCountryDto);
        return CreatedAtAction(nameof(GetCountry), new { id = country.Id }, createCountryDto);
    }

    // DELETE: api/Countries/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteCountry(int id)
    {
        await _countriesRepository.DeleteAsync(id);
        return NoContent();
    }
}
using AutoMapper;
using HotelListing.API.Core.Contracts;
using HotelListing.API.Core.Exceptions;
using HotelListing.API.Core.Models.Country;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OData.Query;

namespace HotelListing.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/countries")]
[ApiVersion("2.0")]
public class CountriesV2Controller : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ICountriesRepository _countriesRepository;
    private readonly ILogger<CountriesV2Controller> _logger;

    public CountriesV2Controller(IMapper mapper,
        ICountriesRepository countriesRepository,
        ILogger<CountriesV2Controller> logger)
    {
        _mapper = mapper;
        _countriesRepository = countriesRepository;
        _logger = logger;
    }

    // GET: api/Countries
    [HttpGet]
    [EnableQuery]
    public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
    {
        var countries = await _countriesRepository.GetAllAsync();
        var records = _mapper.Map<IEnumerable<GetCountryDto>>(countries);
        return Ok(records);
    }

    // GET: api/Countries/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CountryDto>> GetCountry(int id)
    {
        var country = await _countriesRepository.GetDetails(id);

        if (country == null)
        {
            throw new NotFoundException(nameof(GetCountry), id);
        }

        return Ok(_mapper.Map<CountryDto>(country));
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

        var country = await _countriesRepository.GetAsync(id);

        if (country == null)
        {
            throw new NotFoundException(nameof(PutCountry), id);
        }

        _mapper.Map(updateCountryDto, country);
        
        try
        {
            await _countriesRepository.UpdateAsync(country);
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
    public async Task<ActionResult<Country>> PostCountry(CreateCountryDto countryDto)
    {
        var country = _mapper.Map<Country>(countryDto);
        await _countriesRepository.AddAsync(country);

        return CreatedAtAction("GetCountry", new { id = country.Id }, countryDto);
    }

    // DELETE: api/Countries/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteCountry(int id)
    {
        var country = await _countriesRepository.GetAsync(id);
        if (country == null)
        {
            throw new NotFoundException(nameof(DeleteCountry), id);
        }

        await _countriesRepository.DeleteAsync(id);

        return NoContent();
    }
}
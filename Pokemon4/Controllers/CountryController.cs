using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pokemon4.Dto;
using Pokemon4.Interfaces;
using Pokemon4.Models;
using Pokemon4.Repositories;

namespace Pokemon4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository countryRepository, IOwnerRepository ownerRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _ownerRepository = ownerRepository;
            _mapper = mapper;
        }

        [HttpGet("get-countries")]
        public IActionResult GetCountries()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var countriesMap = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());

            return Ok(countriesMap);
        }


        [HttpGet("get-country/{countryId}")]
        public IActionResult GetCountry(int countryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_countryRepository.CountryExists(countryId))
            {
                return NotFound();
            }

            var countryMap = _mapper.Map<CountryDto>(_countryRepository.GetCountry(countryId));

            return Ok(countryMap);
        }

        [HttpGet("get-country-by-owner/{ownerId}")]
        public IActionResult GetCountryByOwner(int ownerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_ownerRepository.OwnerExists(ownerId))
            {
                return NotFound();
            }

            var countryMap = _mapper.Map<CountryDto>(_countryRepository.GetCountryByOwner(ownerId));

            return Ok(countryMap);
        }

        [HttpGet("get-owners-from-a-country/{countryId}")]
        public IActionResult GetOwnersFromACountry(int countryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_countryRepository.CountryExists(countryId))
            {
                return NotFound();
            }

            var ownersMap = _mapper.Map<List<OwnerDto>>(_countryRepository.GetOwnersFromACountry(countryId));

            return Ok(ownersMap);
        }

        [HttpPost("create-country")]
        public IActionResult CreateCountry([FromBody] CountryDto countryToCreate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (countryToCreate == null)
            {
                return BadRequest(ModelState);
            }

            var countryToCheck = _countryRepository.GetCountries().Where(c => c.Name.Trim().ToUpper() == countryToCreate.Name.TrimEnd().ToUpper()).FirstOrDefault();

            if (countryToCheck != null)
            {
                ModelState.AddModelError("", "Country with the same name already exists");
                return StatusCode(422, ModelState);
            }

            var countryMap = _mapper.Map<Country>(countryToCreate);

            if (!_countryRepository.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong creating country");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created country");
        }

        [HttpPut("update-country/{countryId}")]
        [ProducesResponseType(500)]
        public IActionResult UpdateCountry(int countryId, [FromBody] CountryDto countryToUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (countryToUpdate == null)
            {
                return BadRequest(ModelState);
            }

            if (!_countryRepository.CountryExists(countryId))
            {
                return NotFound();
            }

            if (countryId != countryToUpdate.Id)
            {
                return BadRequest(ModelState);
            }

            var countryMap = _mapper.Map<Country>(countryToUpdate);

            if (!_countryRepository.UpdateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something went wrong updating country");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated country");
        }

        [HttpDelete("delete-country/{countryId}")]
        [ProducesResponseType(500)]
        public IActionResult DeleteCountry(int countryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_countryRepository.CountryExists(countryId))
            {
                return NotFound();
            }

            var countryToDelete = _countryRepository.GetCountry(countryId);

            if (!_countryRepository.DeleteCountry(countryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting country");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted country");
        }
    }
}

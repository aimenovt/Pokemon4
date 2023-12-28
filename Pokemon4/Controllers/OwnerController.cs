using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pokemon4.Dto;
using Pokemon4.Interfaces;
using Pokemon4.Models;
using Pokemon4.Repositories;

namespace Pokemon4.Controllers
{
    [Route("apo/[controller]")]
    [ApiController]
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public OwnerController(IOwnerRepository ownerRepository, IPokemonRepository pokemonRepository, ICountryRepository countryRepository, IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _pokemonRepository = pokemonRepository;
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet("get-owners")]
        public IActionResult GetOwners()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ownersMap = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwners());

            return Ok(ownersMap);
        }


        [HttpGet("get-owner/{ownerId}")]
        public IActionResult GetOwner(int ownerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_ownerRepository.OwnerExists(ownerId))
            {
                return NotFound();
            }

            var ownerMap = _mapper.Map<OwnerDto>(_ownerRepository.GetOwner(ownerId));

            return Ok(ownerMap);
        }

        [HttpGet("get-pokemons-by-owner/{ownerId}")]
        public IActionResult GetPokemonsByOwner(int ownerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_ownerRepository.OwnerExists(ownerId))
            {
                return NotFound();
            }

            var pokemonsMap = _mapper.Map<List<PokemonDto>>(_ownerRepository.GetPokemonsByOwner(ownerId));  

            return Ok(pokemonsMap);
        }

        [HttpGet("get-owners-of-a-pokemon/{pokemonId}")]
        public IActionResult GetOwnersOfAPokemon(int pokemonId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_pokemonRepository.PokemonExists(pokemonId))
            {
                return NotFound();
            }

            var ownersMap = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwnersOfAPokemon(pokemonId));

            return Ok(ownersMap);
        }

        [HttpPost("create-owner/{countryId}")]
        public IActionResult CreateOwner(int countryId, [FromBody] OwnerDto ownerToCreate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (ownerToCreate == null)
            {
                return BadRequest(ModelState);
            }

            if (!_countryRepository.CountryExists(countryId))
            {
                return NotFound();
            }

            var ownerToCheck = _ownerRepository.GetOwners().Where(c => c.LastName.Trim().ToUpper() == ownerToCreate.LastName.TrimEnd().ToUpper()).FirstOrDefault();

            if (ownerToCheck != null)
            {
                ModelState.AddModelError("", "Owner with the same last name already exists");
                return StatusCode(422, ModelState);
            }

            var ownerMap = _mapper.Map<Owner>(ownerToCreate);

            ownerMap.Country = _countryRepository.GetCountry(countryId);

            if (!_ownerRepository.CreateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something went wrong creating owner");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created owner");
        }

        [HttpPut("update-owner/{ownerId}")]
        public IActionResult UpdateOwner(int ownerId, [FromBody] OwnerDto ownerToUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (ownerToUpdate == null)
            {
                return BadRequest(ModelState);
            }

            if (!_ownerRepository.OwnerExists(ownerId))
            {
                return NotFound();
            }

            if (ownerId != ownerToUpdate.Id)
            {
                return BadRequest(ModelState);
            }

            var ownerMap = _mapper.Map<Owner>(ownerToUpdate);

            if (!_ownerRepository.UpdateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Something went wrong udpating owner");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated owner");
        }

        [HttpDelete("delete-owner/{ownerId}")]
        public IActionResult DeleteOwner(int ownerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_ownerRepository.OwnerExists(ownerId))
            {
                return NotFound();
            }

            var ownerToDelete = _ownerRepository.GetOwner(ownerId);

            if (!_ownerRepository.DeleteOwner(ownerToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting owner");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted owner");
        }

        [HttpPut("change-country-of-owner/{ownerId}, {newCountryId}")]
        public IActionResult ChangeCountryOfOwner(int ownerId, int newCountryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_ownerRepository.OwnerExists(ownerId))
            {
                return NotFound();
            }

            if (!_countryRepository.CountryExists(newCountryId))
            {
                return NotFound();
            }

            if (!_ownerRepository.ChangeCountryOfOwner(ownerId, newCountryId))
            {
                ModelState.AddModelError("", "Something went wrong changing country of owner");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully changed country of owner");
        }
    }
}

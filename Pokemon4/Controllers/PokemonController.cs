using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pokemon4.Dto;
using Pokemon4.Interfaces;
using Pokemon4.Models;

namespace Pokemon4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public PokemonController(IPokemonRepository pokemonRepository, IOwnerRepository ownerRepository, ICategoryRepository categoryRepository, IReviewRepository reviewRepository, IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _ownerRepository = ownerRepository;
            _categoryRepository = categoryRepository;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        [HttpGet("get-pokemons")]
        public IActionResult GetPokemons()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pokemonsMap = _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons());

            return Ok(pokemonsMap);
        }

        [HttpGet("get-pokemon/{pokemonId}")]
        public IActionResult GetPokemon(int pokemonId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_pokemonRepository.PokemonExists(pokemonId))
            {
                return NotFound();
            }

            var pokemonMap = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(pokemonId));

            return Ok(pokemonMap);
        }

        [HttpGet("get-pokemon-by-name/{pokemonName}")]
        public IActionResult GetPokemonByName(string pokemonName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_pokemonRepository.PokemonExists(_pokemonRepository.GetPokemons().Where(p => p.Name == pokemonName).FirstOrDefault().Id))
            {
                return NotFound();
            }

            var pokemonMap = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemonByName(pokemonName));

            return Ok(pokemonMap);
        }

        [HttpGet("get-pokemon-rating/{pokemonId}")]
        public IActionResult GetPokemonRating(int pokemonId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_pokemonRepository.PokemonExists(pokemonId))
            {
                return NotFound();
            }

            var rating = _pokemonRepository.GetPokemonRating(pokemonId);

            return Ok(rating);
        }

        [HttpPost("create-pokemon/{ownerId}, {categoryId}")]
        public IActionResult CreatePokemon(int ownerId, int categoryId, [FromBody] PokemonDto pokemonToCreate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_ownerRepository.OwnerExists(ownerId))
            {
                return NotFound();
            }

            if (!_categoryRepository.CategoryExists(categoryId))
            {
                return NotFound();
            }

            if (pokemonToCreate == null)
            {
                return BadRequest(ModelState);
            }

            var pokemonToCheck = _pokemonRepository.GetPokemons().Where(p => p.Name.Trim().ToUpper() == pokemonToCreate.Name.TrimEnd().ToUpper()).FirstOrDefault();

            if (pokemonToCheck != null)
            {
                ModelState.AddModelError("", "Pokemon with the same name already exists");
                return StatusCode(422, ModelState);
            }

            var pokemonMap = _mapper.Map<Pokemon>(pokemonToCreate);

            if (!_pokemonRepository.CreatePokemon(ownerId, categoryId, pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong creating pokemon");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created pokemon");
        }

        [HttpPut("update-pokemon/{pokemonId}")]
        public IActionResult UpdatePokemon(int pokemonId, PokemonDto pokemonToUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_pokemonRepository.PokemonExists(pokemonId))
            {
                return NotFound();
            }

            if (pokemonId != pokemonToUpdate.Id)
            {
                return BadRequest(ModelState);
            }

            if (pokemonToUpdate == null)
            {
                return NotFound();
            }

            var pokemonMap = _mapper.Map<Pokemon>(pokemonToUpdate);

            if (!_pokemonRepository.UpdatePokemon(pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong updating pokemon");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated pokemon");
        }

        [HttpDelete("delete-pokemon/{pokemonId}")]
        public IActionResult DeletePokemon(int pokemonId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_pokemonRepository.PokemonExists(pokemonId))
            {
                return NotFound();
            }

            var pokemonToDelete = _pokemonRepository.GetPokemon(pokemonId);
            var reviewsToDelete = _reviewRepository.GetReviews().Where(r => r.Pokemon == pokemonToDelete).ToList();

            if (reviewsToDelete.Count > 0)
            {
                if (!_reviewRepository.DeleteReviews(reviewsToDelete))
                {
                    ModelState.AddModelError("", "Something went wrong deleting reviews");
                    return StatusCode(500, ModelState);
                }
            }

            if (!_pokemonRepository.DeletePokemon(pokemonToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting pokemon");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted pokemon and associated reviews");
        }

        [HttpPost("add-pokemon-category/{pokemonId}, {addingCategoryId}")]
        public IActionResult AddPokemonCategory(int pokemonId, int addingCategoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_pokemonRepository.PokemonExists(pokemonId))
            {
                return NotFound();
            }

            if (!_categoryRepository.CategoryExists(addingCategoryId))
            {
                return NotFound();
            }

            if (!_pokemonRepository.AddPokemonCategory(pokemonId, addingCategoryId))
            {
                ModelState.AddModelError("", "Something went wrong adding pokemon to category");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully added pokemon to category");
        }

        [HttpDelete("removing-pokemon-category/{pokemonId}, {removingCategoryId}")]
        public IActionResult RemovingPokemonCategory(int pokemonId, int removingCategoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_pokemonRepository.PokemonExists(pokemonId))
            {
                return NotFound();
            }

            if (!_categoryRepository.CategoryExists(removingCategoryId))
            {
                return NotFound();
            }

            if (!_pokemonRepository.RemovingPokemonCategory(pokemonId, removingCategoryId))
            {
                ModelState.AddModelError("", "Something went wrong removing pokemon from category");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully removed pokemon from category");
        }

        [HttpPost("add-pokemon-owner/{pokemonId}, {addingOwnerId}")]
        public IActionResult AddPokemonOwner(int pokemonId, int addingOwnerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_pokemonRepository.PokemonExists(pokemonId))
            {
                return NotFound();
            }

            if (!_ownerRepository.OwnerExists(addingOwnerId))
            {
                return NotFound();
            }

            if (!_pokemonRepository.AddPokemonOwner(pokemonId, addingOwnerId))
            {
                ModelState.AddModelError("", "Something went wrong adding pokemon to owner");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully added pokemon to owner");
        }

        [HttpDelete("removing-pokemon-owner/{pokemonId}, {removingOwnerId}")]
        public IActionResult RemovingPokemonOwner(int pokemonId, int removingOwnerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_pokemonRepository.PokemonExists(pokemonId))
            {
                return NotFound();
            }

            if (!_ownerRepository.OwnerExists(removingOwnerId))
            {
                return NotFound();
            }

            if (!_pokemonRepository.RemovingPokemonOwner(pokemonId, removingOwnerId))
            {
                ModelState.AddModelError("", "Something went wrong removing pokemon from owner");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully removed pokemon from owner");
        }
    }
}

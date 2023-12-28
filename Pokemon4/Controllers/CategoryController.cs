using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pokemon4.Dto;
using Pokemon4.Interfaces;
using Pokemon4.Models;

namespace Pokemon4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet("get-categories")]
        public IActionResult GetCategories()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoriesMap = _mapper.Map<List<CategoryDto>>(_categoryRepository.GetCategories());

            return Ok(categoriesMap);
        }

        [HttpGet("get-category/{categoryId}")]
        public IActionResult GetCategory(int categoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_categoryRepository.CategoryExists(categoryId))
            {
                return NotFound();
            }

            var categoryMap = _mapper.Map<CategoryDto>(_categoryRepository.GetCategory(categoryId));

            return Ok(categoryMap);
        }

        [HttpGet("get-pokemons-by-category/{categoryId}")]
        public IActionResult GetPokemonsByCategory(int categoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_categoryRepository.CategoryExists(categoryId))
            {
                return NotFound();
            }

            var pokemonsMap = _mapper.Map<List<PokemonDto>>(_categoryRepository.GetPokemonsByCategory(categoryId));

            return Ok(pokemonsMap);
        }

        [HttpPost("create-category")]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreateCategory([FromBody] CategoryDto categoryToCreate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (categoryToCreate == null)
            {
                return BadRequest(ModelState);
            }

            var categoryToCheck = _categoryRepository.GetCategories().Where(c => c.Name.Trim().ToUpper() == categoryToCreate.Name.TrimEnd().ToUpper()).FirstOrDefault();

            if (categoryToCheck != null)
            {
                ModelState.AddModelError("", "Category with the same name already exists");
                return StatusCode(422, ModelState);
            }

            var categoryMap = _mapper.Map<Category>(categoryToCreate);

            if (!_categoryRepository.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong creating category");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created category");
        }

        [HttpPut("update-category/{categoryId}")]
        [ProducesResponseType(500)]
        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto categoryToUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (categoryToUpdate == null)
            {
                return BadRequest(ModelState);
            }

            if (!_categoryRepository.CategoryExists(categoryId))
            {
                return NotFound();
            }

            if (categoryId != categoryToUpdate.Id)
            {
                return BadRequest(ModelState);
            }

            var categoryMap = _mapper.Map<Category>(categoryToUpdate);

            if (!_categoryRepository.UpdateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong updating category");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated category");
        }

        [HttpDelete("delete-category/{categoryId}")]
        [ProducesResponseType(500)]
        public IActionResult DeleteCategory(int categoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_categoryRepository.CategoryExists(categoryId))
            {
                return NotFound();
            }

            var categoryToDelete = _categoryRepository.GetCategory(categoryId);

            if (!_categoryRepository.DeleteCategory(categoryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting category");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted category");
        }
    }
}

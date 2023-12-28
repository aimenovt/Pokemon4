using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pokemon4.Dto;
using Pokemon4.Interfaces;
using Pokemon4.Models;

namespace Pokemon4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;

        public ReviewController(IReviewRepository reviewRepository, IReviewerRepository reviewerRepository, IPokemonRepository pokemonRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _reviewerRepository = reviewerRepository;
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
        }

        [HttpGet("get-reviews")]
        public IActionResult GetReviews()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  
            }

            var reviewsMap = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviews());
            
            return Ok(reviewsMap);
        }

        [HttpGet("get-review/{reviewId}")]
        public IActionResult GetReview(int reviewId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }

            var reviewMap = _mapper.Map<ReviewDto>(_reviewRepository.GetReview(reviewId));

            return Ok(reviewMap);
        }

        [HttpGet("get-reviews-of-a-pokemon/{pokemonId}")]
        public IActionResult GetReviewsOfAPokemon(int pokemonId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_pokemonRepository.PokemonExists(pokemonId))
            {
                return NotFound();
            }

            var reviewsMap = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviewsOfAPokemon(pokemonId));

            return Ok(reviewsMap);
        }

        [HttpPost("create-review")]
        public IActionResult CreateReview(int pokemonId, int reviewerId, [FromBody] ReviewDto reviewToCreate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (reviewToCreate == null)
            {
                return BadRequest(ModelState);
            }

            if (!_pokemonRepository.PokemonExists(pokemonId))
            {
                return NotFound();
            }

            if (!_reviewerRepository.ReviewerExists(reviewerId))
            {
                return NotFound();
            }

            var reviewToCheck = _reviewRepository.GetReviews().Where(r => r.Title.Trim().ToUpper() == reviewToCreate.Title.TrimEnd().ToUpper()).FirstOrDefault();

            if (reviewToCheck != null)
            {
                ModelState.AddModelError("", "Review with the same title already exists");
                return StatusCode(422, ModelState);
            }

            var reviewMap = _mapper.Map<Review>(reviewToCreate);
            reviewMap.Pokemon = _pokemonRepository.GetPokemon(pokemonId);
            reviewMap.Reviewer = _reviewerRepository.GetReviewer(reviewerId);

            if (!_reviewRepository.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong creating review");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created review");
        }

        [HttpPut("update-review/{reviewId}")]
        public IActionResult UpdateReview(int reviewId, [FromBody] ReviewDto reviewToUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (reviewToUpdate == null)
            {
                return BadRequest(ModelState);
            }

            if (reviewId != reviewToUpdate.Id)
            {
                return BadRequest(ModelState);
            }

            if (!_reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }

            var reviewMap = _mapper.Map<Review>(reviewToUpdate);

            if (!_reviewRepository.UpdateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong updating review");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated review");
        }

        [HttpDelete("delete-review/{reviewId}")]
        public IActionResult DeleteReview(int reviewId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }

            var reviewToDelete = _reviewRepository.GetReview(reviewId);

            if (!_reviewRepository.DeleteReview(reviewToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting review");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted review");
        }

        [HttpPut("change-reviewer-of-review/{reviewId}, {newReviewerId}")]
        public IActionResult ChangeReviewerOfReview(int reviewId, int newReviewerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_reviewRepository.ReviewExists(reviewId))
            {
                return NotFound();
            }

            if (!_reviewerRepository.ReviewerExists(newReviewerId))
            {
                return NotFound();
            }

            if (!_reviewRepository.ChangeReviewerOfReview(reviewId, newReviewerId))
            {
                ModelState.AddModelError("", "Something went wrong changing reviewer of review");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully changed reviewer of review");
        }
    }
}

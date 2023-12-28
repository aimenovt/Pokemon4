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
    public class ReviewerController : Controller
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewerRepository reviewerRepository, IReviewRepository reviewRepository, IMapper mapper)
        {
            _reviewerRepository = reviewerRepository;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        [HttpGet("get-reviewers")]
        public IActionResult GetReviewers()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  
            }

            var reviewersMap = _mapper.Map<List<ReviewerDto>>(_reviewerRepository.GetReviewers());

            return Ok(reviewersMap);
        }

        [HttpGet("get-reviewer/{reviewerId}")]
        public IActionResult GetReviewer(int reviewerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_reviewerRepository.ReviewerExists(reviewerId))
            {
                return NotFound();
            }

            var reviewerMap = _mapper.Map<ReviewerDto>(_reviewerRepository.GetReviewer(reviewerId));

            return Ok(reviewerMap);
        }

        [HttpGet("get-reviews-by-reviewer/{reviewerId}")]
        public IActionResult GetReviewsByReviewer(int reviewerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_reviewerRepository.ReviewerExists(reviewerId))
            {
                return NotFound();
            }

            var reviewsMap = _mapper.Map<List<ReviewDto>>(_reviewerRepository.GetReviewsByReviewer(reviewerId));

            return Ok(reviewsMap);
        }

        [HttpPost("create-reviewer")]
        public IActionResult CreateReviewer(ReviewerDto reviewerToCreate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (reviewerToCreate == null)
            {
                return BadRequest(ModelState);
            }

            var reviewerToCheck = _reviewerRepository.GetReviewers().Where(r => r.LastName.Trim().ToUpper() == reviewerToCreate.LastName.TrimEnd().ToUpper()).FirstOrDefault();

            if (reviewerToCheck != null)
            {
                ModelState.AddModelError("", "Reviewer with the same last name already exists");
                return StatusCode(422, ModelState);
            }

            var reviewerMap = _mapper.Map<Reviewer>(reviewerToCreate);

            if (!_reviewerRepository.CreateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "Something went wrong creating reviewer");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created reviewer");
        }

        [HttpPut("update-reviewer/{reviewerId}")]
        public IActionResult UpdateReviewer(int reviewerId, [FromBody] ReviewerDto reviewerToUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (reviewerToUpdate == null)
            {
                return BadRequest(ModelState);
            }

            if (reviewerId != reviewerToUpdate.Id)
            {
                return BadRequest(ModelState);
            }

            if (!_reviewerRepository.ReviewerExists(reviewerId))
            {
                return NotFound();
            }

            var reviewerMap = _mapper.Map<Reviewer>(reviewerToUpdate);

            if (!_reviewerRepository.UpdateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "Something went wrong updating reviewer");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated reviewer");
        }

        [HttpDelete("delete-reviewer/{reviewerId}")]
        public IActionResult DeleteReviewer(int reviewerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_reviewerRepository.ReviewerExists(reviewerId))
            {
                return NotFound();
            }

            var reviewerToDelete = _reviewerRepository.GetReviewer(reviewerId);
            var reviewsToDelete = _reviewRepository.GetReviews().Where(r => r.Reviewer == reviewerToDelete).ToList();

            if (reviewsToDelete.Any())
            {
                if (!_reviewRepository.DeleteReviews(reviewsToDelete))
                {
                    ModelState.AddModelError("", "Something went wrong deleting reviews");
                    return StatusCode(500, ModelState);
                }
            }

            if (!_reviewerRepository.DeleteReviewer(reviewerToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting reviewer");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted reviewer and associated reviews");
        }
    }
}

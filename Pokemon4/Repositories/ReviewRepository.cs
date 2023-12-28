using Pokemon4.Data;
using Pokemon4.Interfaces;
using Pokemon4.Models;

namespace Pokemon4.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _context;

        public ReviewRepository(DataContext context)
        {
            _context = context;
        }

        public bool ChangeReviewerOfReview(int reviewId, int newReviewerId)
        {
            _context.Reviews.Where(r => r.Id == reviewId).FirstOrDefault().Reviewer = _context.Reviewers.Where(r => r.Id == newReviewerId).FirstOrDefault();

            return Save();
        }

        public bool CreateReview(Review review)
        {
            _context.Reviews.Add(review);

            return Save();
        }

        public bool DeleteReview(Review review)
        {
            _context.Reviews.Remove(review);

            return Save();
        }

        public bool DeleteReviews(List<Review> reviews)
        {
            _context.Reviews.RemoveRange(reviews);

            return Save();
        }

        public Review GetReview(int reviewId)
        {
            return _context.Reviews.Where(r => r.Id == reviewId).FirstOrDefault();
        }

        public ICollection<Review> GetReviews()
        {
            return _context.Reviews.OrderBy(r => r.Id).ToList();
        }

        public ICollection<Review> GetReviewsOfAPokemon(int pokemonId)
        {
            return _context.Reviews.Where(r => r.Pokemon.Id == pokemonId).ToList();
        }

        public bool ReviewExists(int reviewId)
        {
            return _context.Reviews.Any(r => r.Id == reviewId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();

            return saved > 0 ? true : false;
        }

        public bool UpdateReview(Review review)
        {
            _context.Reviews.Update(review);

            return Save();
        }
    }
}

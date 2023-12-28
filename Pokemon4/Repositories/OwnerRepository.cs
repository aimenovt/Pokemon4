using Pokemon4.Data;
using Pokemon4.Interfaces;
using Pokemon4.Models;

namespace Pokemon4.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _context;

        public OwnerRepository(DataContext context)
        {
            _context = context;
        }

        public bool ChangeCountryOfOwner(int ownerId, int newCountryId)
        {
            _context.Owners.Where(o => o.Id == ownerId).FirstOrDefault().Country = _context.Countries.Where(c => c.Id == newCountryId).FirstOrDefault();

            return Save();
        }

        public bool CreateOwner(Owner owner)
        {
            _context.Owners.Add(owner);

            return Save();
        }

        public bool DeleteOwner(Owner owner)
        {
            _context.Owners.Remove(owner);

            return Save();
        }

        public Owner GetOwner(int ownerId)
        {
            return _context.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
        }

        public ICollection<Owner> GetOwners()
        {
            return _context.Owners.OrderBy(o => o.Id).ToList();
        }

        public ICollection<Owner> GetOwnersOfAPokemon(int pokemonId)
        {
            return _context.PokemonOwners.Where(po => po.PokemonId == pokemonId).Select(po => po.Owner).ToList();
        }

        public ICollection<Pokemon> GetPokemonsByOwner(int ownerId)
        {
            return _context.PokemonOwners.Where(po => po.OwnerId == ownerId).Select(po => po.Pokemon).ToList();
        }

        public bool OwnerExists(int ownerId)
        {
            return _context.Owners.Any(o => o.Id == ownerId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateOwner(Owner owner)
        {
            _context.Owners.Update(owner);

            return Save();
        }
    }
}

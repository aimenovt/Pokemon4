using Pokemon4.Data;
using Pokemon4.Interfaces;
using Pokemon4.Models;

namespace Pokemon4.Repositories
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly DataContext _context;

        public PokemonRepository(DataContext context)
        {
            _context = context;
        }

        public bool AddPokemonCategory(int pokemonId, int addingCategoryId)
        {
            var pokemon = _context.Pokemons.Where(p => p.Id == pokemonId).FirstOrDefault();
            var category = _context.Categories.Where(c => c.Id == addingCategoryId).FirstOrDefault();

            var pokemonCategory = new PokemonCategory
            {
                Pokemon = pokemon,
                Category = category
            };

            _context.PokemonCategories.Add(pokemonCategory);

            return Save();
        }

        public bool AddPokemonOwner(int pokemonId, int addingOwnerId)
        {
            var pokemon = _context.Pokemons.Where(p => p.Id == pokemonId).FirstOrDefault();
            var owner = _context.Owners.Where(o => o.Id == addingOwnerId).FirstOrDefault();

            var pokemonOwner = new PokemonOwner
            {
                Pokemon = pokemon,
                Owner = owner
            };

            _context.PokemonOwners.Add(pokemonOwner);

            return Save();
        }

        public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            var category = _context.Categories.Where(p => p.Id == categoryId).FirstOrDefault();
            var owner = _context.Owners.Where(o => o.Id == ownerId).FirstOrDefault();

            var pokemonCategory = new PokemonCategory
            {
                Pokemon = pokemon,
                Category = category
            };

            _context.PokemonCategories.Add(pokemonCategory);

            var pokemonOwner = new PokemonOwner
            {
                Pokemon = pokemon,
                Owner = owner
            };

            _context.PokemonOwners.Add(pokemonOwner);

            _context.Pokemons.Add(pokemon);

            return Save();
        }

        public bool DeletePokemon(Pokemon pokemon)
        {
            _context.Pokemons.Remove(pokemon);

            return Save();
        }

        public Pokemon GetPokemon(int pokemonId)
        {
            return _context.Pokemons.Where(p => p.Id == pokemonId).FirstOrDefault();
        }

        public Pokemon GetPokemonByName(string pokemonName)
        {
            return _context.Pokemons.Where(p => p.Name == pokemonName).FirstOrDefault();
        }

        public decimal GetPokemonRating(int pokemonId)
        {
            var reviews = _context.Reviews.Where(r => r.Pokemon.Id == pokemonId).ToList();

            if (reviews.Count > 0)
            {
                var rating = (decimal)(reviews.Sum(r => r.Rating) / reviews.Count());
                return rating;
            }

            else
            {
                return 0;
            }
        }

        public ICollection<Pokemon> GetPokemons()
        {
            return _context.Pokemons.OrderBy(p => p.Id).ToList();
        }

        public bool PokemonExists(int pokemonId)
        {
            return _context.Pokemons.Any(p => p.Id == pokemonId);
        }

        public bool RemovingPokemonCategory(int pokemonId, int removingCategoryId)
        {
            _context.PokemonCategories.Remove(_context.PokemonCategories.Where(pc => pc.PokemonId == pokemonId && pc.CategoryId == removingCategoryId).FirstOrDefault());

            return Save();
        }

        public bool RemovingPokemonOwner(int pokemonId, int removingOwnerId)
        {
            _context.PokemonOwners.Remove(_context.PokemonOwners.Where(po => po.PokemonId == pokemonId && po.OwnerId == removingOwnerId).FirstOrDefault());

            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();

            return saved > 0 ? true : false;
        }

        public bool UpdatePokemon(Pokemon pokemon)
        {
            _context.Pokemons.Update(pokemon);

            return Save();
        }
    }
}

using Pokemon4.Models;

namespace Pokemon4.Interfaces
{
    public interface IPokemonRepository
    {
        ICollection<Pokemon> GetPokemons();
        Pokemon GetPokemon(int pokemonId);
        Pokemon GetPokemonByName(string pokemonName);
        decimal GetPokemonRating(int pokemonId);
        bool PokemonExists(int pokemonId);
        bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon);
        bool UpdatePokemon(Pokemon pokemon);
        bool DeletePokemon(Pokemon pokemon);
        bool Save();
        bool AddPokemonCategory(int pokemonId, int addingCategoryId);
        bool RemovingPokemonCategory(int pokemonId, int removingCategoryId);
        bool AddPokemonOwner(int pokemonId, int addingOwnerId);
        bool RemovingPokemonOwner(int pokemonId, int removingOwnerId);
    }
}

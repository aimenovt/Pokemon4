using Pokemon4.Models;

namespace Pokemon4.Interfaces
{
    public interface IOwnerRepository
    {
        ICollection<Owner> GetOwners();
        Owner GetOwner(int ownerId);
        ICollection<Pokemon> GetPokemonsByOwner(int ownerId);
        ICollection<Owner> GetOwnersOfAPokemon(int pokemonId);
        bool OwnerExists(int ownerId);
        bool CreateOwner(Owner owner);
        bool UpdateOwner(Owner owner);
        bool DeleteOwner(Owner owner);
        bool Save();
        bool ChangeCountryOfOwner(int ownerId, int newCountryId);
    }
}

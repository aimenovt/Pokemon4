using AutoMapper;
using Pokemon4.Dto;
using Pokemon4.Models;

namespace Pokemon4.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<CategoryDto, Category>();
            CreateMap<CategoryDto, Category>().ReverseMap();

            CreateMap<CountryDto, Country>();
            CreateMap<CountryDto, Country>().ReverseMap();

            CreateMap<OwnerDto, Owner>();
            CreateMap<OwnerDto, Owner>().ReverseMap();

            CreateMap<PokemonDto, Pokemon>();
            CreateMap<PokemonDto, Pokemon>().ReverseMap();

            CreateMap<ReviewDto, Review>();
            CreateMap<ReviewDto, Review>().ReverseMap();

            CreateMap<ReviewerDto, Reviewer>();
            CreateMap<ReviewerDto, Reviewer>().ReverseMap();
        }
    }
}

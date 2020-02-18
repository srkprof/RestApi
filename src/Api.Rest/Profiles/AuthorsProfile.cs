using Api.Rest.Entities;
using Api.Rest.Helpers;
using Api.Rest.Models;
using AutoMapper;

namespace Api.Rest.Profiles
{
    public class AuthorsProfile : Profile
    {
        public AuthorsProfile()
        {
            CreateMap<Author, AuthorDto>()
                .ForMember(mem => mem.Name, opt => opt.MapFrom(x => $"{x.FirstName} {x.LastName}"))
                .ForMember(mem => mem.Age, opt => opt.MapFrom(x => x.DateOfBirth.GetCurrnetAge()))
                .ReverseMap();
            CreateMap<Author, CreateAuthorDto>().ReverseMap();
        }
    }
}
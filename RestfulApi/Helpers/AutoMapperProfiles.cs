using AutoMapper;
using RestfulApi.DTOs;
using RestfulApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulApi.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Genre, GenreDTO>().ReverseMap();

            CreateMap<GenreCreationDTO, Genre>();

            CreateMap<Person, PersonDTO>().ReverseMap();

            CreateMap<PersonCreationDTO, Person>().ForMember(x => x.Picture, option => option.Ignore());

            CreateMap<Person, PersonPatchDTO>().ReverseMap();

            CreateMap<Movie, MovieDTO>().ReverseMap();

            CreateMap<MovieCreationDTO, Movie>().ForMember(x => x.Poster, option => option.Ignore());

            CreateMap<Movie, MoviePatchDTO>().ReverseMap();
        }
    }
}

using Api.Rest.Entities;
using Api.Rest.Models;
using AutoMapper;

namespace Api.Rest.Profiles
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, CourseDto>();
            CreateMap<CourseForCreationDto, Course>();
            CreateMap<CourseForUpdateDto, Course>();
        }
    }
}
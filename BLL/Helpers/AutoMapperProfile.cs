using System.Linq;
using AutoMapper;
using DTO;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace BLL.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            CreateMap<UpdateCourseDto, CourseDto>();
            
            CreateMap<UserDto, ApplicationUser>();

            CreateMap<UserCourseDto, UserCourse>();

            CreateMap<UserCourse, UserCourseDto>();

            CreateMap<ApplicationUser, UserDto>();

            CreateMap<CourseDto, CourseModel>();

            CreateMap<CourseModel, CourseDto>();

            CreateMap<UpdateDto, ApplicationUser>();
            
            CreateMap< ApplicationUser,UpdateDto>();

            CreateMap<UserCourse, UserCourseDto>()
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Course.Description))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Course.Name))
                .ForMember(dest => dest.Id,
                           opt => opt.MapFrom(src => src.Id));
                
            CreateMap<ApplicationUser, AuthenticateResponseDto>();

            CreateMap<FacebookUserInfoDto, ApplicationUser>();

            CreateMap<RegisterDto, ApplicationUser>();
        }
    }
}
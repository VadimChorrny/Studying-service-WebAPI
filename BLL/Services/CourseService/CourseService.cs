using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Internal;
using BLL.Exceptions;
using BLL.Helpers;
using DAL.Commands;
using DTO;
using DAL.Entities;
using DAL.Queries;
using Microsoft.AspNetCore.Mvc;

namespace BLL.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseQuery query;
        private readonly IMapper mapper;
        private readonly IUserQuery userQuery;
        private readonly ICourseCommand courseCommand;

        public CourseService(ICourseQuery query,IUserQuery userQuery, IMapper mapper,ICourseCommand courseCommand)
        {
            this.userQuery = userQuery;
            this.mapper = mapper;
            this.query = query;
            this.courseCommand = courseCommand;
        }

        public async Task<PaginationDto<CourseDto>> GetPagination(CourseParameters courseParameters, string userId)
        {
            courseParameters.SearchText ??= String.Empty; 


            var courses = query.GetAllCourses()
                               .Where(u => u.Name.ToLower()
                                                 .Contains(courseParameters.SearchText.ToLower())
                                     );


            courses = ApplySortService.ApplySort(courses, courseParameters.OrderBy);


            var course = PagedList<CourseModel>.ToPagedList(courses,
                courseParameters.PageNumber,
                courseParameters.PageSize);

            var metadata = new MetadataPaginationDto
            {
                TotalCount = course.TotalCount,
                PageSize = course.PageSize,
                CurrentPage = course.CurrentPage,
                TotalPages = course.TotalPages,

            };
            
            var response = mapper.Map<List<CourseDto>>(course.ToList());

            if (userId != null)
            {
                var user =  userQuery.FindById(userId);

                user.UserCourses.ForEach(c => response
                    .Where(u => u.Id == c.CourseId)
                    .ForAll(uc => uc.IsCurrentUserSubscribe = true)
                );

            }

            return new PaginationDto<CourseDto>  {Collection = response, MetadataPaginationDto = metadata};
        }

        public async void Remove(int id)
        {
             courseCommand.Remove(id);
        }
        
        public async Task<CourseDto> Update(UpdateCourseDto model, int Id)
        {
            var dto = mapper.Map<CourseDto>(model);

            var course = await courseCommand.UpdateUser(mapper.Map<CourseModel>(dto), Id);

            return mapper.Map<CourseDto>(course);
        }

        public async void  AddCourse(CourseDto dto)
        {
            courseCommand.Add(mapper.Map<CourseModel>(dto));
        }
        
        public async Task<PaginationDto<CourseDto>> GetPaginationSubscription(CourseParameters courseParameters, string userId)
                {
                    if (userId == null)
                    {
                        throw new HttpStatusCodeException(HttpStatusCode.MethodNotAllowed,
                                                          "method not allowed for undefined user");
                    }
                    

                    courseParameters.SearchText ??= String.Empty; 
        
        
                    var courses = query.GetAllCourses()
                                       .Where(u => u.Name.ToLower()
                                                         .Contains(courseParameters.SearchText.ToLower())
                                             );
        
        
                    courses = ApplySortService.ApplySort(courses, courseParameters.OrderBy);
        
        
                    var course = PagedList<CourseModel>.ToPagedList(courses,
                        courseParameters.PageNumber,
                        courseParameters.PageSize);
        

                    
                    var response = mapper.Map<List<CourseDto>>(course.ToList());
        
                    
                        var user  =  userQuery.FindById(userId);
        
                        user.UserCourses.ForEach(c => response
                                                     .Where(u => u.Id == c.CourseId)
                                                     .ForAll(uc => uc.IsCurrentUserSubscribe = true)
                                                );
                        response = response.Where(c => c.IsCurrentUserSubscribe == true).ToList();



                        var metadata = new MetadataPaginationDto
                                        {TotalCount = response.Count()};
                        
                    
                    return new PaginationDto<CourseDto>  {Collection = response, MetadataPaginationDto = metadata};
                }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Exceptions;
using BLL.Helpers;
using DTO;
using Hangfire;
using DAL.Commands;
using DAL.Entities;
using DAL.Queries;


namespace BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMailService mailService;
        private readonly IUserQuery query;
        private readonly IUserCommand command;
        private readonly IMapper mapper;

        public AccountService(IUserQuery userQuery,
            IUserCommand command,
            IMailService mailService,
            IMapper mapper)
        {
            this.mapper = mapper;
            this.mailService = mailService;
            query = userQuery;
            this.command = command;
        }

        public async Task<ResponseDto> Delete(string id)
        {
            command.Remove(id);
            return new ResponseDto {Status = "Ok", Message = "User was successful delete"};
        }

        public PaginationDto<UserDto> GetPagination(StudentParameters studentParameters)
        {
            var users = query.GetUserWithSubscribtions();
            if (!string.IsNullOrWhiteSpace(studentParameters.SearchText))
            {
                users = users.Where(u => u.UserName.ToLower().Contains(studentParameters.SearchText.ToLower()) ||
                                         u.Email.ToLower().Contains(studentParameters.SearchText.ToLower()));
            }

            users = ApplySortService.ApplySort(users, studentParameters.OrderBy).AsQueryable();

            var accounts = PagedList<ApplicationUser>.ToPagedList((users),
                studentParameters.PageNumber,
                studentParameters.PageSize);

            var metadata = new MetadataPaginationDto
            {
                TotalCount = accounts.TotalCount,
                PageSize = accounts.PageSize,
                CurrentPage = accounts.CurrentPage,
                TotalPages = accounts.TotalPages,
            };

            return new PaginationDto<UserDto> {Collection = mapper.Map<List<UserDto>>(accounts),MetadataPaginationDto = metadata};
        }

        public void Subscribe(string userId, int courseId, DateTime startStudyDate)
        {
            //var userId = query.GetIdByUserName(userName);
            var daysToStart = (startStudyDate - DateTime.UtcNow).Days;
            
            var userCourse = query.GetUserCourseByUserIdAndCourseId(userId, courseId);

            if (userCourse != null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "Subscription is already exist");
            }

            command.Subscribe(userId, courseId, startStudyDate);
            
            userCourse = query.GetUserCourseByUserIdAndCourseId(userId, courseId);
              
            CreateBodyForMailAboutStartStudy(userCourse, daysToStart);

        }

        public void CreateBodyForMailAboutStartStudy(UserCourse userCourse, int daysToStart)
        {
            const int thirtyDaysToStartStudy = 30;
            const int sevenDaysToStartStudy = 7;
            const int startStudyToday = 0;


            var request = new StartStudyMailDto
            {
                Name = userCourse.Student.FirstName,
                ToEmail = userCourse.Student.Email,
                CourseName = userCourse.Course.Name
            };

            int[] daysToStartStudy = {thirtyDaysToStartStudy, sevenDaysToStartStudy, startStudyToday};

            foreach (var item in daysToStartStudy)
            {
                if (daysToStart >= item)
                {
                    if (item == startStudyToday)
                    {
                        request.DayToStart = "today";
                    }
                    else
                    {
                        request.DayToStart = $"{item} days";
                    }

                    var jobId = AddJobSendEmailAboutStartStudy(request, daysToStart - item);
                    AddHangfireJobToUserCourses(jobId, userCourse);
                }
            }
        }

        public void AddHangfireJobToUserCourses(string jobId,UserCourse userCourse)
        {
            userCourse.HangfireJobs.Add(new HangfireJob  
                                               {JobId = jobId, 
                                                UserCourse = userCourse}); 
        }

        public string AddJobSendEmailAboutStartStudy(StartStudyMailDto request, int days)
        {
            var jobId = BackgroundJob.Schedule(
                () => mailService.SendInfoAboutStartStudyEmailAsync(request),
                TimeSpan.FromDays(days));
            return jobId;
        }

        public void UnSubscribe(string userId, int courseId)
        {
            var userCourse = query.GetUserCourseByUserIdAndCourseId(userId, courseId);
            var jobsId = userCourse.HangfireJobs.Select(uc => uc.JobId);

            foreach (var jobId in jobsId)
            {
                BackgroundJob.Delete(jobId);
            }

            command.UnSubscribe(userId, courseId);
        }

        public async Task<UserDto> GetById(string id)
        {
            var user =  query.FindById(id);
            return mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> Update(string id, UpdateDto model)
        {
            var user = mapper.Map<ApplicationUser>(model);
            
            var res = await command.UpdateUser(id,user);

             var dto =   mapper.Map<UserDto>(res);
             
             return dto;
        }

    }
}
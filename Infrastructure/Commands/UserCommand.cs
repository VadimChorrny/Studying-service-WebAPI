using System;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Queries;
using Microsoft.EntityFrameworkCore;

namespace DAL.Commands
{
    public class UserCommand : IUserCommand
    {
        ApplicationDbContext context;
        private readonly IUserQuery userQuery;

        public UserCommand(ApplicationDbContext context,IUserQuery userQuery)
        {
            this.context = context;
            this.userQuery = userQuery;
        }

        public void UnSubscribe(string userId, int courseId)
        {
            var course = context.UserCourses.FirstOrDefault(uc => uc.CourseId == courseId && uc.StudentId == userId);
            context.UserCourses.Remove(course);

            context.SaveChanges();
        }

        public async Task<ApplicationUser> UpdateUser(string id, ApplicationUser dto)
        {
           var user =  userQuery.FindById(id);
           
           user.Age = dto.Age;
           user.FirstName = dto.FirstName;
           user.LastName = dto.LastName;
           user.UserName = dto.UserName;

           context.Users.Update(user);

           context.SaveChanges();

           return user;
        }

        public async void Remove(string id)
        {
            var user =  userQuery.FindById(id);
           
            if (user == null)
            {
                return;
            }
            context.Users.Remove(user);
            context.SaveChanges();
        }

        public void Subscribe(string userId, int courseId, DateTime startStudyDate)
        {
            var userCourse = new UserCourse
            {
                StartStudyDate = startStudyDate,
                CourseId = courseId,
                StudentId = userId
            };

            context.UserCourses.Add(userCourse);
            context.SaveChanges();
        }
    }
}
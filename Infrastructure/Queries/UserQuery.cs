using System;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Queries
{
    public class UserQuery :  IUserQuery
    {
        private readonly ApplicationDbContext context ;
        public UserQuery(ApplicationDbContext context)
        {
            this.context = context;
        }
        
        public IQueryable<ApplicationUser> GetUserWithSubscribtions()
        {
            var users = context.Users.Include(u => u.UserCourses).ThenInclude(uc => uc.Course);
            return users;
        }

        public RefreshToken GetRefreshTokenByToken(string token)
        {
            var tokens = context.Users.Include(u => u.RefreshTokens);

            var tokens2 = tokens.FirstOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

           var tokenR =  tokens2.RefreshTokens.Last();

           return tokenR;
        }

        public UserCourse GetUserCourseByUserIdAndCourseId(string userId, int courseId)
        {
            return  context.UserCourses.Include(uc =>
                uc.Course
                ).Include(uc => uc.Student).FirstOrDefault( uc => uc.CourseId == courseId && uc.StudentId == userId );
        }

        public RefreshToken GetRefreshTokenFromUserByToken(ApplicationUser user,string token)
        {
            return user.RefreshTokens.SingleOrDefault(x => x.Token == token);
        }
        
        public  ApplicationUser FindById(string id)
        {
                var user = context.Users.Include(u => u.UserCourses).ThenInclude(c => c.Course).SingleOrDefault(u =>u.Id == id);
                return user;
                
                
        }

        public string GetIdByUserName(string userName)
        {
            return context.Users.SingleOrDefault(u => u.UserName == userName)?.Id;
        }
    }
}
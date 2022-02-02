using System.Threading.Tasks;
using DAL.Entities;
using DAL.Queries;
using Org.BouncyCastle.Bcpg;

namespace DAL.Commands
{
    public class CourseCommand :ICourseCommand
    {
        ApplicationDbContext context;
        private readonly ICourseQuery courseQuery;

        public CourseCommand(ApplicationDbContext context,ICourseQuery courseQuery)
        {
            this.context = context;
            this.courseQuery = courseQuery;
        }
        
        public async Task<CourseModel> UpdateUser(CourseModel model,int id)
        {
            var course = await courseQuery.GetById(id);

            course.Description = model.Description;
            
            course.Name = model.Name;
            course.UrlToLogo = model.UrlToLogo;

            context.Courses.Update(course);
            
            context.SaveChanges();

            return await courseQuery.GetById(id);
        }

        public async void Remove(int id)
        {
           var course = await courseQuery.GetById(id);
           
           if (course == null)
           {
               return;
           }
            context.Courses.Remove(course);
             context.SaveChanges();

        }

        public async void Add(CourseModel model)
        {
            await context.Courses.AddAsync(model);
            context.SaveChanges();
        }
    }
}
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Queries
{
    public class CourseQuery : ICourseQuery
    {
        private readonly ApplicationDbContext  context;

        public CourseQuery(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<CourseModel> GetAllCourses()
        {
            var list = context.Courses.ToList();
            return context.Courses;
        }

        public async Task<CourseModel> GetById(int id)
        {
               var res =  context.Courses.FirstOrDefault(c => c.Id == id);
               return res;
        }
    }
}
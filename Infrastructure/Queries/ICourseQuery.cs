using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.Queries
{
    public interface ICourseQuery
    {
        IQueryable<CourseModel> GetAllCourses();

        Task<CourseModel> GetById(int id);
    }
}
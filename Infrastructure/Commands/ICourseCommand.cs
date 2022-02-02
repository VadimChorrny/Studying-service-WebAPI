using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.Commands
{
    public interface ICourseCommand
    {
        Task<CourseModel> UpdateUser(CourseModel dto,int id);

        void Add(CourseModel model);
        
        void Remove(int id);
    }
}
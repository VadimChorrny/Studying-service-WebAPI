using System.Threading.Tasks;
using BLL.Helpers;
using DTO;

namespace BLL.Services
{
    public interface ICourseService
    {
        Task<PaginationDto<CourseDto>> GetPagination(CourseParameters courseParameters,string userId);

        Task<PaginationDto<CourseDto>> GetPaginationSubscription(CourseParameters courseParameters, string userId);
        
        Task<CourseDto> Update(UpdateCourseDto model,int Id);

        void AddCourse(CourseDto dto);
        
        void Remove(int id);
    }
}
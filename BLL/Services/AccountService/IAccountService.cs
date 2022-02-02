using System;
using System.Threading.Tasks;
using BLL.Helpers;
using DTO;
using DAL.Entities;

namespace BLL.Services
{
    public interface IAccountService
    {
        Task<ResponseDto> Delete(string id);

        Task<UserDto> Update(string id, UpdateDto model);

        void Subscribe(string userName, int courseId, DateTime startStudyDate);

        void UnSubscribe(string userName, int courseId);

        Task<UserDto> GetById(string id);

        PaginationDto<UserDto> GetPagination(StudentParameters studentParameters);
    }
}
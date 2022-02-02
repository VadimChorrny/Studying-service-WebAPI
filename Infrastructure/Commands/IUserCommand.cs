using System;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.Commands
{
    public interface IUserCommand
    {
        void UnSubscribe(string userId, int courseId);

        void Subscribe(string userId, int courseId, DateTime startStudyDate);

        Task<ApplicationUser> UpdateUser(string id, ApplicationUser dto);

        void Remove(string id);
    }
}
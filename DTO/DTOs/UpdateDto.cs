using System.ComponentModel.DataAnnotations;
using DAL.Entities;

namespace DTO
{
    public class UpdateDto
    {
        
        public int Age { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public  string UserName { get; set; }
    }
}
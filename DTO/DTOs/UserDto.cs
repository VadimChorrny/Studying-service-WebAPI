using System;
using System.Collections.Generic;

namespace DTO
{
    public class UserDto
    {
        public string Id { get; set; } 

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public DateTime RegisteredDate { get; set; }

        public string Email { get; set; }

        public List<UserCourseDto> UserCourses { get; set; }
    }
}
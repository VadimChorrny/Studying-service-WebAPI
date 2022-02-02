using System;
using System.Collections.Generic;

namespace DTO
{
    public class CourseDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string UrlToLogo { get; set; }
        
        public bool IsCurrentUserSubscribe { get; set; }
    }
}
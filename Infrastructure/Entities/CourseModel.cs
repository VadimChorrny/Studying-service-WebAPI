using System.Collections.Generic;


namespace DAL.Entities
{
    
    public class CourseModel
    {
        public int Id { get; set; }

       public string Name { get; set; }

        public string Description { get; set; }

        public string UrlToLogo { get; set; }

        public virtual ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();
    }
}

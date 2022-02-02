using System;
using System.Collections.Generic;

namespace DAL.Entities
{
    public class UserCourse
    {
        public int  Id { get; set; }

        public string StudentId { get; set; }

        public ApplicationUser Student { get; set; }

        public int CourseId { get; set; }

        public CourseModel Course { get; set; }

        public DateTime StartStudyDate { get; set; }

        public virtual List<HangfireJob> HangfireJobs { get; set; } = new List<HangfireJob>();
    }
}

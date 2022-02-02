using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class HangfireJob
    {
        [Key]
        public string JobId { get; set; }

        public int UserCouseId { get; set; }

        public UserCourse UserCourse { get; set; }
    }
}

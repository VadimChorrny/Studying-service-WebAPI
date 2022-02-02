namespace BLL.Helpers
{
    public class CourseParameters : QueryStringParameters
    {
        public CourseParameters()
        {
            OrderBy = "Name";
        }
    }
}
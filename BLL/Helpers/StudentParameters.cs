namespace BLL.Helpers
{
    public class StudentParameters : QueryStringParameters
    {
        public StudentParameters()
        {
            OrderBy = "FirstName";
        }
    }
}
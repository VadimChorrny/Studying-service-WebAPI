using Newtonsoft.Json;

namespace BLL.Exceptions
{
    public class MyErrorDetails
    {
        public int StatusCode { get; set; }
        
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
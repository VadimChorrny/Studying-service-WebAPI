using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;

namespace BLL.Helpers
{
    public class FacebookTokenValidationResult
    {
        public class Data
        {
            public string app_id { get; set; }
            public string type { get; set; }
            public string application { get; set; }
            public int data_access_expires_at { get; set; }
            public int expires_at { get; set; }
            public bool is_valid { get; set; }
            public List<string> scopes { get; set; }
            public string user_id { get; set; }
        }

        public class Root
        {
            public Data data { get; set; }
        }
    }
}
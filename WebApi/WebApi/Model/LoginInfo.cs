using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Model
{
    public class LoginInfo
    {
        public int id { get; set; }
        public string userName { get; set; }

        public string email { get; set; }

        public DateTime dateTime { get; set; }

        public string token { get; set; }
    }
}

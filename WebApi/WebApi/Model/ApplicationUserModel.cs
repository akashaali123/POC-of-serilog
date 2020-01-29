using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Model
{
    public class ApplicationUserModel
    {

        //User for to bind in the Controller for registration

        public string userName { get; set; }

        public string email { get; set; }
        public string password { get; set; }

        public string fullName { get; set; }
    }
}

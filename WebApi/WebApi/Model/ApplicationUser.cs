using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Model
{
    public class ApplicationUser:IdentityUser
    {
        //use for to add new column in ASP.NET CORE identity
        [Column(TypeName ="nvarchar(50)")]
        public string fullName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Model;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogInRecordController : ControllerBase
    {
        private readonly AuthenticationContext _context;
        public LogInRecordController(AuthenticationContext context)
        {
            _context = context;

        }

        //  [HttpPost]
        //[Route("LoginInfo")
        //public async Task<ActionResult<LoginInfo>> PostLoginInfo(LoginInfo loginInfo)
        //{
        //    _context.loginInfo.Add(loginInfo);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("", new { id = loginInfo.id }, loginInfo);
        //}



    }
}
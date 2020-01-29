using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApi.Model;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;

        private SignInManager<ApplicationUser> _signInManager;

        private readonly ApplicationSettings _appSettings;
       
        
        //IOption Use to inject applicationSettings class in startup and we use it to get a key of appjson settings file just like client url or jwt keys
        public ApplicationUserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,IOptions<ApplicationSettings> appSettings)
        {
            _userManager = userManager;

            _signInManager = signInManager;

            _appSettings = appSettings.Value;
        }




        [HttpPost]

        [Route("Register")]

        //POST : api/ApplicationUser/Register
        public async Task<object> PostApplicationUser(ApplicationUserModel model) //return object type task //To pass the parameter we made ApplicationUserModel
        {
            var applicationUser = new ApplicationUser() { 
           
            UserName = model.userName, //the value pass to the model and we assign the model value in application user constructor to take a value in database

            Email = model.email,

            fullName = model.fullName //password not assign here it is send in a encrypted form
           
            };
           
            try
            {
                var result = await _userManager.CreateAsync(applicationUser, model.password); //password assign here
                
                return Ok(result); //return object of new user
            }
            catch (Exception)
            {

                throw;
            }
         }


        
        
        
        [HttpPost]
       
        [Route("Login")]
       
        //POST : api/ApplicationUser/Login

        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.userName);
            
            if(user != null && await _userManager.CheckPasswordAsync(user,model.password))
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]   
                    {
                        new Claim("userId", user.Id.ToString()) //We access this userID in UserProfile Controller
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(5),
                   
                    SigningCredentials= new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret)),SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
               
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
               
                var token = tokenHandler.WriteToken(securityToken);
                
                return Ok(new { token });

            }
            else
            {
                return BadRequest(new { message = "Invalid User Name or Password" });
            }
        }


   
    
    
    
    }
}
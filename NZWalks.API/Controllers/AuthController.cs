using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager,ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }
        //Post api/auth/register
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            //create identity user
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDTO.Username,
                Email = registerRequestDTO.Username
            };
            var identityResult=await userManager.CreateAsync(identityUser,registerRequestDTO.Password);
            if(identityResult.Succeeded)
            {
                //Add Roles to this user(reader || writer || both)
                if(registerRequestDTO.Roles!=null && registerRequestDTO.Roles.Any())
                {
                    identityResult=await userManager.AddToRolesAsync(identityUser,registerRequestDTO.Roles);
                }
                if(identityResult.Succeeded)
                {
                    return Ok("User registered succesfully! Please Login");
                }

            }
            return BadRequest("something went wrong");
        }



        //Post api/auth/login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var user=await userManager.FindByEmailAsync(loginRequestDTO.Username);
            if (user != null)
            {
                var checkPassworResult=await userManager.CheckPasswordAsync(user, loginRequestDTO.Password);
                if (checkPassworResult)
                {
                    //GET ROLES OF THE USER
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles!=null)
                    {
                        //create JWT Token
                        var jwtToken=tokenRepository.CreateJWTToken(user, roles.ToList());
                        var response = new LoginResponseDTO
                        {
                            JwtToken = jwtToken
                        };

                        return Ok(response);
                    }
                }


            }
            return BadRequest("Invalid Request to login");
            
        }
    }
}

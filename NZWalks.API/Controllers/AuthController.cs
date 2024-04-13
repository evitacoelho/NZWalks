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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }

        //POST: api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            //define the required prop of the Identity user to be used as a first param to create user
            var IdentityUser = new IdentityUser
            {
                UserName = registerRequestDTO.Username,
                Email = registerRequestDTO.Username
            };

            //create an identity using this method - takes a user and a passwaord
            var identityResult = await _userManager.CreateAsync(IdentityUser,registerRequestDTO.Password);

            //if successful, assign roles to created user
            if (identityResult.Succeeded)
            {
                //Assign role to the registered user
                if (registerRequestDTO.Roles != null && registerRequestDTO.Roles.Any())
                {
                    identityResult = await _userManager.AddToRolesAsync(IdentityUser, registerRequestDTO.Roles);

                    if (identityResult.Succeeded)
                    {
                        return Ok("User Registered Successfully - Please Login");
                    }
                }
            }
            return BadRequest("Registration Failed");
        }

        //POST: api/Auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            // get the username from the DTO object
            var user = await _userManager.FindByEmailAsync(loginRequestDTO.Username);

            //if user is registered
            if(user != null)
            {
                //check if the password is valid - returns true or false
                var checkPassword = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

                //create a token is the user is authenticated
                if (checkPassword)
                {
                    //get roles for this user
                    var roles = await _userManager.GetRolesAsync(user);
                    if(roles != null)
                    {
                        var jwtToken = _tokenRepository.CreateJWTToken(user, roles.ToList());

                        //use the dto for making more additions to the response
                        var response = new LoginResponseDTO
                        {
                            JwtToken = jwtToken
                        };
                        return Ok(response);
                    }
                   
                }
                
            }
            return BadRequest("Usernme or password incorrect");
        }
    }
}

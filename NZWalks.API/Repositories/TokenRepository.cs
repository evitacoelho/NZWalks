using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NZWalks.API.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration _configuration;


        //IConfig is required to get information from app settings which is required to create tokens/keys
        public TokenRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string CreateJWTToken(IdentityUser user, List<string> roles)
        {
            //token creation required to create Claims of different types
            var claims = new List<Claim>();

            //add claims - first type email and its value obtained in the user param
            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            //add claims - second type is the role obtained as param
            foreach(var role in roles) 
            { 
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

           
            //create a key from info from app settings
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            //create credentials using genrated key and algothims
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //create tokens - using the infor from app settings and the key and credentials generated
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            //create a  new JWT security token using the token generated above
            return new JwtSecurityTokenHandler().WriteToken(token); 
        }
    }
}

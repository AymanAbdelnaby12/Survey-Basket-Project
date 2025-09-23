using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SurveyBasket.Authentication
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _options;

        public JwtProvider(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }

        public (string token, int expiresIn) GenerateToken(ApplicationUser user)
        {
            // adding Claims
            Claim[] cliams=new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName,user.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName,user.LastName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            // adding encryption and signing credentials
            var symmetricSecurityKey =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
            var signingCredentials=new SigningCredentials(symmetricSecurityKey,SecurityAlgorithms.HmacSha256);
            

            // creating the token   
            var jwtSecurityToken =new JwtSecurityToken(
                issuer:_options.Issuer,
                audience:_options.Audience,
                claims:cliams, 
                expires:DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes),
                signingCredentials:signingCredentials
            );

            // generating the token
            var token=new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return (token,_options.ExpiryMinutes * 60);
        }
    }
}

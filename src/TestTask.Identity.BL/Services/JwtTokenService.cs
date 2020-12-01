using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TestTask.Identity.Common;
using TestTask.Identity.Common.Exceptions;

namespace TestTask.Identity.BL.Services
{
    public interface IJwtTokenService
    {
        Task<string> GenerateJwtToken(List<Claim> claims);
        Task ValidateToken(string token);
    }

    public class JwtTokenService : IJwtTokenService
    {
        private readonly IOptions<AppSettings> _appSettings;

        public JwtTokenService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        public async Task<string> GenerateJwtToken(List<Claim> claims)
        {
            const string sec = "ProEMLh5e_qnzdNUQrqdHPgp";

            var key = new SymmetricSecurityKey(Encoding.Default.GetBytes(sec));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                Issuer = _appSettings.Value.Issuer,
                Audience = _appSettings.Value.Audience,
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task ValidateToken(string token)
        {
            const string sec = "ProEMLh5e_qnzdNUQrqdHPgp";

            var key = new SymmetricSecurityKey(Encoding.Default.GetBytes(sec));

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,             
                    ValidIssuer = _appSettings.Value.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _appSettings.Value.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key              
                }, out SecurityToken validatedToken);
            }
            catch
            {
                throw new UnauthorizedException("Invalid access token");
            }
        }
    }
}

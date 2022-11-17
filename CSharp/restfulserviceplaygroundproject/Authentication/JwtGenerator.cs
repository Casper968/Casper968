using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace restfulserviceplaygroundproject.Authentication
{
    public class JwtGenerator
    {
        public static string GenerateJwt(string username)
        {
            var someClaims = new Claim[]{
                new Claim(JwtRegisteredClaimNames.UniqueName,username),
                new Claim(JwtRegisteredClaimNames.NameId,Guid.NewGuid().ToString())
            };

            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("world_car_api_public_key"));
            var token = new JwtSecurityToken(
                issuer: "www.worldcarplayground.com",
                audience: "www.worldcarplayground.com",
                claims: someClaims,
                expires: DateTime.Now.AddDays(3),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
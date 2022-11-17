using System.Data;
using System.Globalization;
using System.Security.Principal;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using restfulserviceplaygroundproject.DatabaseContext;
using restfulserviceplaygroundproject.Helpers;
using restfulserviceplaygroundproject.Infrastructure;
using System.Data.Entity;
using restfulserviceplaygroundproject.Model;
using System.Data.SQLite;

namespace restfulserviceplaygroundproject.Authentication
{
    public class JwtGenerator
    {
        CarsDbContext _carDbContext;

        public JwtGenerator(
            CarsDbContext carsDbContext)
        {
            this._carDbContext = carsDbContext;
        }

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

        public static bool ValidateToken(string authToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();

            var principal = tokenHandler.ValidateToken(authToken, validationParameters, out SecurityToken validatedToken);
            
            return ReadUserList().Any(x => x.UserName == principal.Claims.FirstOrDefault()?.Value);;
        }

        private static TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidIssuer = "www.worldcarplayground.com",
                ValidAudience = "www.worldcarplayground.com",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("world_car_api_public_key"))
            };
        }

        public static List<UserInfo> ReadUserList()
        {
            try
            {
                string dbFilePath = Directory.GetCurrentDirectory() + "/" + "db_restserviceplayground.db";
                using ( var con = new SQLiteConnection("URI=file:" + dbFilePath))
                {
                    con.Open();
                    string sqlScript = "select ID, Username, Password from Users";
                    using ( var cmd = new SQLiteCommand(sqlScript, con))
                    {
                        using ( var rdr = cmd.ExecuteReader())
                        {

                            List<UserInfo> result = new List<UserInfo>();
                            while(rdr.Read())
                            {
                                UserInfo cur = new UserInfo();
                                cur.ID = rdr.GetInt32(0);
                                cur.UserName = rdr.GetString(1);
                                cur.Password = rdr.GetString(2);
                                result.Add(cur);
                            }
                            con.Close();
                            return result;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                UserInfo cur = new UserInfo();
                cur.ID = -1;
                cur.UserName = e.ToString();
                return new List<UserInfo>() { cur };
            }
        }
    }
}
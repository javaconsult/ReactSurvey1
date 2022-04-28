using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using ClassLibrary1.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApi.Services
{
    public class TokenService
    {
        private readonly IConfiguration config;
        public TokenService(IConfiguration config)
        {
            this.config = config;
        }

        //return token to user when they log in. they can subsequently 
        //use it for authentication
        public string CreateToken(AdminUser user)
        {
            //token contains claims about the user
            //this will be sent by user when authenticating
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
            };

            //To create JWT signature,
            //get 256 bit (32 character) token key from keyvault secret named Settings--TokenKey if present,
            //otherwise from Settings/TokenKey in appsettings 
            string tokenKey = config["Settings:TokenKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
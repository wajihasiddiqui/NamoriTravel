using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Linq;
using System;

namespace ServiceLayer.Helper
{
    public static class JwtTokenManager
    {
        public static string JWTKey = "XCAP05H6LoKvbRRa/QkqLNMI7cOHguaRyHzyg7n5qEkGjQmtBhz4SzYh4Fqwjyi3KJHlSXKPwVu2+bXr6CtpgQ==";
        public static string GenerateToken(string UserId)
        {
            try
            {
                //byte[] key = Convert.FromBase64String(ConfigurationManager.AppSettings["JWTKey"]);
                byte[] key = Convert.FromBase64String(JWTKey);
                SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
                SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] {
                          new Claim(ClaimTypes.UserData, UserId)}),
                    Expires = DateTime.Now.AddDays(7),
                    SigningCredentials = new SigningCredentials(securityKey,
                    SecurityAlgorithms.HmacSha256Signature)
                };
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
                return handler.WriteToken(token);
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null)
                    return null;
               // byte[] key = Convert.FromBase64String(ConfigurationManager.AppSettings["JWTKey"]);
                byte[] key = Convert.FromBase64String(JWTKey);
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token,
                      parameters, out securityToken);
                return principal;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        // ValidateToken Function Called in Filter or Authication Class
        public static Claim ValidateToken(string token)
        {
            ClaimsPrincipal principal = GetPrincipal(token);
            if (principal == null)
                return null;
            ClaimsIdentity identity = null;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return null;
            }
            Claim UserDataClaim = identity.FindFirst(ClaimTypes.UserData);
            return UserDataClaim;
        }
        public static string GenerateStringCode()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 8)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }
    }
}

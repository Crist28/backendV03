using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using backendV03.Models;


namespace backendV03.Tokens
{
    public class Jwt
    {
        private readonly string _secretKey;
        private readonly int _expirationDays;

        public Jwt(IConfiguration configuration)
        {
            _secretKey = configuration["Jwt:Secret"] ?? throw new ArgumentNullException("JWT Secret is missing");
            _expirationDays = int.Parse(configuration["Jwt:ExpirationDays"] ?? "7");
        }

        public string CreateToken(Cliente user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim("nombres", user.Nombres),
                new Claim("apellidos", user.Apellidos),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("role", user.rol), // Ajusta si el rol está en otro campo
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            };

            var token = new JwtSecurityToken(
                issuer: "backendV03",
                audience: "backendV03",
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_expirationDays),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

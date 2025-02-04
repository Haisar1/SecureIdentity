using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using API_Secure.Models;
using Microsoft.Extensions.Configuration;


namespace API_Secure.Custom
{
    public class Utils
    {
        private readonly IConfiguration _configuracion;
        public Utils(IConfiguration configuracion)
        {
            _configuracion = configuracion;
        }

        public string encriptarSHA256(string texto)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(texto));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        public string generarJWT(Usuario modelo)
        {
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, modelo.Usuario1.ToString()),
            };

            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuracion["Jwt:key"]!));
            var credencial = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256Signature);

            var jwtConfig = new JwtSecurityToken(
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: credencial
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }


    }


}

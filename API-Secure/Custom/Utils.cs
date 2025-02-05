using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using API_Secure.Models;


namespace API_Secure.Custom
{
    public class Utils
    {
        private readonly IConfiguration _configuracion;
        private readonly ILogger<Utils> _logger;

        public Utils(IConfiguration configuracion, ILogger<Utils> logger)
        {
            _configuracion = configuracion;
            _logger = logger;
        }

        public string encriptarSHA256(string texto)
        {
            _logger.LogInformation("Iniciando proceso de encriptación SHA256.");
            try
            {
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(texto));

                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytes[i].ToString("x2"));
                    }

                    _logger.LogInformation("Encriptación exitosa.");
                    return builder.ToString();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al encriptar el texto.");
                throw;
            }
        }

        public string generarJWT(Usuario modelo)
        {
            _logger.LogInformation("Generando JWT para el usuario: {Usuario}", modelo.Usuario1);
            try
            {
                var userClaims = new[] {
                    new Claim(ClaimTypes.NameIdentifier, modelo.Usuario1.ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuracion["Jwt:key"]!));
                var credencial = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

                var jwtConfig = new JwtSecurityToken(
                    claims: userClaims,
                    expires: DateTime.UtcNow.AddMinutes(10),
                    signingCredentials: credencial
                );

                var token = new JwtSecurityTokenHandler().WriteToken(jwtConfig);
                _logger.LogInformation("JWT generado exitosamente.");
                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar el JWT para el usuario: {Usuario}", modelo.Usuario1);
                throw;
            }
        }

        public bool validarJWT(string token)
        {
            _logger.LogInformation("Validando token JWT.");
            try
            {
                var claimsPrincipal = new ClaimsPrincipal();
                var tokenValid = new JwtSecurityTokenHandler();
                var validacion = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuracion["Jwt:key"]!))
                };

                claimsPrincipal = tokenValid.ValidateToken(token, validacion, out SecurityToken validatedToken);
                _logger.LogInformation("Token validado exitosamente.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al validar el token JWT.");
                return false;
            }
        }
    }
}

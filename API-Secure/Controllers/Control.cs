using API_Secure.Custom;
using API_Secure.Models;
using API_Secure.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Secure.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class Control : ControllerBase
    {
        private readonly SecureDbContext _secureDbContext;
        private readonly Utils _utilidades;
        private readonly ILogger<Control> _logger;


        public Control(SecureDbContext dbPruebaContext, Utils utilidades, ILogger<Control> logger)
        {
            _secureDbContext = dbPruebaContext;
            _utilidades = utilidades;
            _logger = logger;  
        }

        [HttpPost]
        [Route("Registrarse")]
        public async Task<IActionResult> Registrarse(UsuarioRegistro objeto)
        {
            _logger.LogInformation("Inicio de registro de usuario: {Usuario}", objeto.usuario);

            try
            {
                var modeloUsuario = new Usuario
                {
                    Usuario1 = objeto.usuario,
                    Pass = _utilidades.encriptarSHA256(objeto.pass),
                };

                await _secureDbContext.Usuarios.AddAsync(modeloUsuario);
                await _secureDbContext.SaveChangesAsync();

                if (modeloUsuario.Identificador != 0)
                {
                    _logger.LogInformation("Usuario registrado con éxito: {Usuario}", objeto.usuario);
                    return StatusCode(StatusCodes.Status200OK, new { isSuccess = true });
                }
                else
                {
                    _logger.LogWarning("No se pudo registrar el usuario: {Usuario}", objeto.usuario);
                    return StatusCode(StatusCodes.Status200OK, new { isSuccess = false });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar usuario: {Usuario}", objeto.usuario);
                return StatusCode(StatusCodes.Status500InternalServerError, new { isSuccess = false, error = ex.Message });
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(Login objeto)
        {
            _logger.LogInformation("Intentando iniciar sesión con el usuario: {Usuario}", objeto.usuario);

            try
            {
                var usuarioEncontrado = await _secureDbContext.Usuarios
                                                        .Where(u => u.Pass == _utilidades.encriptarSHA256(objeto.pass))
                                                        .FirstOrDefaultAsync();

                if (usuarioEncontrado == null)
                {
                    _logger.LogWarning("Credenciales incorrectas para el usuario: {Usuario}", objeto.usuario);
                    return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, token = "" });
                }
                else
                {
                    _logger.LogInformation("Inicio de sesión exitoso para el usuario: {Usuario}", objeto.usuario);
                    return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, token = _utilidades.generarJWT(usuarioEncontrado) });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al iniciar sesión para el usuario: {Usuario}", objeto.usuario);
                return StatusCode(StatusCodes.Status500InternalServerError, new { isSuccess = false, error = ex.Message });
            }
        }

        [HttpGet]
        [Route("ValidarToken")]
        public IActionResult ValidarToken(string token)
        {
            _logger.LogInformation("Validando token JWT.");

            try
            {
                bool request = _utilidades.validarJWT(token);

                if (request)
                    _logger.LogInformation("Token validado exitosamente.");
                else
                    _logger.LogWarning("Token inválido.");

                return StatusCode(StatusCodes.Status200OK, new { isSuccess = request });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al validar token JWT.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { isSuccess = false, error = ex.Message });
            }
        }
    }
}

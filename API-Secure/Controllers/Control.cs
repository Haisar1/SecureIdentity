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
        public Control(SecureDbContext dbPruebaContext, Utils utilidades)
        {
            _secureDbContext = dbPruebaContext;
            _utilidades = utilidades;
        }

        [HttpPost]
        [Route("Registrarse")]
        public async Task<IActionResult> Registrarse(UsuarioRegistro objeto)
        {
            var modeloUsuario = new Usuario
            {
                Usuario1 = objeto.usuario,
                Pass = _utilidades.encriptarSHA256(objeto.pass),
            };

            await _secureDbContext.Usuarios.AddAsync(modeloUsuario);
            await _secureDbContext.SaveChangesAsync();

            if (modeloUsuario.Identificador != 0)
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true });
            else
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(Login objeto)
        {
            System.Diagnostics.Debug.WriteLine(objeto.pass);
            var usuarioEncontrado = await _secureDbContext.Usuarios
                                                    .Where(u =>
                                                        u.Pass == _utilidades.encriptarSHA256(objeto.pass)
                                                      ).FirstOrDefaultAsync();

            if (usuarioEncontrado == null)
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, token = "" });
            else
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, token = _utilidades.generarJWT(usuarioEncontrado) });
        }

    }
}

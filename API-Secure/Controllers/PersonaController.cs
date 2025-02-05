using API_Secure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using validatorLIB;


namespace API_Secure.Controllers
{
    [Route("api/personas")]
    [Authorize]
    [ApiController]
    public class PersonasController : ControllerBase
    {
        private readonly ILogger<PersonasController> _logger;
        private readonly SecureDbContext _secureDbContext;

        public PersonasController(SecureDbContext secureDbContext, ILogger<PersonasController> logger)
        {
            _secureDbContext = secureDbContext;
            _logger = logger;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                _logger.LogInformation("Obteniendo lista de personas.");

                var listaPersonas = await _secureDbContext.Personas
                    .FromSqlRaw("EXEC ConsultarPersonas @PageNumber = {0}, @PageSize = {1}", pageNumber, pageSize)
                    .ToListAsync();

                _logger.LogInformation("Lista de personas obtenida exitosamente.");
                return Ok(listaPersonas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de personas.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }


        [HttpPost]
        [Route("Agregar")]
        public async Task<IActionResult> Agregar([FromBody] Persona request)
        {
            try
            {
                _logger.LogInformation("Agregando nueva persona.");

                if (!EmailValidator.IsValidEmail(request.Email))
                {
                    return BadRequest("El correo electrónico proporcionado no es válido.");
                }

                await _secureDbContext.Personas.AddAsync(request);
                await _secureDbContext.SaveChangesAsync();
                _logger.LogInformation("Persona agregada exitosamente.");
                return Ok(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar persona.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }


        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                _logger.LogInformation("Eliminando persona con ID: {Id}", id);
                var personaEliminar = await _secureDbContext.Personas.FindAsync(id);
                if (personaEliminar == null)
                {
                    _logger.LogWarning("Persona con ID {Id} no encontrada.", id);
                    return NotFound("Persona no encontrada.");
                }
                _secureDbContext.Personas.Remove(personaEliminar);
                await _secureDbContext.SaveChangesAsync();
                _logger.LogInformation("Persona con ID {Id} eliminada exitosamente.", id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar persona con ID {Id}.", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpPut]
        [Route("Actualizar/{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Persona request)
        {
            try
            {
                _logger.LogInformation("Actualizando persona con ID: {Id}", id);

                var personaActualizar = await _secureDbContext.Personas.FindAsync(id);
                if (personaActualizar == null)
                    return NotFound("No existe la persona");
                _logger.LogInformation("request.", request);
                personaActualizar.Nombres = request.Nombres;
                personaActualizar.Apellidos = request.Apellidos;
                personaActualizar.Email = request.Email;
                personaActualizar.NumeroIdentificacion = request.NumeroIdentificacion;
                personaActualizar.TipoIdentificacion = request.TipoIdentificacion;

                _secureDbContext.Personas.Update(personaActualizar);
                await _secureDbContext.SaveChangesAsync();

                _logger.LogInformation("Persona con ID {Id} actualizada exitosamente.", id);

                return Ok(personaActualizar);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar persona con ID {Id}.", id);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

    }

}

using API_Secure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Secure.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/personas")]
    public class PersonasController : ControllerBase
    {
        private readonly SecureDbContext _secureDbContext;
        public PersonasController(SecureDbContext secureDbContext)
        {
            _secureDbContext = secureDbContext;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            var listaPeliculas = await _secureDbContext.Personas.ToListAsync();
            return Ok(listaPeliculas);
        }

        [HttpPost]
        [Route("Agregar")]
        public async Task<IActionResult> Agregar([FromBody] Persona request)
        {
            await _secureDbContext.Personas.AddAsync(request);
            await _secureDbContext.SaveChangesAsync();
            return Ok(request);
        }

        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var personaEliminar = await _secureDbContext.Personas.FindAsync(id);
            if (personaEliminar == null) 
                return BadRequest("No existe la persona");
            _secureDbContext.Personas.Remove(personaEliminar);
            await _secureDbContext.SaveChangesAsync();
            return Ok();
          

        }
    }

}

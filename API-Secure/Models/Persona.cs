using System;
using System.Collections.Generic;

namespace API_Secure.Models;

public partial class Persona
{
    public int Identificador { get; set; }

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string NumeroIdentificacion { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string TipoIdentificacion { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public string? NumeroIdentificacionConTipo { get; set; }

    public string? NombresApellidos { get; set; }
}

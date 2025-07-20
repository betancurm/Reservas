using apiReservas.Models;
using Microsoft.AspNetCore.Identity;

namespace apiReservas.Identity;

public class ApplicationUser : IdentityUser<Guid> 
{
    public ICollection<Ingreso> Ingresos { get; set; } = new List<Ingreso>();
    public ICollection<Egreso> Egresos { get; set; } = new List<Egreso>();
    public ICollection<Inmueble> Inmuebles { get; set; } = new List<Inmueble>();
    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}

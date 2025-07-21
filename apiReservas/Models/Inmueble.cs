
using System.Text.Json.Serialization;

namespace apiReservas.Models
{
    public class Inmueble
    {
        public Guid InmuebleId { get; set; }
        public string CodigoInmueble { get; set; }
        public string Direccion { get; set; }
        public string Ciudad { get; set; }
        public int NumeroHabitaciones { get; set; }
        public int NumeroBanos { get; set; }
        public bool Parqueadero { get; set; }
        [JsonIgnore]
        public virtual ICollection<Ingreso>? Ingresos { get; set; }
        [JsonIgnore]
        public virtual ICollection<Egreso>? Egresos { get; set; }
        [JsonIgnore]
        public virtual ICollection<Reserva>? Reservas { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public Guid ApplicationUserId { get; set; }
    }
}

using System.Text.Json.Serialization;

namespace apiReservas.Models
{
    public class Reserva
    {
        public Guid ReservaId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public float ValorReservaUSD { get; set; }
        public Guid InmuebleId { get; set; }
        [JsonIgnore]
        public virtual Inmueble? Inmueble { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        [JsonIgnore]
        public virtual ICollection<DetalleReserva> DetallesReserva { get; set; } = new  List<DetalleReserva>();
        public Guid ApplicationUserId { get; set; }

    }
}

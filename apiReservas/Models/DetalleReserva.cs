using System.Text.Json.Serialization;

namespace apiReservas.Models
{
    public class DetalleReserva
    {
        public Guid DetalleReservaId { get; set; }
        public Guid ReservaId { get; set; }
        public virtual Reserva Reserva { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int NochesOcupadas { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}

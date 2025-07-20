using System.Text.Json.Serialization;

namespace apiReservas.Models
{
    public class Ingreso
    {
        public Guid IngresoId { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaIngreso { get; set; }
        public TipoPlataforma TipoPlataforma { get; set; }
        public float ValorUSD { get; set; }
        public float TRM { get; set; }
        [JsonIgnore]
        public float ValorCOP { get; set ; }
        public Guid InmuebleId { get; set; }
        [JsonIgnore]
        public virtual Inmueble? Inmueble { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public Guid ApplicationUserId { get; private set; }

    }

    public enum TipoPlataforma
    {
        Airbnb,
        Booking,
        Directa
    }
}


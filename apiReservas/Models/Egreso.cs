using System.Text.Json.Serialization;

namespace apiReservas.Models
{
    public class Egreso
    {

        public Guid EgresoId { get; set; }
        public DateTime FechaEgreso { get; set; }
        public Concepto Concepto { get; set; }
        public Tipo Tipo { get; set; }
        public string Descripcion { get; set; }
        public float Valor { get; set; }
        public Guid InmuebleId { get; set; }
        [JsonIgnore]
        public virtual Inmueble Inmueble { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public Guid ApplicationUserId { get; private set; }

    }
    public enum Concepto
    {
        GastosFijos,
        Bancos,
        Otros
    }
    public enum Tipo
    {
        Administracion,
        Aseo,
        Carreras,
        Creditos,
        GastosExtras,
        Impuestos,
        Productos,
        Reparaciones,
        Servicios,
    }
}


namespace apiReservas.DTOs.Reservas
{
    public class ReservasDTO
    {
        public Guid ReservaId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public float ValorReservaUSD { get; set; }
        public string CodigoInmueble { get; set; }  // Agregar esta propiedad
        public string NochesOcupadas { get; set;}

    }
}

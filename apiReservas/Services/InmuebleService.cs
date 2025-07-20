using apiReservas.Data;
using apiReservas.Models;

namespace apiReservas.Services
{
    public class InmuebleService : IInmuebleService
    {
        private readonly ReservasContext _context;
        private readonly ICurrentUserService _currentUserService;

        public InmuebleService(ReservasContext dbcontext, ICurrentUserService currentUserService)
        {
            _context = dbcontext;
            _currentUserService = currentUserService;

        }
        public IEnumerable<Inmueble> Get()
        {
            return _context.Inmuebles;
        }
        public async Task Save(Inmueble inmueble)
        {
            inmueble.InmuebleId = Guid.NewGuid();
            inmueble.FechaCreacion = DateTime.Now;
            inmueble.FechaModificacion = DateTime.Now;
            _context.Inmuebles.Add(inmueble);

            await _context.SaveChangesAsync();
        }
        public async Task Update(Guid id, Inmueble inmueble)
        {
            var inmuebleActual = _context.Inmuebles.Find(id);

            if (inmuebleActual != null)
            {
                inmuebleActual.Direccion = inmueble.Direccion;
                inmuebleActual.Ciudad = inmueble.Ciudad;
                inmuebleActual.NumeroHabitaciones = inmueble.NumeroHabitaciones;
                inmuebleActual.NumeroBanos = inmueble.NumeroBanos;
                inmuebleActual.Parqueadero = inmueble.Parqueadero;
                inmuebleActual.FechaCreacion = inmueble.FechaCreacion;
                inmuebleActual.FechaModificacion = DateTime.Now;

                await _context.SaveChangesAsync();
            }
        }
        public async Task Delete(Guid id)
        {
            var inmuebleActual = _context.Inmuebles.Find(id);
            if (inmuebleActual != null)
            {
                _context.Inmuebles.Remove(inmuebleActual);
                await _context.SaveChangesAsync();
            }
        }

    }
    public interface IInmuebleService
    {
        public IEnumerable<Inmueble> Get();
        public Task Save(Inmueble inmueble);
        public Task Update(Guid id, Inmueble inmueble);
        public Task Delete(Guid id);
    }
}

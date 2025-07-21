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
        public async Task SaveAsync(Inmueble inmueble)
        {
            inmueble.ApplicationUserId = _currentUserService.ApplicationUserId;
            inmueble.InmuebleId = Guid.NewGuid();
            inmueble.FechaCreacion = DateTime.Now;
            inmueble.FechaModificacion = DateTime.Now;
            _context.Inmuebles.Add(inmueble);

            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Guid id, Inmueble inmueble)
        {
            var inmuebleActual = _context.Inmuebles.Find(id);

            if (inmuebleActual != null)
            {
                inmueble.ApplicationUserId = _currentUserService.ApplicationUserId;
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
        public async Task DeleteAsync(Guid id)
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
        public Task SaveAsync(Inmueble inmueble);
        public Task UpdateAsync(Guid id, Inmueble inmueble);
        public Task DeleteAsync(Guid id);
    }
}

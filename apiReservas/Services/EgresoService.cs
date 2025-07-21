using apiReservas.Data;
using apiReservas.Models;

namespace apiReservas.Services
{
    public class EgresoService : IEgresoService
    {
        
        private readonly ICurrentUserService _currentUserService;
        private readonly ReservasContext _context;
        public EgresoService(ReservasContext dbcontext, ICurrentUserService currentUserService)
        {
            _context = dbcontext;
            _currentUserService = currentUserService;
        }
        public IEnumerable<Egreso> Get()
        {
            return _context.Egresos;
        }
        public async Task Save(Egreso egreso)
        {
            // 1. Usuario autenticado
            if (_currentUserService.ApplicationUserId == Guid.Empty)
                throw new UnauthorizedAccessException("Token sin uid");
            egreso.ApplicationUserId = _currentUserService.ApplicationUserId;
            egreso.EgresoId = Guid.NewGuid();
            egreso.FechaCreacion = DateTime.Now;
            egreso.FechaModificacion = DateTime.Now;
            _context.Egresos.Add(egreso);

            await _context.SaveChangesAsync();
        }
        public async Task Update(Guid id, Egreso egreso)
        {
            // 1. Usuario autenticado
            if (_currentUserService.ApplicationUserId == Guid.Empty)
                throw new UnauthorizedAccessException("Token sin uid");
            var egresoActual = _context.Egresos.Find(id);

            if (egresoActual != null)
            {
                egresoActual.ApplicationUserId = _currentUserService.ApplicationUserId;
                egresoActual.Descripcion = egreso.Descripcion;
                egresoActual.FechaEgreso = egreso.FechaEgreso;
                egresoActual.Tipo = egreso.Tipo;
                egresoActual.Valor = egreso.Valor;
                egresoActual.InmuebleId = egreso.InmuebleId;
                egreso.FechaCreacion = egreso.FechaCreacion;
                egresoActual.FechaModificacion = DateTime.Now;

                await _context.SaveChangesAsync();
            }
        }
        public async Task Delete(Guid id)
        {
            var egresoActual = _context.Egresos.Find(id);
            if (egresoActual != null)
            {
                _context.Egresos.Remove(egresoActual);
                await _context.SaveChangesAsync();
            }
        }
    }

    public interface IEgresoService
    {
        IEnumerable<Egreso> Get();
        Task Save(Egreso egreso);
        Task Update(Guid id, Egreso egreso);
        Task Delete(Guid id);

    }
}
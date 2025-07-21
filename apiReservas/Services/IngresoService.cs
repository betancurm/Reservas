using apiReservas.Data;
using apiReservas.Models;

namespace apiReservas.Services
{
    public class IngresoService : IIngresoService
    {
        private readonly ICurrentUserService _currentUserService;

        private readonly ReservasContext _context;

        public IngresoService(ReservasContext dbcontext, ICurrentUserService currentUserService)
        {
            _context = dbcontext;
            _currentUserService = currentUserService;
        }
        public IEnumerable<Ingreso> Get()
        {
            return _context.Ingresos;
        }

        public async Task SaveAsync(Ingreso ingreso)
        {
            // 1. Usuario autenticado
            if (_currentUserService.ApplicationUserId == Guid.Empty)
                throw new UnauthorizedAccessException("Token sin uid");
            ingreso.ApplicationUserId  = _currentUserService.ApplicationUserId;
            
            ingreso.IngresoId = Guid.NewGuid();
            ingreso.FechaCreacion = DateTime.Now;
            ingreso.FechaModificacion = DateTime.Now;
            ingreso.ValorCOP = ingreso.ValorUSD * ingreso.TRM;
            _context.Ingresos.Add(ingreso);

            await _context.SaveChangesAsync();
        }
 
        public async Task Update(Guid id, Ingreso ingreso)
        {
            // 1. Usuario autenticado
            if (_currentUserService.ApplicationUserId == Guid.Empty)
                throw new UnauthorizedAccessException("Token sin uid");
            var ingresoActual = _context.Ingresos.Find(id);

            if (ingresoActual != null)
            {   
                ingresoActual.ApplicationUserId = _currentUserService.ApplicationUserId;
                ingresoActual.Descripcion = ingreso.Descripcion;
                ingresoActual.FechaIngreso = ingreso.FechaIngreso;
                ingresoActual.TipoPlataforma = ingreso.TipoPlataforma;
                ingresoActual.ValorUSD = ingreso.ValorUSD;
                ingresoActual.TRM = ingreso.TRM;
                ingresoActual.ValorCOP = ingreso.ValorUSD * ingreso.TRM;
                ingresoActual.InmuebleId = ingreso.InmuebleId;
                //ingreso.FechaCreacion = ingreso.FechaCreacion;
                ingresoActual.FechaModificacion = DateTime.Now;

                await _context.SaveChangesAsync();
            }
        }
        public async Task Delete(Guid id)
        {
            var ingresoActual = _context.Ingresos.Find(id);
            if(ingresoActual != null)
            {
                _context.Ingresos.Remove(ingresoActual);
                await _context.SaveChangesAsync();
            }
        }   
    }

    public interface IIngresoService
    {
        IEnumerable<Ingreso> Get();
        Task SaveAsync(Ingreso ingreso);
        Task Update(Guid id, Ingreso ingreso);
        Task Delete(Guid id);

    }
}
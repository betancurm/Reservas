using apiReservas.Data;
using apiReservas.Models;

namespace apiReservas.Services
{
    public class IngresoService : IIngresoService
    {
        private readonly ReservasContext _context;
        public IngresoService(ReservasContext dbcontext)
        {
            _context = dbcontext;
        }
        public IEnumerable<Ingreso> Get()
        {
            return _context.Ingresos;
        }

        public async Task Save(Ingreso ingreso)
        {
            ingreso.IngresoId = Guid.NewGuid();
            ingreso.FechaCreacion = DateTime.Now;
            ingreso.FechaModificacion = DateTime.Now;
            ingreso.ValorCOP = ingreso.ValorUSD * ingreso.TRM;
            _context.Ingresos.Add(ingreso);

            await _context.SaveChangesAsync();
        }
 
        public async Task Update(Guid id, Ingreso ingreso)
        {
            var ingresoActual = _context.Ingresos.Find(id);

            if (ingresoActual != null)
            { 
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
        Task Save(Ingreso ingreso);
        Task Update(Guid id, Ingreso ingreso);
        Task Delete(Guid id);

    }
}
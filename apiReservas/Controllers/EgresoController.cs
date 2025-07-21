using apiReservas.Models;
using apiReservas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apiReservas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EgresoController: ControllerBase
    {
        IEgresoService _egresoService;
        public EgresoController(IEgresoService egresoService)
        {
            _egresoService = egresoService;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_egresoService.Get());
        }
        [HttpPost]
        public IActionResult Post([FromBody] Egreso egreso)
        {
            _egresoService.Save(egreso);
            return Ok();
        }
        [HttpPut("{id:guid}")]
        public IActionResult Put(Guid id, [FromBody] Egreso egreso)
        {
            _egresoService.Update(id, egreso);
            return Ok();
        }
        [HttpDelete("{id:guid}")]    
        public IActionResult Delete(Guid id)
        {
            _egresoService.Delete(id);
            return Ok();
        }
    }
}

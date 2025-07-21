using apiReservas.Models;
using apiReservas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiReservas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class IngresoController: ControllerBase
    {
        IIngresoService _ingresoService;
        public IngresoController(IIngresoService ingresoService)
        {
            _ingresoService = ingresoService;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_ingresoService.Get());
        }

        [HttpPost]
        public IActionResult Post([FromBody] Ingreso ingreso)
        {
            _ingresoService.SaveAsync(ingreso);
            return Ok();
        }

        [HttpPut("{id:guid}")]
        public IActionResult Put(Guid id, [FromBody] Ingreso ingreso)
        {
            _ingresoService.Update(id, ingreso);
            return Ok();
        }
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            _ingresoService.Delete(id);
            return Ok();
        }
       
    }
}

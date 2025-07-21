using apiReservas.Data;
using apiReservas.DTOs.Reservas;
using apiReservas.Models;
using apiReservas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apiReservas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReservaController : ControllerBase
    {
        IReservaService _reservaService;
        public ReservaController(IReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_reservaService.Get());
        }
        [HttpGet("dto")]
        public ActionResult<IEnumerable<ReservasDTO>> GetReservas()
        {
            var reservas = _reservaService.GetReservaDto();
            return Ok(reservas);
        }
        [HttpPost]
        public IActionResult Post([FromBody] Reserva reserva)
        {
            _reservaService.SaveAsync(reserva);
            return Ok();
        }
        [HttpPut("{id:guid}")]
        public  IActionResult Put(Guid id, [FromBody] Reserva reserva)
        {
            _reservaService.UpdateAsync(id, reserva);
            return Ok();
        }
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            _reservaService.DeleteAsync(id);
            return Ok();
        }

    }
}

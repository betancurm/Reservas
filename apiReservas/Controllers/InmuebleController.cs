using apiReservas.Data;
using apiReservas.Models;
using apiReservas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apiReservas.Controllers
{
    [ApiController]  
    [Route("api/[controller]")]
    [Authorize]
    public class InmuebleController : ControllerBase
    {
        IInmuebleService _inmuebleService;
        public InmuebleController(IInmuebleService inmuebleService)
        {
            _inmuebleService = inmuebleService;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_inmuebleService.Get());
        }


        [HttpPost]
        public IActionResult Post([FromBody] Inmueble inmueble)
        {
            _inmuebleService.SaveAsync(inmueble);
            return Ok();
        }

        [HttpPut("{id:guid}")]
        public IActionResult Put(Guid id, [FromBody] Inmueble inmueble)
        {
            _inmuebleService.UpdateAsync(id, inmueble);
            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            _inmuebleService.DeleteAsync(id);
            return Ok();
        }   
    }
}

using apiReservas.Data;
using apiReservas.Models;
using apiReservas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apiReservas.Controllers
{
    [Authorize]                     

    [Route("api/[controller]")]

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
            _inmuebleService.Save(inmueble);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] Inmueble inmueble)
        {
            _inmuebleService.Update(id, inmueble);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _inmuebleService.Delete(id);
            return Ok();
        }   
    }
}

using Microsoft.AspNetCore.Mvc;
using SocialCrap.DTOs;
using SocialCrap.Service;

namespace SocialCrap.Controllers
{
    // Endpoints para registrar reacoes (poops).
    [ApiController]
    [Route("api/[controller]")]
    public class PoopController : ControllerBase
    {
        private readonly IPoopService _service;

        public PoopController(IPoopService service)
        {
            _service = service;
        }

        // Lista todos os poops.
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var poops = await _service.GetAllAsync();
            return Ok(poops);
        }

        // Busca poop por id.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (!result.Success)
                return NotFound();

            return Ok(result.Data);
        }

        // Cria um poop.
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PoopCreateRequest request)
        {
            var result = await _service.CreateAsync(request);

            if (!result.Success)
                return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
        }

        // Remove um poop.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);

            if (result.NotFound)
                return NotFound();

            return NoContent();
        }
    }
}

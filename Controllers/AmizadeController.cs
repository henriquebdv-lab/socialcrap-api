using Microsoft.AspNetCore.Mvc;
using SocialCrap.DTOs;
using SocialCrap.Service;

namespace SocialCrap.Controllers
{
    // Endpoints para gerenciar amizades.
    [ApiController]
    [Route("api/[controller]")]
    public class AmizadeController : ControllerBase
    {
        private readonly IAmizadeService _service;

        public AmizadeController(IAmizadeService service)
        {
            _service = service;
        }

        // Lista todas as amizades.
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var amizades = await _service.GetAllAsync();
            return Ok(amizades);
        }

        // Busca amizade por id.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (!result.Success)
                return NotFound();

            return Ok(result.Data);
        }

        // Envia solicitacao de amizade.
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AmizadeCreateRequest request)
        {
            var result = await _service.CreateAsync(request);

            if (!result.Success)
                return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
        }

        // Aceita solicitacao pendente.
        [HttpPut("{id}/aceitar")]
        public async Task<IActionResult> Aceitar(int id)
        {
            var result = await _service.AceitarAsync(id);

            if (result.NotFound)
                return NotFound();

            if (!result.Success)
                return BadRequest(result.Error);

            return Ok(new { mensagem = "Amizade aceita." });
        }

        // Recusa solicitacao pendente.
        [HttpPut("{id}/recusar")]
        public async Task<IActionResult> Recusar(int id)
        {
            var result = await _service.RecusarAsync(id);

            if (result.NotFound)
                return NotFound();

            if (!result.Success)
                return BadRequest(result.Error);

            return Ok(new { mensagem = "Amizade recusada." });
        }

        // Remove amizade.
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

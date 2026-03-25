using Microsoft.AspNetCore.Mvc;
using SocialCrap.DTOs;
using SocialCrap.Service;

namespace SocialCrap.Controllers
{
    // Endpoints para gerenciar noticias.
    [ApiController]
    [Route("api/[controller]")]
    public class NoticiaController : ControllerBase
    {
        private readonly INoticiaService _service;

        public NoticiaController(INoticiaService service)
        {
            _service = service;
        }

        // Lista todas as noticias.
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var noticias = await _service.GetAllAsync();
            return Ok(noticias);
        }

        // Busca noticia por id.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (!result.Success)
                return NotFound();

            return Ok(result.Data);
        }

        // Cria uma noticia.
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NoticiaCreateRequest request)
        {
            var result = await _service.CreateAsync(request);

            if (!result.Success)
                return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
        }

        // Atualiza titulo e conteudo.
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] NoticiaUpdateRequest request)
        {
            var result = await _service.UpdateAsync(id, request);

            if (result.NotFound)
                return NotFound();

            if (!result.Success)
                return BadRequest(result.Error);

            return Ok(result.Data);
        }

        // Remove noticia.
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

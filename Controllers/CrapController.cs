using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialCrap.Data;
using SocialCrap.DTOs;
using SocialCrap.Models;

namespace SocialCrap.Controllers
{
    // Endpoints para criar e listar craps.
    [ApiController]
    [Route("api/[controller]")]
    public class CrapController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CrapController(AppDbContext context)
        {
            _context = context;
        }

        // Lista todos os craps com um payload enxuto.
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var craps = await _context.Craps
                .Include(c => c.Usuario)
                .Select(c => new CrapResponse
                {
                    Id = c.Id,
                    Mensagem = c.Mensagem,
                    Data = c.Data,
                    UsuarioId = c.UsuarioId,
                    UsuarioNome = c.Usuario.Nome
                })
                .ToListAsync();

            return Ok(craps);
        }

        // Busca um crap por id.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var crap = await _context.Craps
                .Include(c => c.Usuario)
                .Where(c => c.Id == id)
                .Select(c => new CrapResponse
                {
                    Id = c.Id,
                    Mensagem = c.Mensagem,
                    Data = c.Data,
                    UsuarioId = c.UsuarioId,
                    UsuarioNome = c.Usuario.Nome
                })
                .FirstOrDefaultAsync();

            if (crap == null)
                return NotFound();

            return Ok(crap);
        }

        // Cria um novo crap.
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CrapCreateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Mensagem) || request.UsuarioId <= 0)
                return BadRequest("Mensagem e UsuarioId sao obrigatorios.");

            var usuario = await _context.Usuarios.FindAsync(request.UsuarioId);

            if (usuario == null)
                return BadRequest("Usuario nao encontrado.");

            var crap = new Crap(request.Mensagem, request.UsuarioId);

            _context.Craps.Add(crap);
            await _context.SaveChangesAsync();

            var response = new CrapResponse
            {
                Id = crap.Id,
                Mensagem = crap.Mensagem,
                Data = crap.Data,
                UsuarioId = crap.UsuarioId,
                UsuarioNome = usuario.Nome
            };

            return CreatedAtAction(nameof(GetById), new { id = crap.Id }, response);
        }
    }
}

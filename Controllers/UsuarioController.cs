using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialCrap.Data;
using SocialCrap.DTOs;
using SocialCrap.Models;

namespace SocialCrap.Controllers
{
    // Endpoints para gerenciar usuários.
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UsuarioCreateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Nome) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Senha))
            {
                return BadRequest("Nome, email e senha sao obrigatorios.");
            }

            var usuario = new Usuario(request.Nome, request.Email, request.Senha);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, usuario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UsuarioUpdateRequest request)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound();

            var perfilAtualizado = usuario.AtualizarPerfil(request.Nome, request.Email);

            if (!perfilAtualizado)
                return BadRequest("Nome e email invalidos.");

            await _context.SaveChangesAsync();

            return Ok(usuario);
        }

        [HttpPut("{id}/senha")]
        public async Task<IActionResult> UpdateSenha(int id, [FromBody] UsuarioSenhaRequest request)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound();

            var senhaAtualizada = usuario.AtualizarSenha(request.SenhaAtual, request.NovaSenha);

            if (!senhaAtualizada)
                return BadRequest("Senha atual incorreta ou nova senha invalida.");

            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Senha atualizada com sucesso." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound();

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

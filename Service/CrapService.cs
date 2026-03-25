using Microsoft.EntityFrameworkCore;
using SocialCrap.Data;
using SocialCrap.DTOs;
using SocialCrap.Models;

namespace SocialCrap.Service
{
    // Regras de negocio para Crap.
    public class CrapService : ICrapService
    {
        private readonly AppDbContext _context;

        public CrapService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CrapResponse>> GetAllAsync()
        {
            // Monta lista com nome do usuario para o front.
            return await _context.Craps
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
        }

        public async Task<OperationResult<CrapResponse>> GetByIdAsync(int id)
        {
            // Busca com include para devolver nome do usuario.
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
                return OperationResult<CrapResponse>.NotFoundResult();

            return OperationResult<CrapResponse>.Ok(crap);
        }

        public async Task<OperationResult<CrapResponse>> CreateAsync(CrapCreateRequest request)
        {
            // Validacoes basicas de entrada.
            if (string.IsNullOrWhiteSpace(request.Mensagem) || request.UsuarioId <= 0)
                return OperationResult<CrapResponse>.Fail("Mensagem e UsuarioId sao obrigatorios.");

            // Garante que o usuario existe.
            var usuario = await _context.Usuarios.FindAsync(request.UsuarioId);
            if (usuario == null)
                return OperationResult<CrapResponse>.Fail("Usuario nao encontrado.");

            var crap = new Crap(request.Mensagem.Trim(), request.UsuarioId);
            _context.Craps.Add(crap);
            await _context.SaveChangesAsync();

            // Resposta enxuta para o front.
            var response = new CrapResponse
            {
                Id = crap.Id,
                Mensagem = crap.Mensagem,
                Data = crap.Data,
                UsuarioId = crap.UsuarioId,
                UsuarioNome = usuario.Nome
            };

            return OperationResult<CrapResponse>.Ok(response);
        }

        public async Task<OperationResult<Crap>> UpdateAsync(int id, CrapUpdateRequest request)
        {
            // Valida mensagem antes de buscar.
            if (string.IsNullOrWhiteSpace(request.Mensagem))
                return OperationResult<Crap>.Fail("Mensagem invalida.");

            var crap = await _context.Craps.FindAsync(id);
            if (crap == null)
                return OperationResult<Crap>.NotFoundResult();

            // Usa regra de dominio para atualizar.
            var atualizado = crap.AtualizarMensagem(request.Mensagem.Trim());
            if (!atualizado)
                return OperationResult<Crap>.Fail("Mensagem invalida.");

            await _context.SaveChangesAsync();
            return OperationResult<Crap>.Ok(crap);
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            var crap = await _context.Craps.FindAsync(id);
            if (crap == null)
                return OperationResult.NotFoundResult();

            _context.Craps.Remove(crap);
            await _context.SaveChangesAsync();

            return OperationResult.Ok();
        }
    }
}

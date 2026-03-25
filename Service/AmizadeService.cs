using Microsoft.EntityFrameworkCore;
using SocialCrap.Data;
using SocialCrap.DTOs;
using SocialCrap.Models;

namespace SocialCrap.Service
{
    // Regras de negocio para Amizade.
    public class AmizadeService : IAmizadeService
    {
        private readonly AppDbContext _context;

        public AmizadeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AmizadeResponse>> GetAllAsync()
        {
            // Lista enxuta para o front.
            return await _context.Amizades
                .Select(a => new AmizadeResponse
                {
                    Id = a.Id,
                    UsuarioId = a.UsuarioId,
                    AmigoId = a.AmigoId,
                    Status = a.Status
                })
                .ToListAsync();
        }

        public async Task<OperationResult<AmizadeResponse>> GetByIdAsync(int id)
        {
            var amizade = await _context.Amizades
                .Where(a => a.Id == id)
                .Select(a => new AmizadeResponse
                {
                    Id = a.Id,
                    UsuarioId = a.UsuarioId,
                    AmigoId = a.AmigoId,
                    Status = a.Status
                })
                .FirstOrDefaultAsync();

            if (amizade == null)
                return OperationResult<AmizadeResponse>.NotFoundResult();

            return OperationResult<AmizadeResponse>.Ok(amizade);
        }

        public async Task<OperationResult<AmizadeResponse>> CreateAsync(AmizadeCreateRequest request)
        {
            // Valida ids basicos.
            if (request.UsuarioId <= 0 || request.AmigoId <= 0)
                return OperationResult<AmizadeResponse>.Fail("UsuarioId e AmigoId sao obrigatorios.");

            // Nao permite amizade consigo mesmo.
            if (request.UsuarioId == request.AmigoId)
                return OperationResult<AmizadeResponse>.Fail("Nao e possivel adicionar a si mesmo.");

            // Garante que ambos usuarios existem.
            var usuariosExistem = await _context.Usuarios
                .Where(u => u.Id == request.UsuarioId || u.Id == request.AmigoId)
                .CountAsync();

            if (usuariosExistem < 2)
                return OperationResult<AmizadeResponse>.Fail("Usuario ou amigo nao encontrado.");

            // Evita duplicidade em qualquer direcao.
            var jaExiste = await _context.Amizades.AnyAsync(a =>
                (a.UsuarioId == request.UsuarioId && a.AmigoId == request.AmigoId) ||
                (a.UsuarioId == request.AmigoId && a.AmigoId == request.UsuarioId));

            if (jaExiste)
                return OperationResult<AmizadeResponse>.Fail("Ja existe uma amizade ou solicitacao entre esses usuarios.");

            var amizade = new Amizade(request.UsuarioId, request.AmigoId, "Pendente");

            _context.Amizades.Add(amizade);
            await _context.SaveChangesAsync();

            var response = new AmizadeResponse
            {
                Id = amizade.Id,
                UsuarioId = amizade.UsuarioId,
                AmigoId = amizade.AmigoId,
                Status = amizade.Status
            };

            return OperationResult<AmizadeResponse>.Ok(response);
        }

        public async Task<OperationResult> AceitarAsync(int id)
        {
            var amizade = await _context.Amizades.FindAsync(id);
            if (amizade == null)
                return OperationResult.NotFoundResult();

            // Apenas pendente pode aceitar.
            var ok = amizade.Aceitar();
            if (!ok)
                return OperationResult.Fail("Apenas solicitacoes pendentes podem ser aceitas.");

            await _context.SaveChangesAsync();
            return OperationResult.Ok();
        }

        public async Task<OperationResult> RecusarAsync(int id)
        {
            var amizade = await _context.Amizades.FindAsync(id);
            if (amizade == null)
                return OperationResult.NotFoundResult();

            // Apenas pendente pode recusar.
            var ok = amizade.Recusar();
            if (!ok)
                return OperationResult.Fail("Apenas solicitacoes pendentes podem ser recusadas.");

            await _context.SaveChangesAsync();
            return OperationResult.Ok();
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            var amizade = await _context.Amizades.FindAsync(id);
            if (amizade == null)
                return OperationResult.NotFoundResult();

            _context.Amizades.Remove(amizade);
            await _context.SaveChangesAsync();

            return OperationResult.Ok();
        }
    }
}

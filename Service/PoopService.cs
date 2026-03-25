using Microsoft.EntityFrameworkCore;
using SocialCrap.Data;
using SocialCrap.DTOs;
using SocialCrap.Models;

namespace SocialCrap.Service
{
    // Regras de negocio para Poop.
    public class PoopService : IPoopService
    {
        private readonly AppDbContext _context;

        public PoopService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PoopResponse>> GetAllAsync()
        {
            // Retorna lista simples sem joins pesados.
            return await _context.Poops
                .Select(p => new PoopResponse
                {
                    Id = p.Id,
                    UsuarioId = p.UsuarioId,
                    CrapId = p.CrapId,
                    Data = p.Data
                })
                .ToListAsync();
        }

        public async Task<OperationResult<PoopResponse>> GetByIdAsync(int id)
        {
            var poop = await _context.Poops
                .Where(p => p.Id == id)
                .Select(p => new PoopResponse
                {
                    Id = p.Id,
                    UsuarioId = p.UsuarioId,
                    CrapId = p.CrapId,
                    Data = p.Data
                })
                .FirstOrDefaultAsync();

            if (poop == null)
                return OperationResult<PoopResponse>.NotFoundResult();

            return OperationResult<PoopResponse>.Ok(poop);
        }

        public async Task<OperationResult<PoopResponse>> CreateAsync(PoopCreateRequest request)
        {
            // Valida ids basicos.
            if (request.UsuarioId <= 0 || request.CrapId <= 0)
                return OperationResult<PoopResponse>.Fail("UsuarioId e CrapId sao obrigatorios.");

            // Garante usuario e crap existentes.
            var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.Id == request.UsuarioId);
            if (!usuarioExiste)
                return OperationResult<PoopResponse>.Fail("Usuario nao encontrado.");

            var crapExiste = await _context.Craps.AnyAsync(c => c.Id == request.CrapId);
            if (!crapExiste)
                return OperationResult<PoopResponse>.Fail("Crap nao encontrado.");

            // Evita poop duplicado do mesmo usuario no mesmo crap.
            var jaExiste = await _context.Poops.AnyAsync(p => p.UsuarioId == request.UsuarioId && p.CrapId == request.CrapId);
            if (jaExiste)
                return OperationResult<PoopResponse>.Fail("Poop ja registrado para este usuario e crap.");

            var poop = new Poop(request.UsuarioId, request.CrapId);

            _context.Poops.Add(poop);
            await _context.SaveChangesAsync();

            var response = new PoopResponse
            {
                Id = poop.Id,
                UsuarioId = poop.UsuarioId,
                CrapId = poop.CrapId,
                Data = poop.Data
            };

            return OperationResult<PoopResponse>.Ok(response);
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            var poop = await _context.Poops.FindAsync(id);
            if (poop == null)
                return OperationResult.NotFoundResult();

            _context.Poops.Remove(poop);
            await _context.SaveChangesAsync();

            return OperationResult.Ok();
        }
    }
}

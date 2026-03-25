using Microsoft.EntityFrameworkCore;
using SocialCrap.Data;
using SocialCrap.DTOs;
using SocialCrap.Models;
using System.Net.Mail;

namespace SocialCrap.Service
{
    // Regras de negocio para Usuario.
    public class UsuarioService : IUsuarioService
    {
        private readonly AppDbContext _context;

        public UsuarioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Usuario>> GetAllAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<OperationResult<Usuario>> GetByIdAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return OperationResult<Usuario>.NotFoundResult();

            return OperationResult<Usuario>.Ok(usuario);
        }

        public async Task<OperationResult<Usuario>> CreateAsync(UsuarioCreateRequest request)
        {
            // Valida campos obrigatorios.
            if (string.IsNullOrWhiteSpace(request.Nome) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Senha))
            {
                return OperationResult<Usuario>.Fail("Nome, email e senha sao obrigatorios.");
            }

            // Valida formato de email.
            if (!IsValidEmail(request.Email))
                return OperationResult<Usuario>.Fail("Email invalido.");

            var emailNormalizado = request.Email.Trim().ToLowerInvariant();

            // Evita duplicidade de email.
            var emailExiste = await _context.Usuarios.AnyAsync(u => u.Email.ToLower() == emailNormalizado);
            if (emailExiste)
                return OperationResult<Usuario>.Fail("Email ja cadastrado.");

            var usuario = new Usuario(request.Nome.Trim(), emailNormalizado, request.Senha);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return OperationResult<Usuario>.Ok(usuario);
        }

        public async Task<OperationResult<Usuario>> UpdateAsync(int id, UsuarioUpdateRequest request)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return OperationResult<Usuario>.NotFoundResult();

            // Valida dados basicos.
            if (string.IsNullOrWhiteSpace(request.Nome) || string.IsNullOrWhiteSpace(request.Email))
                return OperationResult<Usuario>.Fail("Nome e email sao obrigatorios.");

            if (!IsValidEmail(request.Email))
                return OperationResult<Usuario>.Fail("Email invalido.");

            var emailNormalizado = request.Email.Trim().ToLowerInvariant();

            // Evita trocar para email ja existente.
            var emailExiste = await _context.Usuarios.AnyAsync(u => u.Id != id && u.Email.ToLower() == emailNormalizado);
            if (emailExiste)
                return OperationResult<Usuario>.Fail("Email ja cadastrado.");

            var perfilAtualizado = usuario.AtualizarPerfil(request.Nome.Trim(), emailNormalizado);
            if (!perfilAtualizado)
                return OperationResult<Usuario>.Fail("Nome e email invalidos.");

            await _context.SaveChangesAsync();

            return OperationResult<Usuario>.Ok(usuario);
        }

        public async Task<OperationResult> UpdateSenhaAsync(int id, UsuarioSenhaRequest request)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return OperationResult.NotFoundResult();

            // Usa regra de dominio para validar senha atual e aplicar hash.
            var senhaAtualizada = usuario.AtualizarSenha(request.SenhaAtual, request.NovaSenha);
            if (!senhaAtualizada)
                return OperationResult.Fail("Senha atual incorreta ou nova senha invalida.");

            await _context.SaveChangesAsync();

            return OperationResult.Ok();
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return OperationResult.NotFoundResult();

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return OperationResult.Ok();
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var _ = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

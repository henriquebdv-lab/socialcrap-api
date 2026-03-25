using Microsoft.EntityFrameworkCore;
using SocialCrap.Data;
using SocialCrap.DTOs;
using SocialCrap.Service;

namespace SocialCrap.Service
{
    // Regras de negocio para autenticacao.
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<AuthResponse>> LoginAsync(UsuarioLoginRequest request)
        {
            // Validacao basica dos campos.
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Senha))
                return OperationResult<AuthResponse>.Fail("Email e senha sao obrigatorios.");

            var emailNormalizado = request.Email.Trim().ToLowerInvariant();

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email.ToLower() == emailNormalizado);
            if (usuario == null)
                return OperationResult<AuthResponse>.Fail("Email ou senha invalidos.");

            // Verifica o hash salvo no banco.
            var senhaOk = PasswordHasher.Verify(request.Senha, usuario.Senha);
            if (!senhaOk)
                return OperationResult<AuthResponse>.Fail("Email ou senha invalidos.");

            var response = new AuthResponse
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email
            };

            return OperationResult<AuthResponse>.Ok(response);
        }
    }
}

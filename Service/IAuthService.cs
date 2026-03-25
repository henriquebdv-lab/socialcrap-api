using SocialCrap.DTOs;

namespace SocialCrap.Service
{
    // Contrato da camada de servico para autenticacao.
    public interface IAuthService
    {
        // Valida email e senha e retorna dados basicos do usuario.
        Task<OperationResult<AuthResponse>> LoginAsync(UsuarioLoginRequest request);
    }
}

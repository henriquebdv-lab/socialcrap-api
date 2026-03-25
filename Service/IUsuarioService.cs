using SocialCrap.DTOs;
using SocialCrap.Models;

namespace SocialCrap.Service
{
    // Contrato da camada de servico para Usuario.
    public interface IUsuarioService
    {
        // Lista todos os usuarios.
        Task<List<Usuario>> GetAllAsync();
        // Busca usuario por id.
        Task<OperationResult<Usuario>> GetByIdAsync(int id);
        // Cria usuario com validacoes.
        Task<OperationResult<Usuario>> CreateAsync(UsuarioCreateRequest request);
        // Atualiza perfil (nome e email).
        Task<OperationResult<Usuario>> UpdateAsync(int id, UsuarioUpdateRequest request);
        // Atualiza senha do usuario.
        Task<OperationResult> UpdateSenhaAsync(int id, UsuarioSenhaRequest request);
        // Remove usuario.
        Task<OperationResult> DeleteAsync(int id);
    }
}

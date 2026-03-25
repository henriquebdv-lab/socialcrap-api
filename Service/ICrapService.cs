using SocialCrap.DTOs;
using SocialCrap.Models;

namespace SocialCrap.Service
{
    // Contrato da camada de servico para Crap.
    public interface ICrapService
    {
        // Lista todos os craps com dados do usuario.
        Task<List<CrapResponse>> GetAllAsync();
        // Busca um crap por id.
        Task<OperationResult<CrapResponse>> GetByIdAsync(int id);
        // Cria um crap validando regras basicas.
        Task<OperationResult<CrapResponse>> CreateAsync(CrapCreateRequest request);
        // Atualiza a mensagem de um crap.
        Task<OperationResult<Crap>> UpdateAsync(int id, CrapUpdateRequest request);
        // Remove um crap existente.
        Task<OperationResult> DeleteAsync(int id);
    }
}

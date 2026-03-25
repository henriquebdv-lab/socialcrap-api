using SocialCrap.DTOs;

namespace SocialCrap.Service
{
    // Contrato da camada de servico para Amizade.
    public interface IAmizadeService
    {
        // Lista todas as amizades.
        Task<List<AmizadeResponse>> GetAllAsync();
        // Busca amizade por id.
        Task<OperationResult<AmizadeResponse>> GetByIdAsync(int id);
        // Envia pedido de amizade.
        Task<OperationResult<AmizadeResponse>> CreateAsync(AmizadeCreateRequest request);
        // Aceita pedido.
        Task<OperationResult> AceitarAsync(int id);
        // Recusa pedido.
        Task<OperationResult> RecusarAsync(int id);
        // Remove amizade.
        Task<OperationResult> DeleteAsync(int id);
    }
}

using SocialCrap.DTOs;

namespace SocialCrap.Service
{
    // Contrato da camada de servico para Poop.
    public interface IPoopService
    {
        // Lista todos os poops.
        Task<List<PoopResponse>> GetAllAsync();
        // Busca poop por id.
        Task<OperationResult<PoopResponse>> GetByIdAsync(int id);
        // Cria poop com validacoes.
        Task<OperationResult<PoopResponse>> CreateAsync(PoopCreateRequest request);
        // Remove poop.
        Task<OperationResult> DeleteAsync(int id);
    }
}

using SocialCrap.DTOs;
using SocialCrap.Models;

namespace SocialCrap.Service
{
    // Contrato da camada de servico para Noticia.
    public interface INoticiaService
    {
        // Lista todas as noticias.
        Task<List<NoticiaResponse>> GetAllAsync();
        // Busca noticia por id.
        Task<OperationResult<NoticiaResponse>> GetByIdAsync(int id);
        // Cria noticia com validacoes.
        Task<OperationResult<NoticiaResponse>> CreateAsync(NoticiaCreateRequest request);
        // Atualiza titulo e conteudo.
        Task<OperationResult<Noticia>> UpdateAsync(int id, NoticiaUpdateRequest request);
        // Remove noticia.
        Task<OperationResult> DeleteAsync(int id);
    }
}

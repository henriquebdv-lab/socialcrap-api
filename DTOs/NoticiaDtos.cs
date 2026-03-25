using System;

namespace SocialCrap.DTOs
{
    // Corpo da requisicao para criar uma noticia.
    public class NoticiaCreateRequest
    {
        public string Titulo { get; set; } = string.Empty;
        public string Conteudo { get; set; } = string.Empty;
        public int AutorId { get; set; }
    }

    // Corpo da requisicao para atualizar uma noticia.
    public class NoticiaUpdateRequest
    {
        public string Titulo { get; set; } = string.Empty;
        public string Conteudo { get; set; } = string.Empty;
    }

    // Payload de resposta para listagem e detalhe.
    public class NoticiaResponse
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Conteudo { get; set; } = string.Empty;
        public DateTime Data { get; set; }
        public int AutorId { get; set; }
        public string AutorNome { get; set; } = string.Empty;
    }
}

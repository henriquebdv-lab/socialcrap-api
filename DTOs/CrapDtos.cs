using System;

namespace SocialCrap.DTOs
{
    // Corpo da requisicao para criar um crap.
    public class CrapCreateRequest
    {
        public string Mensagem { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
    }

    // Corpo da requisicao para atualizar um crap.
    public class CrapUpdateRequest
    {
        public string Mensagem { get; set; } = string.Empty;
    }

    // Payload de resposta para listagem e detalhe.
    public class CrapResponse
    {
        public int Id { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public DateTime Data { get; set; }
        public int UsuarioId { get; set; }
        public string UsuarioNome { get; set; } = string.Empty;
    }
}

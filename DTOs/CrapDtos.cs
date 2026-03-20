using System;

namespace SocialCrap.DTOs
{
    // Corpo da requisição para criar um crap.
    public class CrapCreateRequest
    {
        public string Mensagem { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
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

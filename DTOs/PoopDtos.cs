using System;

namespace SocialCrap.DTOs
{
    // Corpo da requisicao para criar um poop.
    public class PoopCreateRequest
    {
        public int UsuarioId { get; set; }
        public int CrapId { get; set; }
    }

    // Payload de resposta para listagem e detalhe.
    public class PoopResponse
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int CrapId { get; set; }
        public DateTime Data { get; set; }
    }
}

namespace SocialCrap.DTOs
{
    // Corpo da requisicao para criar amizade.
    public class AmizadeCreateRequest
    {
        public int UsuarioId { get; set; }
        public int AmigoId { get; set; }
    }

    // Payload de resposta para listagem e detalhe.
    public class AmizadeResponse
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int AmigoId { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}

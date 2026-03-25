namespace SocialCrap.Service
{
    // Resposta simples de autenticacao.
    public class AuthResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}

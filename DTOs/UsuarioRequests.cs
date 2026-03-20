namespace SocialCrap.DTOs
{
    // Corpo da requisição para criar um usuário.
    public class UsuarioCreateRequest
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }

    // Corpo da requisição para atualizar dados básicos do perfil.
    public class UsuarioUpdateRequest
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    // Corpo da requisição para trocar a senha do usuário.
    public class UsuarioSenhaRequest
    {
        public string SenhaAtual { get; set; } = string.Empty;
        public string NovaSenha { get; set; } = string.Empty;
    }
}

using System;
using System.Collections.Generic;

namespace SocialCrap.Models
{
    // Entidade de post simples.
    public class Crap
    {
        public int Id { get; private set; }

        public string Mensagem { get; private set; } = string.Empty;

        public DateTime Data { get; private set; }

        public int UsuarioId { get; set; }

        public Usuario Usuario { get; set; } = null!;

        public List<Poop> Poops { get; set; } = new();

        // Construtor do EF
        public Crap() { }

        // Construtor de domínio
        public Crap(string mensagem, int usuarioId)
        {
            Mensagem = mensagem;
            UsuarioId = usuarioId;
            Data = DateTime.UtcNow;
        }

        public bool AtualizarMensagem(string mensagem)
        {
            if (string.IsNullOrWhiteSpace(mensagem))
                return false;

            Mensagem = mensagem;
            return true;
        }
    }
}

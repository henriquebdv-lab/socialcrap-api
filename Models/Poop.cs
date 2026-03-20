using System;

namespace SocialCrap.Models
{
    // Entidade de reação ligada a Crap e Usuário.
    public class Poop
    {
        public int Id { get; private set; }

        public int UsuarioId { get; set; }

        public int CrapId { get; set; }

        public DateTime Data { get; private set; }

        // Navegação
        public Usuario Usuario { get; set; } = null!;

        public Crap Crap { get; set; } = null!;

        // Construtor do EF
        public Poop() { }

        // Construtor de domínio
        public Poop(int usuarioId, int crapId)
        {
            UsuarioId = usuarioId;
            CrapId = crapId;
            Data = DateTime.UtcNow;
        }
    }
}

using System;

namespace SocialCrap.Models
{
    // Relação de amizade entre dois usuários.
    public class Amizade
    {
        public int Id { get; private set; }
        public int UsuarioId { get; private set; }
        public int AmigoId { get; private set; }
        public string Status { get; private set; } = string.Empty;

        public Usuario Usuario { get; private set; } = null!;
        public Usuario Amigo { get; private set; } = null!;

        public Amizade()
        {
        }

        public Amizade(int usuarioId, int amigoId, string status)
        {
            UsuarioId = usuarioId;
            AmigoId = amigoId;
            Status = status;
        }

        public bool EnviarPedido()
        {
            if (UsuarioId <= 0 || AmigoId <= 0 || Usuario == null || Amigo == null)
                return false;

            Status = "Pendente";
            // TODO: persistir o pedido no banco
            return true;
        }

        public bool Aceitar()
        {
            if (Status != "Pendente")
                return false;

            Status = "Aceita";
            // TODO: atualizar status no banco
            return true;
        }

        public bool Recusar()
        {
            if (Status != "Pendente")
                return false;

            Status = "Recusada";
            // TODO: atualizar status no banco
            return true;
        }
    }
}

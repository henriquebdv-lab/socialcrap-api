using System;

namespace SocialCrap.Models
{
    // Notícia publicada por um usuário.
    public class Noticia
    {
        public int Id { get; private set; }

        public string Titulo { get; private set; } = string.Empty;

        public string Conteudo { get; private set; } = string.Empty;

        public DateTime Data { get; private set; }

        public int AutorId { get; set; }

        public Usuario Autor { get; set; } = null!;

        // Construtor do EF
        public Noticia() { }

        // Construtor de domínio
        public Noticia(string titulo, string conteudo, int autorId)
        {
            Titulo = titulo;
            Conteudo = conteudo;
            AutorId = autorId;
            Data = DateTime.UtcNow;
        }

        public bool Atualizar(string titulo, string conteudo)
        {
            if (string.IsNullOrWhiteSpace(titulo) || string.IsNullOrWhiteSpace(conteudo))
                return false;

            Titulo = titulo;
            Conteudo = conteudo;
            return true;
        }
    }
}

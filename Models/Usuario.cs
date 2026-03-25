using System;
using System.Collections.Generic;
using SocialCrap.Service;

namespace SocialCrap.Models
{
    // Raiz de agregado do usuario.
    public class Usuario
    {
        public int Id { get; private set; }

        public string Nome { get; private set; } = "";

        public string Email { get; private set; } = "";

        public string Senha { get; private set; } = "";

        // Relacionamentos
        public List<Crap> Craps { get; set; } = new();
        public List<Noticia> Noticias { get; set; } = new();
        public List<Poop> Poops { get; set; } = new();
        public List<Amizade> AmizadesEnviadas { get; set; } = new();
        public List<Amizade> AmizadesRecebidas { get; set; } = new();

        // Construtor do EF
        public Usuario() { }

        // Construtor de dominio
        public Usuario(string nome, string email, string senha)
        {
            Nome = nome;
            Email = email;
            Senha = PasswordHasher.Hash(senha);
        }

        public bool AtualizarPerfil(string nome, string email)
        {
            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(email))
                return false;

            Nome = nome;
            Email = email;
            return true;
        }

        public bool AtualizarSenha(string senhaAtual, string novaSenha)
        {
            if (!PasswordHasher.Verify(senhaAtual, Senha) || string.IsNullOrWhiteSpace(novaSenha))
                return false;

            Senha = PasswordHasher.Hash(novaSenha);
            return true;
        }
    }
}

"use client";

import { useEffect, useState } from "react";
import styles from "./page.module.css";

// URL base da API, pode ser sobrescrita via NEXT_PUBLIC_API_URL.
const API_URL = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5000";

function useUsuarioLogado() {
  const [logado, setLogado] = useState(false);

  useEffect(() => {
    try {
      const salvo = localStorage.getItem("usuarioLogado");
      if (!salvo) {
        setLogado(false);
        return;
      }

      const usuario = JSON.parse(salvo);
      setLogado(Boolean(usuario && usuario.nome));
    } catch {
      setLogado(false);
    }
  }, []);

  return logado;
}

export default function Cadastro() {
  // Campos do formulario.
  const [nome, setNome] = useState("");
  const [email, setEmail] = useState("");
  const [senha, setSenha] = useState("");
  const [confirmarSenha, setConfirmarSenha] = useState("");

  // Estados de feedback.
  const [loading, setLoading] = useState(false);
  const [erro, setErro] = useState("");
  const [sucesso, setSucesso] = useState("");

  const usuarioLogado = useUsuarioLogado();

  // Validacoes locais simples.
  function validarFormulario() {
    if (!nome.trim() || !email.trim() || !senha.trim() || !confirmarSenha.trim()) {
      return "Preencha nome, email e senha.";
    }

    if (!email.includes("@") || !email.includes(".")) {
      return "Email invalido.";
    }

    if (senha.length < 6) {
      return "Senha deve ter pelo menos 6 caracteres.";
    }

    if (senha !== confirmarSenha) {
      return "As senhas nao conferem.";
    }

    return "";
  }

  async function cadastrar(event) {
    event.preventDefault();
    setErro("");
    setSucesso("");

    const erroValidacao = validarFormulario();
    if (erroValidacao) {
      setErro(erroValidacao);
      return;
    }

    setLoading(true);

    try {
      const response = await fetch(`${API_URL}/api/usuario`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          nome: nome.trim(),
          email: email.trim(),
          senha: senha.trim(),
        }),
      });

      if (!response.ok) {
        const text = await response.text();
        throw new Error(text || "Erro ao cadastrar.");
      }

      setNome("");
      setEmail("");
      setSenha("");
      setConfirmarSenha("");
      setSucesso("Cadastro realizado com sucesso.");
    } catch (err) {
      setErro(err instanceof Error ? err.message : "Erro inesperado.");
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className={styles.page}>
      <header className={styles.topbar}>
        <div className={styles.topLeft}>
          <div className={styles.logo}>💩 SocialCrap</div>
          <nav className={styles.topLinks}>
            <a href="/">Inicio</a>
            {!usuarioLogado && <a href="/cadastro">Cadastro</a>}
            {!usuarioLogado && <a href="/login">Login</a>}
          </nav>
        </div>
      </header>

      <main className={styles.content}>
        <section className={styles.card}>
          <h1>Cadastro de Usuario</h1>
          <p className={styles.subtitle}>
            Crie sua conta para postar e interagir.
          </p>

          <form onSubmit={cadastrar} className={styles.form}>
            <label className={styles.label}>
              Nome
              <input
                className={styles.inputFull}
                value={nome}
                onChange={(e) => setNome(e.target.value)}
                placeholder="Seu nome"
              />
            </label>

            <label className={styles.label}>
              Email
              <input
                className={styles.inputFull}
                type="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                placeholder="voce@email.com"
              />
            </label>

            <label className={styles.label}>
              Senha
              <input
                className={styles.inputFull}
                type="password"
                value={senha}
                onChange={(e) => setSenha(e.target.value)}
                placeholder="Crie uma senha"
              />
            </label>

            <label className={styles.label}>
              Confirmar senha
              <input
                className={styles.inputFull}
                type="password"
                value={confirmarSenha}
                onChange={(e) => setConfirmarSenha(e.target.value)}
                placeholder="Repita a senha"
              />
            </label>

            <div className={styles.buttonRow}>
              <button className={styles.button} type="submit" disabled={loading}>
                {loading ? "Enviando..." : "Cadastrar"}
              </button>
              <a className={styles.link} href="/">
                Voltar
              </a>
            </div>
          </form>

          {erro && <p className={`${styles.status} ${styles.statusError}`}>{erro}</p>}
          {sucesso && <p className={`${styles.status} ${styles.statusSuccess}`}>{sucesso}</p>}
        </section>
      </main>
    </div>
  );
}

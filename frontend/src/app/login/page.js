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

export default function Login() {
  // Campos do formulario.
  const [email, setEmail] = useState("");
  const [senha, setSenha] = useState("");

  // Estados de feedback.
  const [loading, setLoading] = useState(false);
  const [erro, setErro] = useState("");
  const [sucesso, setSucesso] = useState("");

  const usuarioLogado = useUsuarioLogado();

  async function entrar(event) {
    event.preventDefault();
    setErro("");
    setSucesso("");

    // Validacao simples antes de enviar.
    if (!email.trim() || !senha.trim()) {
      setErro("Preencha email e senha.");
      return;
    }

    setLoading(true);

    try {
      const response = await fetch(`${API_URL}/api/auth/login`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          email: email.trim(),
          senha: senha.trim(),
        }),
      });

      if (!response.ok) {
        const text = await response.text();
        throw new Error(text || "Erro ao entrar.");
      }

      const data = await response.json();

      // Salva dados basicos do usuario no navegador.
      localStorage.setItem(
        "usuarioLogado",
        JSON.stringify({
          id: data.id,
          nome: data.nome,
          email: data.email,
        })
      );

      setSucesso(`Bem-vindo, ${data.nome}.`);
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
          <h1>Login</h1>
          <p className={styles.subtitle}>Entre para acessar sua conta.</p>

          <form onSubmit={entrar} className={styles.form}>
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
                placeholder="Sua senha"
              />
            </label>

            <div className={styles.buttonRow}>
              <button className={styles.button} type="submit" disabled={loading}>
                {loading ? "Entrando..." : "Entrar"}
              </button>
              <a className={styles.link} href="/cadastro">
                Criar conta
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

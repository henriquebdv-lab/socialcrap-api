"use client";

import { useEffect, useState } from "react";
import styles from "./page.module.css";

// URL base da API, pode ser sobrescrita via NEXT_PUBLIC_API_URL.
const API_URL = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5000";

export default function Home() {
  // Estado principal da tela.
  const [craps, setCraps] = useState([]);
  const [mensagem, setMensagem] = useState("");
  const [usuarioId, setUsuarioId] = useState("");
  const [loading, setLoading] = useState(false);
  const [erro, setErro] = useState("");
  const [sucesso, setSucesso] = useState("");
  const [poopsMap, setPoopsMap] = useState({});
  const [poopsDoUsuario, setPoopsDoUsuario] = useState(new Set());

  // Usuario exibido no painel esquerdo (vem do localStorage).
  const [usuarioAtual, setUsuarioAtual] = useState({
    nome: "Visitante",
    idade: "-",
    cidade: "-",
    status: "offline",
  });

  // Lista de blocos decorativos para o painel direito.
  const blocos = ["Amigos", "Fotos", "Craps", "Jogos (em construcao)", "teste"];

  // Busca lista inicial ao abrir a pagina.
  useEffect(() => {
    carregarUsuarioLocal();
  }, []);

  useEffect(() => {
    carregarCraps();
  }, [usuarioId]);

  function carregarUsuarioLocal() {
    try {
      const salvo = localStorage.getItem("usuarioLogado");
      if (!salvo) return;

      const usuario = JSON.parse(salvo);
      if (usuario && usuario.nome) {
        setUsuarioAtual({
          nome: usuario.nome,
          idade: usuario.idade || "-",
          cidade: usuario.cidade || "-",
          status: "online",
        });

        if (usuario.id) {
          setUsuarioId(String(usuario.id));
        }
      }
    } catch {
      // Ignora caso esteja corrompido.
    }
  }

  function logout() {
    localStorage.removeItem("usuarioLogado");
    setUsuarioAtual({
      nome: "Visitante",
      idade: "-",
      cidade: "-",
      status: "offline",
    });
    setUsuarioId("");
    setPoopsDoUsuario(new Set());
  }

  async function carregarCraps() {
    setLoading(true);
    setErro("");
    setSucesso("");

    try {
      const response = await fetch(`${API_URL}/api/crap`);
      if (!response.ok) {
        throw new Error("Falha ao carregar craps.");
      }

      const data = await response.json();
      setCraps(Array.isArray(data) ? data : []);

      // Carrega poops para montar contagem por crap.
      await carregarPoops();
    } catch (err) {
      setErro(err instanceof Error ? err.message : "Erro inesperado.");
    } finally {
      setLoading(false);
    }
  }

  async function carregarPoops() {
    try {
      const response = await fetch(`${API_URL}/api/poop`);
      if (!response.ok) return;

      const data = await response.json();
      if (!Array.isArray(data)) return;

      const mapa = {};
      const doUsuario = new Set();

      data.forEach((poop) => {
        mapa[poop.crapId] = (mapa[poop.crapId] || 0) + 1;
        if (usuarioId && String(poop.usuarioId) === String(usuarioId)) {
          doUsuario.add(poop.crapId);
        }
      });

      setPoopsMap(mapa);
      setPoopsDoUsuario(doUsuario);
    } catch {
      // Se falhar, apenas ignora a contagem.
    }
  }

  async function criarCrap(event) {
    event.preventDefault();
    setErro("");
    setSucesso("");

    // Validacao simples no front para evitar envio vazio.
    if (!mensagem.trim()) {
      setErro("Informe uma mensagem.");
      return;
    }

    if (!usuarioId || Number(usuarioId) <= 0) {
      setErro("Faca login para publicar.");
      return;
    }

    try {
      const response = await fetch(`${API_URL}/api/crap`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          mensagem: mensagem.trim(),
          usuarioId: Number(usuarioId),
        }),
      });

      if (!response.ok) {
        const text = await response.text();
        throw new Error(text || "Erro ao criar crap.");
      }

      setMensagem("");
      setSucesso("Crap criado com sucesso.");
      await carregarCraps();
    } catch (err) {
      setErro(err instanceof Error ? err.message : "Erro inesperado.");
    }
  }

  async function darPoop(crapId) {
    setErro("");
    setSucesso("");

    if (!usuarioId || Number(usuarioId) <= 0) {
      setErro("Faca login para dar poop.");
      return;
    }

    if (poopsDoUsuario.has(crapId)) {
      setErro("Voce ja deu poop neste crap.");
      return;
    }

    try {
      const response = await fetch(`${API_URL}/api/poop`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          usuarioId: Number(usuarioId),
          crapId: Number(crapId),
        }),
      });

      if (!response.ok) {
        const text = await response.text();
        throw new Error(text || "Erro ao dar poop.");
      }

      setSucesso("Poop registrado.");
      await carregarPoops();
    } catch (err) {
      setErro(err instanceof Error ? err.message : "Erro inesperado.");
    }
  }

  const usuarioLogado = usuarioAtual.status === "online";

  return (
    <div className={styles.page}>
      <header className={styles.topbar}>
        <div className={styles.topLeft}>
          <div className={styles.logo}>💩 SocialCrap</div>
          <nav className={styles.topLinks}>
            <a href="#feed">Inicio</a>
            <a href="#novo">Novo crap</a>
            <a href="#feed">Feed</a>
            {!usuarioLogado && <a href="/cadastro">Cadastro</a>}
            {!usuarioLogado && <a href="/login">Login</a>}
          </nav>
        </div>
      </header>

      <main className={styles.layout}>
        <aside className={styles.sidebarLeft}>
          <section className={styles.profileCard}>
            <div className={styles.avatar} />
            <h2 className={styles.profileName}>{usuarioAtual.nome}</h2>
            <ul className={styles.profileInfo}>
              <li>Idade: {usuarioAtual.idade}</li>
              <li>Cidade: {usuarioAtual.cidade}</li>
              <li>Status: {usuarioAtual.status}</li>
            </ul>
            {usuarioLogado && (
              <button className={styles.logoutBtn} onClick={logout} type="button">
                Sair
              </button>
            )}
          </section>
        </aside>

        <section className={styles.main}>
          <section id="novo" className={styles.card}>
            <h2>Novo Crap</h2>
            <form onSubmit={criarCrap} className={styles.form}>
              <label className={styles.label}>
                Mensagem
                <textarea
                  className={`${styles.inputFull} ${styles.inputTextarea}`}
                  value={mensagem}
                  onChange={(e) => setMensagem(e.target.value)}
                  placeholder="Digite sua mensagem"
                  rows={3}
                />
              </label>

              <div className={styles.buttonRow}>
                <button className={styles.button} type="submit">
                  Publicar
                </button>
                <button
                  className={`${styles.button} ${styles.buttonSecondary}`}
                  type="button"
                  onClick={carregarCraps}
                >
                  Recarregar
                </button>
              </div>
            </form>

            {erro && <p className={`${styles.status} ${styles.statusError}`}>{erro}</p>}
            {sucesso && (
              <p className={`${styles.status} ${styles.statusSuccess}`}>{sucesso}</p>
            )}
          </section>

          <section id="feed" className={styles.card}>
            <div className={styles.feedHeader}>
              <h2>Feed de Craps</h2>
              <span className={styles.counter}>{craps.length}</span>
            </div>

            {loading ? (
              <p className={`${styles.status} ${styles.statusInfo}`}>Carregando...</p>
            ) : (
              <ul className={styles.list}>
                {craps.map((crap) => (
                  <li key={crap.id} className={styles.listItem}>
                    <div className={styles.listMeta}>
                      <span>{crap.usuarioNome}</span>
                      <span>{new Date(crap.data).toLocaleString()}</span>
                    </div>
                    <p className={styles.listMessage}>{crap.mensagem}</p>
                    <div className={styles.poopRow}>
                      <button
                        className={styles.poopButton}
                        onClick={() => darPoop(crap.id)}
                        type="button"
                        disabled={poopsDoUsuario.has(crap.id)}
                      >
                        💩 Poop
                      </button>
                      <span className={styles.poopCount}>
                        {poopsMap[crap.id] || 0}
                      </span>
                    </div>
                  </li>
                ))}
              </ul>
            )}
          </section>
        </section>

        <aside className={styles.sidebarRight}>
          <section className={styles.menuCard}>
            <h3>Menu</h3>
            <ul className={styles.menuList}>
              <li>
                <a href="#">Perfil</a>
              </li>
              <li>
                <a href="#">Amigos</a>
              </li>
              <li>
                <a href="#">Craps</a>
              </li>
              <li>
                <a href="#">Fotos</a>
              </li>
            </ul>
          </section>

          <section className={styles.card}>
            <div className={styles.privIcon} title="Privado">
              🔒
            </div>
            <div className={styles.rectGrid}>
              {blocos.map((item) => (
                <div key={item} className={styles.rectItem}>
                  {item}
                </div>
              ))}
            </div>
          </section>
        </aside>
      </main>
    </div>
  );
}

const API = "http://localhost:5279";
const APIbuscarFilmes = `${API}/api/filmes`;
const APIbuscarResenhas = `${API}/api/resenhas`;
const APIcriarFilmes = `${API}/api/filmes`;

const btn = document.getElementById("btnCarregar");
const btnCriar = document.getElementById("btnCriar");
const lista = document.getElementById("listaFilmes");
const status = document.getElementById("statusMsg");
var inputTitulo = document.getElementById("title");
var director = document.getElementById("director");
var synopsis = document.getElementById("synopsis");
var genre = document.getElementById("genre");
var year = document.getElementById("year");

//Filmes
//TO-DO
//ATUALIZAR E DELETAR FILMES
async function buscarFilmes() {
  status.innerText = "Carregando...";
  lista.innerHTML = "";
  try {
    const response = await fetch(APIbuscarFilmes, {
      method: "GET",
      headers: { "Content-Type": "application/json" },
    });

    if (!response.ok) {
      throw new Error("Erro HTTP: ${response.status}");
    }

    const filmes = await response.json();
    status.innerText = "Sucesso!";
    console.log("Dados de filmes recebidos: ", filmes);
    return filmes;
  } catch (error) {
    console.error("Falha na conexão: ", error);
  }
}

async function criarFilmes() {
  status.innerText = "Carregando...";
  lista.innerHTML = "";
  try {
    const novoFilme = {
      titulo: inputTitulo.value,
      diretor: director.value,
      sinopse: synopsis.value,
      ano: parseInt(year.value),
      genero: genre.value,
      lancado: true,
    };
    const response = await fetch(APIcriarFilmes, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(novoFilme),
    });
    if (response.ok) {
      alert("Filme criado");
      carregarTudo();
    }
  } catch (error) {
    console.error("Erro ao criar filme: ", error);
  }
}

//Resenhas
async function buscarResenhas() {
  status.innerText = "Carregando...";
  lista.innerHTML = "";
  try {
    const response = await fetch(APIbuscarResenhas, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
      },
    });

    if (!response.ok) {
      throw new Error("Erro HTTP: ${response.status}");
    }

    const resenhas = await response.json();
    status.innerText = "Sucesso!";
    console.log("Dados de resenhas recebidos: ", resenhas);
    return resenhas;
  } catch (error) {
    console.error("Falha na conexão: ", error);
  }
}

//TO-DO
//CRIAR,ATUALIZAR E DELETAR RESENHAS

function renderizarFilmesEResenhas(filmes, resenhas) {
  lista.innerHTML = "";
  filmes.forEach((filme) => {
    const card = document.createElement("div");
    card.className = "film-card";

    const resenhasDesteFilme = resenhas.filter((r) => r.filmeId === filme.id);
    card.innerHTML = `
            <h3>${filme.titulo}</h3>
            <p><strong>Diretor:</strong> ${filme.diretor}</p>
            <p><strong>Sinopse:</strong> ${filme.sinopse}</p>
            <hr>
            <div class="container-resenhas">
                <h4>Resenhas:</h4>
                ${
                  resenhasDesteFilme.length > 0
                    ? resenhasDesteFilme
                        .map(
                          (r) => `
                        <div class="resenha-item">
                            <p><strong>${r.autorNome}:</strong> ${r.texto} (Nota: ${r.nota})</p>
                        </div>
                    `,
                        )
                        .join("")
                    : "<p>Ainda não há resenhas para este filme.</p>"
                }
            </div>
        `;

    lista.appendChild(card);
  });
}

async function carregarTudo() {
  const filmes = await buscarFilmes();
  const resenhas = await buscarResenhas();

  if (filmes && resenhas) {
    renderizarFilmesEResenhas(filmes, resenhas);
  }
}

btn.addEventListener("click", carregarTudo);
btnCriar.addEventListener("click", criarFilmes);

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



btn.addEventListener("click", carregarTudo);
btnCriar.addEventListener("click", criarFilmes);
buscarFilmes;
buscarResenhas;
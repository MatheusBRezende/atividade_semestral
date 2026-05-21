# 🎬 Filmes & Resenhas API

API RESTful para gerenciamento de filmes e resenhas, desenvolvida como Trabalho Prático Semestral da disciplina **Arquitetura de Aplicações Web — 2026**.

---

## 📖 Descrição do Projeto

O sistema permite cadastrar filmes e associar resenhas a eles. Cada resenha contém uma nota (de 1 a 5), um texto avaliativo e o nome do autor. A API valida a existência do filme antes de permitir o cadastro de uma resenha, garantindo integridade referencial na camada de serviço.

**Entidades principais:**
- **Filme** — título, diretor, sinopse, ano de lançamento, gênero e status de lançamento.
- **Resenha** — referência ao filme, nome do autor, nota (1–5), texto e data de criação.

---

## 🛠️ Stack Tecnológica

| Camada   | Tecnologia                          |
|----------|-------------------------------------|
| Backend  | .NET 10 (C#) — Minimal API         |
| Banco    | MongoDB                             |
| Docs     | Swagger / OpenAPI (Swashbuckle)     |
| Frontend | HTML + JavaScript (fetch assíncrono)|

---

## ✅ Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [MongoDB](https://www.mongodb.com/try/download/community) rodando localmente **ou** via Docker
- Git

---

## 🚀 Como executar o projeto localmente

### 1. Clone o repositório

```bash
git clone https://github.com/MatheusBRezende/FilmesEResenha
cd FilmesEResenha
```

### 2. Configure as variáveis de ambiente

Na pasta `api/`, crie um arquivo `.env` (ou configure as variáveis no sistema operacional):

```env
MongoDb__ConnectionString=mongodb://localhost:27017
MongoDb__DatabaseName=ResenhasFilmes
```

> ⚠️ Nunca commite valores reais de credenciais. Use o `.env` apenas localmente.

Alternativamente, você pode subir o MongoDB com Docker:

```bash
docker run -d -p 27017:27017 --name mongo-resenhas mongo:latest
```

### 3. Restaure as dependências e execute

```bash
cd api
dotnet restore
dotnet run
```

A API estará disponível em:
- HTTP: `http://localhost:5279`
- HTTPS: `https://localhost:7264`

---

## 📚 Documentação Swagger

Com a aplicação rodando, acesse:

```
http://localhost:5279/swagger
```

A interface lista todos os endpoints com descrições, parâmetros, corpo das requisições e exemplos de resposta.

---

## 🌐 Frontend

O frontend está na pasta `frontend/`. Para utilizá-lo, basta abrir o arquivo `index.html` no navegador (com a API rodando). As chamadas à API são feitas de forma assíncrona via `fetch`, sem recarregar a página.

---

## 🔌 Endpoints da API

### Filmes `/api/filmes`

| Método | Rota               | Descrição                        |
|--------|--------------------|----------------------------------|
| GET    | `/api/filmes`      | Lista todos os filmes            |
| GET    | `/api/filmes/{id}` | Busca um filme pelo ID           |
| POST   | `/api/filmes`      | Cadastra um novo filme           |
| PUT    | `/api/filmes/{id}` | Atualiza um filme pelo ID        |
| DELETE | `/api/filmes/{id}` | Remove um filme pelo ID          |

### Resenhas `/api/resenhas`

| Método | Rota                             | Descrição                            |
|--------|----------------------------------|--------------------------------------|
| GET    | `/api/resenhas`                  | Lista todas as resenhas              |
| GET    | `/api/resenhas/{id}`             | Busca uma resenha pelo ID            |
| GET    | `/api/resenhas/filme/{filmeId}`  | Lista resenhas de um filme específico|
| POST   | `/api/resenhas`                  | Cadastra uma nova resenha            |
| PUT    | `/api/resenhas/{id}`             | Atualiza uma resenha pelo ID         |
| DELETE | `/api/resenhas/{id}`             | Remove uma resenha pelo ID           |

### Utilitários

| Método | Rota              | Descrição                        |
|--------|-------------------|----------------------------------|
| GET    | `/api/mongo-test` | Verifica a conexão com o MongoDB |

---

## 🧱 Estrutura do Projeto

```
/
├── api/
│   ├── Models/
│   │   ├── Filme.cs
│   │   └── Resenha.cs
│   ├── Repositories/
│   │   ├── IFilmeRepository.cs
│   │   ├── FilmeRepository.cs
│   │   ├── IResenhaRepository.cs
│   │   └── ResenhaRepository.cs
│   ├── Services/
│   │   ├── IFilmeService.cs
│   │   ├── FilmeService.cs
│   │   ├── IResenhaService.cs
│   │   └── ResenhaService.cs
│   ├── Program.cs
│   └── AtividadeSemestral.csproj
├── frontend/
│   └── index.html
└── README.md
```

---

## 🔑 Variáveis de Ambiente

| Variável                    | Descrição                              | Exemplo                         |
|-----------------------------|----------------------------------------|---------------------------------|
| `MongoDb__ConnectionString` | URI de conexão com o MongoDB           | `mongodb://localhost:27017`     |
| `MongoDb__DatabaseName`     | Nome do banco de dados a ser utilizado | `ResenhasFilmes`                |

> As variáveis seguem a convenção de configuração hierárquica do ASP.NET Core (`__` como separador de seção).

---

## 📦 Dependências

- [`MongoDB.Driver`](https://www.nuget.org/packages/MongoDB.Driver/) `3.8.0`
- [`Swashbuckle.AspNetCore`](https://www.nuget.org/packages/Swashbuckle.AspNetCore/) `10.1.7`
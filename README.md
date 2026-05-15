# 🎬 Filmes & Resenhas API

> API REST para gerenciamento de filmes e resenhas, desenvolvida em .NET com persistência no MongoDB.

---

## 📋 Descrição

O **Filmes & Resenhas** é uma aplicação web que permite cadastrar filmes e associar resenhas a eles. O sistema expõe uma API REST completa com operações de CRUD para as duas entidades principais — **Filme** e **Resenha** — além de uma página web que consome a API de forma assíncrona.

**Domínio:** Plataforma de avaliação de filmes, onde usuários podem cadastrar filmes e escrever resenhas com notas de 1 a 5.

---

## 🏗️ Arquitetura

```
Client (Frontend)
      │
      ▼
 Minimal API (.NET 10)
  ├── /api/filmes   → FilmeService → FilmeRepository
  └── /api/resenhas → ResenhaService → ResenhaRepository
                                              │
                                              ▼
                                         MongoDB Atlas
                                     (filmes / resenhas)
```

A aplicação segue o padrão de camadas:

| Camada       | Responsabilidade                              |
|--------------|-----------------------------------------------|
| **Endpoints**    | Recebe requisições HTTP, retorna respostas    |
| **Service**      | Regras de negócio e validações               |
| **Repository**   | Acesso e persistência no banco de dados       |
| **Models**       | Definição das entidades e validações          |

---

## 🛠️ Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [MongoDB](https://www.mongodb.com/) (local ou Atlas)
- [Git](https://git-scm.com/)

---

## 🚀 Como executar localmente

### 1. Clone o repositório

```bash
git clone https://github.com/seu-usuario/FilmesEResenha
cd FilmesEResenha
```

### 2. Configure as variáveis de ambiente

Crie um arquivo `.env` ou configure as variáveis de ambiente do sistema. Você pode usar o arquivo `.env` de exemplo como base (veja a seção [Variáveis de Ambiente](#-variáveis-de-ambiente)).

No Linux/macOS:
```bash
export MongoDb__ConnectionString="mongodb://localhost:27017"
export MongoDb__DatabaseName="filmesresenha"
```

No Windows (PowerShell):
```powershell
$env:MongoDb__ConnectionString="mongodb://localhost:27017"
$env:MongoDb__DatabaseName="filmesresenha"
```

Alternativamente, configure em `appsettings.json` (não commite valores reais):
```json
{
  "MongoDb": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "filmesresenha"
  }
}
```

### 3. Restaure as dependências

```bash
dotnet restore
```

### 4. Execute a aplicação

```bash
dotnet run
```

A API estará disponível em:
- HTTP: `http://localhost:5279`
- HTTPS: `https://localhost:7264`

---

## 📖 Documentação Swagger

Com a aplicação rodando, acesse:

```
http://localhost:5279/swagger
```

A interface do Swagger permite explorar e testar todos os endpoints interativamente, com descrições, parâmetros, exemplos de corpo de requisição e respostas esperadas.

---

## 📡 Endpoints da API

### 🎬 Filmes — `/api/filmes`

| Método | Rota                    | Descrição                          | Status       |
|--------|-------------------------|------------------------------------|--------------|
| GET    | `/api/filmes`           | Lista todos os filmes              | 200          |
| GET    | `/api/filmes/{id}`      | Busca um filme pelo ID             | 200, 404     |
| POST   | `/api/filmes`           | Cria um novo filme                 | 201, 400     |
| PUT    | `/api/filmes/{id}`      | Atualiza um filme existente        | 204, 400, 404|
| DELETE | `/api/filmes/{id}`      | Remove um filme pelo ID            | 204, 404     |

### 📝 Resenhas — `/api/resenhas`

| Método | Rota                             | Descrição                            | Status       |
|--------|----------------------------------|--------------------------------------|--------------|
| GET    | `/api/resenhas`                  | Lista todas as resenhas              | 200          |
| GET    | `/api/resenhas/{id}`             | Busca uma resenha pelo ID            | 200, 404     |
| GET    | `/api/resenhas/filme/{filmeId}`  | Lista resenhas de um filme específico| 200          |
| POST   | `/api/resenhas`                  | Cria uma nova resenha                | 201, 400     |
| PUT    | `/api/resenhas/{id}`             | Atualiza uma resenha existente       | 204, 400, 404|
| DELETE | `/api/resenhas/{id}`             | Remove uma resenha pelo ID           | 204, 404     |

### 🔧 Utilitários

| Método | Rota              | Descrição                        |
|--------|-------------------|----------------------------------|
| GET    | `/api/mongo-test` | Verifica a conexão com o MongoDB |

---

## 📦 Exemplos de Uso

### Criar um Filme

**Request:**
```http
POST /api/filmes
Content-Type: application/json

{
  "titulo": "Interestelar",
  "diretor": "Christopher Nolan",
  "sinopse": "Uma equipe de exploradores viaja através de um buraco de minhoca no espaço.",
  "ano": 2014,
  "genero": "Ficção Científica",
  "lancado": true
}
```

**Response `201 Created`:**
```json
{
  "id": "664a1f2e3b4c5d6e7f8a9b0c",
  "titulo": "Interestelar",
  "diretor": "Christopher Nolan",
  "sinopse": "Uma equipe de exploradores viaja através de um buraco de minhoca no espaço.",
  "ano": 2014,
  "genero": "Ficção Científica",
  "lancado": true
}
```

---

### Criar uma Resenha

**Request:**
```http
POST /api/resenhas
Content-Type: application/json

{
  "filmeId": "664a1f2e3b4c5d6e7f8a9b0c",
  "autorNome": "Maria Silva",
  "nota": 5,
  "texto": "Obra-prima do cinema moderno. Visualmente deslumbrante e emocionalmente devastador."
}
```

**Response `201 Created`:**
```json
{
  "id": "664b2a3f4c5d6e7f8a9b0d1e",
  "filmeId": "664a1f2e3b4c5d6e7f8a9b0c",
  "autorNome": "Maria Silva",
  "nota": 5,
  "texto": "Obra-prima do cinema moderno. Visualmente deslumbrante e emocionalmente devastador.",
  "dataCriacao": "2026-05-15T14:30:00Z"
}
```

---

### Buscar Resenhas de um Filme

**Request:**
```http
GET /api/resenhas/filme/664a1f2e3b4c5d6e7f8a9b0c
```

**Response `200 OK`:**
```json
[
  {
    "id": "664b2a3f4c5d6e7f8a9b0d1e",
    "filmeId": "664a1f2e3b4c5d6e7f8a9b0c",
    "autorNome": "Maria Silva",
    "nota": 5,
    "texto": "Obra-prima do cinema moderno.",
    "dataCriacao": "2026-05-15T14:30:00Z"
  }
]
```

---

## 🌱 Variáveis de Ambiente

| Variável                    | Descrição                          | Exemplo                              |
|-----------------------------|------------------------------------|--------------------------------------|
| `MongoDb__ConnectionString` | String de conexão com o MongoDB    | `mongodb://localhost:27017`          |
| `MongoDb__DatabaseName`     | Nome do banco de dados             | `filmesresenha`                      |

> ⚠️ **Nunca commite valores reais** de conexão no repositório. Use variáveis de ambiente ou um arquivo `.env` listado no `.gitignore`.

---

## 🐳 MongoDB com Docker (opcional)

Para subir o MongoDB localmente via Docker Compose, crie um `docker-compose.yml`:

```yaml
version: '3.8'
services:
  mongodb:
    image: mongo:7
    container_name: mongo_filmesresenha
    ports:
      - "27017:27017"
    volumes:
      - mongo_data:/data/db

volumes:
  mongo_data:
```

Execute:
```bash
docker-compose up -d
```

---

## 🧱 Modelos de Dados

### Filme

| Campo     | Tipo    | Validação                        |
|-----------|---------|----------------------------------|
| `id`      | string  | Gerado automaticamente (ObjectId)|
| `titulo`  | string  | Obrigatório, máx. 300 chars      |
| `diretor` | string  | Obrigatório, máx. 100 chars      |
| `sinopse` | string  | Obrigatório, máx. 1000 chars     |
| `ano`     | int     | Entre 1888 e 2030                |
| `genero`  | string  | Obrigatório, máx. 100 chars      |
| `lancado` | bool    | Padrão: `true`                   |

### Resenha

| Campo        | Tipo     | Validação                           |
|--------------|----------|-------------------------------------|
| `id`         | string   | Gerado automaticamente (ObjectId)   |
| `filmeId`    | string   | Obrigatório — deve existir no banco |
| `autorNome`  | string   | Obrigatório, máx. 100 chars         |
| `nota`       | int      | Entre 1 e 5                         |
| `texto`      | string   | Obrigatório, mín. 10, máx. 2000 chars|
| `dataCriacao`| DateTime | Gerado automaticamente (UTC)        |

---

## 🧰 Stack Tecnológica

| Camada    | Tecnologia                   |
|-----------|------------------------------|
| Backend   | .NET 10 (C#) — Minimal API  |
| Banco     | MongoDB                      |
| Driver    | MongoDB.Driver (NuGet)       |
| Docs      | Swagger / OpenAPI (Swashbuckle) |
| Frontend  | HTML + JavaScript (fetch)    |

---

## 📐 Princípios SOLID aplicados

Consulte o arquivo [`SOLID.md`](./SOLID.md) para a descrição detalhada de cada princípio aplicado no projeto.

Resumo:

| Princípio | Onde se aplica |
|-----------|---------------|
| **S** — Single Responsibility | Cada classe tem uma única responsabilidade: `FilmeService` cuida das regras de negócio, `FilmeRepository` cuida do acesso ao banco |
| **I** — Interface Segregation | Interfaces separadas por entidade: `IFilmeRepository`, `IResenhaRepository`, `IFilmeService`, `IResenhaService` |
| **D** — Dependency Inversion | Services e Repositories são injetados via `IServiceCollection` — as classes dependem de abstrações, não de implementações concretas |

---

## 🗂️ Estrutura do Projeto

```
AtividadeSemestral/
├── Models/
│   ├── Filme.cs
│   └── Resenha.cs
├── Repositories/
│   ├── IFilmeRepository.cs
│   ├── FilmeRepository.cs
│   ├── IResenhaRepository.cs
│   └── ResenhaRepository.cs
├── Services/
│   ├── IFilmeService.cs
│   ├── FilmeService.cs
│   ├── IResenhaService.cs
│   └── ResenhaService.cs
├── Program.cs
├── AtividadeSemestral.csproj
├── README.md
└── SOLID.md
```

---

## 👤 Autor

Desenvolvido como Trabalho Prático Semestral da disciplina **Arquitetura de Aplicações Web — 2026.1**.
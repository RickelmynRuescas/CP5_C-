# 🏖️ PraiaStore API

API RESTful desenvolvida em **C# .NET 10** com **Entity Framework Core** para a gestão de uma loja de moda praia.

> Checkpoint 5 — C# Software Development — FIAP

---

## 👥 Integrantes

| Nome | RM |
|------|----|
| Rickelmyn de Souza Ruescas| RM556055 |
| Heloísa Fleury Jardim | RM556378 |
| Juan Fuentes Rufino | RM557673 |
| Paulo Henrique Monteiro Golovanevsky | RM555300 |
| Pedro Henrique Silva Batista  | RM5558137 |

---

## 📌 Contexto do projeto

**O que é:** uma API para o back-office de uma loja de moda praia (biquínis, maiôs, saídas de praia e acessórios).

**Problema que resolve:** pequenas lojas de moda praia costumam controlar catálogo, estoque por tamanho/cor e cadastro de clientes em planilhas soltas, o que gera divergência de estoque, produtos duplicados e dificuldade para integrar com um e-commerce ou app. A PraiaStore API centraliza esses dados em um único serviço, com regras de negócio e validações.

**Para quem é destinado:** lojistas e equipes de loja de moda praia, além de desenvolvedores que precisam integrar um site, app ou sistema de PDV ao catálogo da loja.

**Principais regras de negócio:**
- Todo produto pertence a uma categoria existente.
- Uma categoria com produtos vinculados **não pode ser removida** (retorna `409`).
- Nome de categoria e e-mail de cliente são **únicos** (retorna `409` em caso de duplicidade).
- Tamanhos aceitos: `PP`, `P`, `M`, `G`, `GG` ou `U` (único).
- Preço deve ser maior que zero e estoque não pode ser negativo.

---

## 🛠️ Tecnologias

- .NET 10 / ASP.NET Core Web API (Controllers)
- Entity Framework Core 10 + **SQLite**
- Asp.Versioning (versionamento por URL: `/api/v1/...`)
- OpenAPI + Swagger UI
- ProblemDetails (RFC 9457) para respostas de erro padronizadas

## 🗄️ Banco de dados

**SQLite** — escolhido por não exigir instalação de servidor: o arquivo `praiastore.db` é criado automaticamente na primeira execução.

- String de conexão: `appsettings.json` → `ConnectionStrings:DefaultConnection`
- Ao iniciar, a API executa `Database.Migrate()` (aplica as migrations pendentes) e insere alguns dados de exemplo.

---

## 🧱 Estrutura do projeto

```
├── Controllers/V1/          # Endpoints versionados (v1)
├── Data/
│   ├── AppDbContext.cs      # DbContext do EF Core
│   ├── Configurations/      # Mapeamento das entidades (Fluent API)
│   ├── Migrations/          # Migrations do EF Core
│   └── DbSeeder.cs          # Dados iniciais para testes
├── Dtos/                    # Objetos de entrada (Request) e saída (Response)
├── Exceptions/              # Exceções de negócio (404, 400, 409)
├── Handlers/                # GlobalExceptionHandler (tratamento de erros)
├── Mappings/                # Projeções entidade → DTO
├── Models/                  # Entidades: Categoria, Produto, Cliente
├── Services/                # Regras de negócio
├── docs/evidencias/         # Prints dos testes
└── Program.cs               # Configuração da aplicação
```

**Modelo de dados:** `Categoria (1) ──── (N) Produto` · `Cliente`

---

## 🔄 Migrations

A migration inicial está em `Data/Migrations/20260929220000_InitialCreate.cs` e cria:

| Tabela | Colunas principais | Restrições |
|--------|--------------------|------------|
| `Categorias` | Id, Nome, Descricao | PK, índice único em `Nome` |
| `Produtos` | Id, Nome, Descricao, Preco, Tamanho, Cor, Estoque, Ativo, DataCadastro, CategoriaId | PK, FK → `Categorias` (Restrict), índice em `CategoriaId` |
| `Clientes` | Id, Nome, Email, Telefone, DataCadastro | PK, índice único em `Email` |

Comandos utilizados (a ferramenta `dotnet-ef` está declarada em `.config/dotnet-tools.json`):

```bash
dotnet tool restore
dotnet ef migrations add InitialCreate --output-dir Data/Migrations
dotnet ef database update
```

> Não é necessário rodar o `database update` manualmente: a API aplica as migrations ao iniciar.

---

## ▶️ Como rodar localmente

**Pré-requisito:** [.NET 10 SDK](https://dotnet.microsoft.com/download) (`dotnet --version` deve mostrar `10.x`).

```bash
git clone <url-do-repositorio>
cd <pasta-do-repositorio>
dotnet run
```

Depois acesse **http://localhost:5000/swagger** para testar os endpoints pelo Swagger UI.

O documento OpenAPI fica em `http://localhost:5000/openapi/v1.json`.

---

## 🔗 Endpoints (v1)

Base URL: `http://localhost:5000/api/v1`

### Categorias

| Método | Rota | Descrição | Status |
|--------|------|-----------|--------|
| GET | `/api/v1/categorias` | Lista todas as categorias | 200 |
| GET | `/api/v1/categorias/{id}` | Busca uma categoria pelo id | 200, 404 |
| GET | `/api/v1/categorias/{id}/produtos` | Lista os produtos de uma categoria | 200, 404 |
| POST | `/api/v1/categorias` | Cadastra uma categoria | 201, 400, 409 |
| PUT | `/api/v1/categorias/{id}` | Atualiza uma categoria | 204, 400, 404, 409 |
| DELETE | `/api/v1/categorias/{id}` | Remove uma categoria sem produtos | 204, 404, 409 |

### Produtos

| Método | Rota | Descrição | Status |
|--------|------|-----------|--------|
| GET | `/api/v1/produtos` | Lista produtos. Filtros opcionais: `?categoriaId=1&nome=biquini&ativo=true` | 200 |
| GET | `/api/v1/produtos/{id}` | Busca um produto pelo id | 200, 404 |
| POST | `/api/v1/produtos` | Cadastra um produto | 201, 400 |
| PUT | `/api/v1/produtos/{id}` | Atualiza um produto | 204, 400, 404 |
| DELETE | `/api/v1/produtos/{id}` | Remove um produto | 204, 404 |

### Clientes

| Método | Rota | Descrição | Status |
|--------|------|-----------|--------|
| GET | `/api/v1/clientes` | Lista todos os clientes | 200 |
| GET | `/api/v1/clientes/{id}` | Busca um cliente pelo id | 200, 404 |
| POST | `/api/v1/clientes` | Cadastra um cliente | 201, 400, 409 |
| PUT | `/api/v1/clientes/{id}` | Atualiza um cliente | 204, 400, 404, 409 |
| DELETE | `/api/v1/clientes/{id}` | Remove um cliente | 204, 404 |

### Exemplos de corpo (JSON)

**POST/PUT `/api/v1/categorias`**
```json
{ "nome": "Acessórios", "descricao": "Chapéus, bolsas e óculos" }
```

**POST/PUT `/api/v1/produtos`**
```json
{
  "nome": "Biquíni Asa Delta Azul",
  "descricao": "Calcinha asa delta com top triângulo",
  "preco": 159.90,
  "tamanho": "M",
  "cor": "Azul",
  "estoque": 15,
  "ativo": true,
  "categoriaId": 1
}
```

**POST/PUT `/api/v1/clientes`**
```json
{ "nome": "Carla Mendes", "email": "carla.mendes@email.com", "telefone": "11966665555" }
```

### Formato de erro (ProblemDetails)

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.5",
  "title": "Recurso não encontrado",
  "status": 404,
  "detail": "Produto com id 999 não encontrado.",
  "instance": "/api/v1/produtos/999"
}
```

---

## ✅ Evidências de teste

Testes realizados pelo Swagger UI.

**Visão geral dos endpoints versionados (`/api/v1`):**

![](docs/evidencias/00-swagger-endpoints.png)

### Categorias
| Teste | Print |
|-------|-------|
| GET lista (200) | ![](docs/evidencias/01-get-categorias.png) |
| GET por id (200) | ![](docs/evidencias/02-get-categoria-id.png) |
| GET por id inexistente (404) | ![](docs/evidencias/03-get-categoria-404.png) |
| GET produtos da categoria (200) | ![](docs/evidencias/04-get-categoria-produtos.png) |
| POST (201) | ![](docs/evidencias/05-post-categoria.png) |
| POST inválido (400) | ![](docs/evidencias/06-post-categoria-400.png) |
| PUT (204) | ![](docs/evidencias/07-put-categoria.png) |
| PUT com nome já existente (409) | ![](docs/evidencias/07b-put-categoria-409.png) |
| DELETE (204) | ![](docs/evidencias/08-delete-categoria.png) |
| DELETE com produtos (409) | ![](docs/evidencias/09-delete-categoria-409.png) |

### Produtos
| Teste | Print |
|-------|-------|
| GET lista (200) | ![](docs/evidencias/10-get-produtos.png) |
| GET por id (200) | ![](docs/evidencias/11-get-produto-id.png) |
| POST (201) | ![](docs/evidencias/12-post-produto.png) |
| POST inválido (400) | ![](docs/evidencias/13-post-produto-400.png) |
| PUT (204) | ![](docs/evidencias/14-put-produto.png) |
| DELETE (204) | ![](docs/evidencias/15-delete-produto.png) |
| GET após DELETE (404) | ![](docs/evidencias/16-get-produto-404.png) |

### Clientes
| Teste | Print |
|-------|-------|
| GET lista (200) | ![](docs/evidencias/17-get-clientes.png) |
| GET por id (200) | ![](docs/evidencias/18-get-cliente-id.png) |
| POST (201) | ![](docs/evidencias/19-post-cliente.png) |
| POST com e-mail inválido (400) | ![](docs/evidencias/19b-post-cliente-400.png) |
| POST e-mail duplicado (409) | ![](docs/evidencias/20-post-cliente-409.png) |
| PUT (204) | ![](docs/evidencias/21-put-cliente.png) |
| PUT com e-mail inválido (400) | ![](docs/evidencias/21b-put-cliente-400.png) |
| DELETE (204) | ![](docs/evidencias/22-delete-cliente.png) |

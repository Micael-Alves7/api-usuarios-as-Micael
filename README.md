# API de Gerenciamento de Usuários

## Descrição
Esta API foi desenvolvida para a disciplina de Padrões de Projeto, com o objetivo de criar um sistema completo para gerenciamento de usuários utilizando ASP.NET Core com Minimal APIs.

Ela permite realizar operações CRUD (criação, consulta, atualização e remoção) de usuários, seguindo boas práticas de desenvolvimento. A arquitetura foi organizada usando princípios de Clean Architecture, garantindo separação clara entre as camadas e facilitando manutenção e evolução do projeto.

O sistema utiliza validações com FluentValidation, persistência de dados com Entity Framework Core e SQLite, além de recursos como soft delete e normalização de emails para garantir consistência e integridade das informações.

## Tecnologias Utilizadas

- .NET 9.0
- ASP.NET Core com Minimal APIs
- Entity Framework Core 9.0
- SQLite
- FluentValidation 11.3
- C# 12.0

## Padrões de Projeto Implementados

- **Repository Pattern**: Abstração da camada de persistência de dados
- **Service Pattern**: Orquestração e centralização das regras de negócio
- **DTO Pattern**: Desacoplamento entre entidades e contratos externos
- **Dependency Injection**: Injeção de dependências nativa do ASP.NET Core

## Como Executar o Projeto

### Pré-requisitos

- .NET SDK 9.0 ou superior

### Passos

1. Clone o repositório
```bash
git clone [url-do-repositorio]
cd AS
```

2. Restaure as dependências
```bash
dotnet restore
```

3. Execute a aplicação
```bash
dotnet run
```

4. Acesse a API em `http://localhost:5276` (a porta pode variar)

### Exemplos de Requisições

#### Criar Usuário (POST /usuarios)
```json
{
    "nome": "João Silva",
    "email": "joao@email.com",
    "senha": "123456",
    "dataNascimento": "2000-05-15",
    "telefone": "(11) 99999-9999"
}
```

#### Atualizar Usuário (PUT /usuarios/{id})
```json
{
    "nome": "João Silva Atualizado",
    "email": "joao@email.com",
    "dataNascimento": "2000-05-15",
    "telefone": "(11) 88888-8888",
    "ativo": true
}
```

## Estrutura do Projeto

```
AS/
├── Domain/
│   └── Entities/
│       └── Usuario.cs              # Entidade de domínio
│
├── Application/
│   ├── DTOs/
│   │   ├── UsuarioCreateDto.cs     # DTO para criação
│   │   ├── UsuarioReadDto.cs       # DTO para leitura
│   │   └── UsuarioUpdateDto.cs     # DTO para atualização
│   │
│   ├── Interfaces/
│   │   ├── IUsuarioRepository.cs   # Interface do repositório
│   │   └── IUsuarioService.cs      # Interface do serviço
│   │
│   ├── Services/
│   │   └── UsuarioService.cs       # Implementação do serviço
│   │
│   └── Validators/
│       ├── UsuarioCreateDtoValidator.cs  # Validador de criação
│       └── UsuarioUpdateDtoValidator.cs  # Validador de atualização
│
├── Infrastructure/
│   ├── Persistence/
│   │   └── AppDbContext.cs         # Contexto do EF Core
│   │
│   └── Repositories/
│       └── UsuarioRepository.cs    # Implementação do repositório
│
├── Program.cs                      # Configuração e endpoints
├── appsettings.json               # Configurações da aplicação
└── AS.csproj                      # Arquivo de projeto
```

## Endpoints da API

| Método | Endpoint | Descrição | Status Code |
|--------|----------|-----------|-------------|
| GET | /usuarios | Lista todos os usuários | 200 OK |
| GET | /usuarios/{id} | Busca usuário por ID | 200 OK |
| POST | /usuarios | Cria novo usuário | 201 Created |
| PUT | /usuarios/{id} | Atualiza usuário | 200 OK |
| DELETE | /usuarios/{id} | Remove usuário (soft delete) | 204 No Content |

## Validações Implementadas

- **Nome**: Obrigatório, entre 3 e 100 caracteres
- **Email**: Obrigatório, formato válido, único no sistema
- **Senha**: Obrigatória, mínimo 6 caracteres
- **Data de Nascimento**: Obrigatória, idade mínima 18 anos
- **Telefone**: Opcional, máximo 15 caracteres
- **Soft Delete**: Ao deletar, marca Ativo = false
- **Normalização**: Email armazenado em lowercase

## Autor

Nome: [Micael Alves Evaldt]
Curso: [Análise e Desenvolvimento de Sistemas]

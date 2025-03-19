# Developer Evaluation Project - Sale API

## Descrição do Projeto

Este projeto implementa uma API REST completa para gerenciamento de vendas seguindo os princípios de Domain-Driven Design (DDD). A aplicação fornece um CRUD completo para registros de vendas, incluindo informações como número da venda, data, cliente, valor total, filial, produtos, quantidades, preços unitários, descontos e status da venda.

### Funcionalidades Principais

- Criação, leitura, atualização e exclusão de vendas
- Aplicação automática de descontos baseados na quantidade de itens
- Cancelamento de vendas completas ou itens específicos
- Publicação de eventos de negócio (SaleCreated, SaleModified, SaleCancelled, ItemCancelled)
- Validação de regras de negócio
- Paginação, filtragem e ordenação nos endpoints de consulta

### Regras de Negócio

- Compras acima de 4 itens idênticos têm 10% de desconto
- Compras entre 10 e 20 itens idênticos têm 20% de desconto
- Não é possível vender acima de 20 itens idênticos
- Compras abaixo de 4 itens não podem ter desconto

## Tecnologias Utilizadas

### Backend
- .NET 8.0
- C#
- Entity Framework Core
- MediatR
- AutoMapper
- Rebus (para publicação de eventos)

### Banco de Dados
- PostgreSQL (dados relacionais)
- MongoDB (logs e eventos)

### Testes
- xUnit
- NSubstitute
- Bogus (Faker)

## Estrutura do Projeto

O projeto segue uma arquitetura limpa (Clean Architecture) com separação clara entre as camadas:

```
src/
├── Ambev.DeveloperEvaluation.ORM/
├── Ambev.DeveloperEvaluation.MongoDB/
├── Ambev.DeveloperEvaluation.WebApi/
├── Ambev.DeveloperEvaluation.Application/ 
├── Ambev.DeveloperEvaluation.Domain/     
├── Ambev.DeveloperEvaluation.IoC/
└── Ambev.DeveloperEvaluation.Common/

tests/
├── Ambev.DeveloperEvaluation.Unit/ 
├── Ambev.DeveloperEvaluation.Integration/ 
└── Ambev.DeveloperEvaluation.Functional/ 
```

## Como Executar o Projeto

### Pré-requisitos
- .NET SDK 8.0
- PostgreSQL
- MongoDB
- Docker (opcional)

### Configuração Local

1. Clone o repositório:
```
git clone https://github.com/seu-usuario/ambev-developer-evaluation.git
cd ambev-developer-evaluation
```

2. Configure as strings de conexão no arquivo `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "PostgreSQL": "Host=localhost;Database=ambev_sales;Username=postgres;Password=your_password",
    "MongoDB": "mongodb://localhost:27017/ambev_events"
  }
}
```

3. Execute as migrações do banco de dados:
```
cd src/Ambev.DeveloperEvaluation.API
dotnet ef database update
```

4. Inicie a aplicação:
```
dotnet run
```

A API estará disponível em `https://localhost:5001/swagger`

### Utilizando Docker

Alternativamente, você pode usar Docker para executar o projeto:

```
docker-compose up -d
```

## Como Executar os Testes

Para executar todos os testes:
```
dotnet test
```

Para executar um tipo específico de testes:
```
dotnet test tests/Ambev.DeveloperEvaluation.Unit
dotnet test tests/Ambev.DeveloperEvaluation.Integration
dotnet test tests/Ambev.DeveloperEvaluation.Functional
```

## Endpoints da API

### Vendas

- `GET /api/sales` - Obtém lista de vendas com paginação, filtro e ordenação
- `GET /api/sales/{id}` - Obtém detalhes de uma venda específica
- `POST /api/sales` - Cria uma nova venda
- `PUT /api/sales/{id}` - Atualiza uma venda existente
- `DELETE /api/sales/{id}` - Cancela uma venda
- `DELETE /api/sales/{saleId}/items/{itemId}` - Cancela um item específico da venda

## Padrões de Desenvolvimento

Este projeto segue padrões e práticas como:

- DDD (Domain-Driven Design)
- CQRS (Command Query Responsibility Segregation)
- Repository Pattern
- Mediator Pattern
- Unit of Work
- Clean Architecture
- GitFlow para controle de versão
- Commits semânticos

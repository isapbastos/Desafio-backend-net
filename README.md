# 📘 InvestimentosJwtApi

API para simulação de investimentos com autenticação JWT, gestão de produtos e telemetria.

Tecnologias:

- .NET 8  
- SQLite  
- Docker & Docker Compose  
- Swagger para documentação da API  

---

## 🐳 Pré-requisitos

- [Docker Desktop](https://www.docker.com/products/docker-desktop)  
- [.NET SDK 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) (apenas para desenvolvimento local)  

> Docker Compose já vem integrado no Docker Desktop.
No terminal dentro da pasta do projeto, execute: 
docker-compose up --build

Para acessar o swagger, abra o seu navegador e digite:
http://localhost:8080/swagger/index.html
---

## ⚙️ Estrutura do projeto
/InvestimentosJwtApi.sln
/docker-compose.yml
/InvestimentosJwtApi/ <-- Projeto Web API
/InvestimentosJwt.Application/
/InvestimentosJwt.Domain/
/InvestimentosJwt.Infra.Data/

O **SQLite** será armazenado no volume Docker persistente `/app/Data/app.db`.

---

## 🔐 Autenticação JWT

- O token é gerado no login (`POST /api/Usuario/login`) usando email e senha.  
- Todos os endpoints com `[Authorize]` exigem que o token seja enviado no **header**:


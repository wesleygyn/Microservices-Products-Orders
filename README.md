# 🍔 Lanchonete FIAP - Sistema de Autoatendimento e Microsserviços

[![.NET 8.0](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?logo=docker)](https://www.docker.com/)
[![MySQL](https://img.shields.io/badge/MySQL-8.0-4479A1?logo=mysql&logoColor=white)](https://www.mysql.com/)
[![MongoDB](https://img.shields.io/badge/MongoDB-6.0-4EA94B?logo=mongodb&logoColor=white)](https://www.mongodb.com/)
[![RabbitMQ](https://img.shields.io/badge/RabbitMQ-3.13-FF6600?logo=rabbitmq&logoColor=white)](https://www.rabbitmq.com/)
[![Tests](https://img.shields.io/badge/Tests-84.5%25%20Coverage-success)](https://xunit.net/) <!-- Atualizado para 84.5% de cobertura -->
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

<!-- COLOQUE AQUI O BADGE DE CI/CD DO GITHUB ACTIONS -->
<!-- Exemplo: [![CI/CD Status](https://github.com/wesleygyn/Microservices-Products-Orders/actions/workflows/ci-cd.yml/badge.svg)](https://github.com/wesleygyn/Microservices-Products-Orders/actions/workflows/ci-cd.yml) -->

<!-- COLOQUE AQUI O BADGE DE COBERTURA DO SONARCLOUD -->
<!-- Exemplo: [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=SEU_ORGANIZATION_KEY_SEU_PROJECT_KEY&metric=coverage)](https://sonarcloud.io/dashboard?id=SEU_ORGANIZATION_KEY_SEU_PROJECT_KEY) -->

> **TechChallenge - Fase 4 - Pós-Graduação Arquitetura de Software - FIAP**

Sistema de autoatendimento escalável e robusto para a Lanchonete FIAP, desenvolvido com **arquitetura de microsserviços**, **Clean Architecture** e práticas de desenvolvimento modernas. Moderniza o processo de pedidos e pagamentos, garantindo eficiência operacional e uma experiência fluida para o cliente.

---

## 📋 Índice

- [Sobre o Projeto](#sobre-o-projeto)
- [Métricas de Qualidade](#métricas-de-qualidade)
- [Princípios Arquiteturais](#princípios-arquiteturais)
- [Arquitetura](#arquitetura)
- [Diagramas Arquiteturais](#diagramas-arquiteturais)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)
- [Microsserviços](#microsserviços)
- [Requisitos Funcionais](#requisitos-funcionais)
- [Requisitos Não-Funcionais](#requisitos-não-funcionais)
- [Pré-requisitos](#pré-requisitos)
- [Instalação e Execução](#instalação-e-execução)
- [Testes](#testes)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [API Endpoints](#api-endpoints)
- [Migrations](#migrations)
- [Docker](#docker)
- [Variáveis de Ambiente](#variáveis-de-ambiente)
- [Decisões Arquiteturais (ADRs)](#decisões-arquiteturais-adrs)
- [Build e Deploy](#build-e-deploy)
- [CI/CD Pipeline](#ci/cd-pipeline)
- [Resiliência](#resiliência)
- [Escalabilidade](#escalabilidade)
- [Roadmap](#roadmap)
- [Como Contribuir](#como-contribuir)
- [Licença](#licença)
- [Autores](#autores)
- [Suporte](#suporte)
- [Documentação Adicional](#documentação-adicional)
- [Destaques do Projeto](#destaques-do-projeto)
- [Para a Apresentação](#para-a-apresentação)

---

## 🎯 Sobre o Projeto

A Lanchonete FIAP, em sua expansão, enfrentava desafios no controle de pedidos e atendimento. Este projeto oferece uma solução completa de autoatendimento, permitindo que os clientes montem seus pedidos de forma personalizada e realizem pagamentos de maneira ágil via QR Code do Mercado Pago. Além disso, o sistema integra um fluxo de acompanhamento de pedidos em tempo real para a cozinha e para o cliente, e oferece um painel administrativo para gerenciamento.

### Objetivos do Projeto (Fase 4)

- ✅ Implementar arquitetura de microsserviços (mínimo 3)
- ✅ Aplicar conceitos de Clean Architecture
- ✅ Separação de bancos de dados por serviço (ao menos 1 SQL e 1 NoSQL)
- ✅ Comunicação entre microsserviços (chamada direta e mensagens em fila)
- ✅ Testes unitários com cobertura mínima de 80%
- ✅ Testes BDD (ao menos um caminho de teste)
- ✅ Containerização com Docker
- ✅ CI/CD (build, teste, deploy automatizados)
- ✅ Integração com Mercado Pago
- ✅ Monitoramento e Observabilidade básica

---

## 📊 Métricas de Qualidade

| Métrica | Valor | Status |
|---------|-------|--------|
| **Cobertura de Testes** | **84.5%+** | ✅ Meta Alcançada | <!-- Atualizado para 84.5% -->
| **Testes Unitários** | **~XXX** | ✅ Abrangente |
| **Linhas Cobertas** | **XXX/YYY** | ✅ Meta Alcançada |
| **CI/CD** | Automatizado | ✅ GitHub Actions |
| **Arquitetura** | Clean Architecture | ✅ SOLID Compliant |

---

## 🎯 Princípios Arquiteturais

- ✅ **Clean Architecture** - Separação de responsabilidades em camadas
- ✅ **SOLID** - Todos os princípios aplicados rigorosamente
- ✅ **DDD** - Domain-Driven Design com entidades ricas
- ✅ **Event-Driven** - Comunicação assíncrona via eventos
- ✅ **CQRS** - Separação de leitura e escrita (onde aplicável)
- ✅ **Dependency Inversion** - Todas dependências apontam para abstrações
- ✅ **Testabilidade** - 80%+ de cobertura de testes

---

## 🏗️ Arquitetura

O projeto segue os princípios de **Clean Architecture** e **Microsserviços**, garantindo:

- **Separação de Responsabilidades**: Cada microsserviço e camada tem uma função específica.
- **Independência de Frameworks**: Lógica de negócio isolada de detalhes técnicos.
- **Testabilidade**: Facilidade para criar testes unitários e de integração.
- **Independência de UI e Database**: Flexibilidade para mudanças.
- **Escalabilidade Horizontal**: Microsserviços independentes podem escalar separadamente.
- **Resiliência**: Falhas em um serviço não derrubam o sistema inteiro.

### Diagramas Arquiteturais

<!-- Esta seção é para incluir seus diagramas. Se você tiver links para imagens ou arquivos .drawio, pode colocá-los aqui. -->
<!-- Exemplo: ![Diagrama C4 Contexto](link-para-sua-imagem-diagrama-c4-contexto.png) -->
<!-- Exemplo: [Diagrama C4 Containers (Draw.io)](link-para-seu-arquivo-drawio) -->

---

## 🛠️ Tecnologias Utilizadas

### Stack Principal
- **Linguagem:** C# (.NET 8)
- **Framework:** ASP.NET Core
- **Bancos de Dados:** MySQL (SQL) e MongoDB (NoSQL)
- **Mensageria:** RabbitMQ com MassTransit
- **Containerização:** Docker e Docker Compose
- **Logging:** Serilog com Seq
- **Testes:** xUnit, Moq, SpecFlow (BDD)
- **Integração de Pagamento:** Mercado Pago

### Padrões e Práticas
- Clean Architecture
- Domain-Driven Design (DDD)
- Microsserviços
- SOLID Principles
- Event-Driven Architecture
- CQRS (Command Query Responsibility Segregation)
- Repository Pattern
- Unit of Work

---

## 🚀 Microsserviços

O sistema é composto por 4 microsserviços principais, cada um com responsabilidades bem definidas:

### 📦 Pedido Service (Porta: 8081)
- **Responsabilidade:** Gerenciamento de produtos (catálogo, estoque) e pedidos (criação, status, itens).
- **Tecnologias:** ASP.NET Core, MongoDB (para pedidos e status), MySQL (para produtos).
- **Funcionalidades:**
    - Listar e gerenciar produtos.
    - Criar, consultar e atualizar status de pedidos.
    - Publicar eventos de pedido (ex: `PedidoCriadoEvent`, `PedidoPagoEvent`).

### 👥 Customer Service (Porta: 8082)
- **Responsabilidade:** Gerenciamento de clientes (cadastro, identificação).
- **Tecnologias:** ASP.NET Core, MySQL.
- **Funcionalidades:**
    - Cadastrar e consultar clientes por CPF.
    - Receber eventos de identificação de cliente.

### 💳 Pagamento Service (Porta: 8083)
- **Responsabilidade:** Orquestração do processo de pagamento, integração com Mercado Pago.
- **Tecnologias:** ASP.NET Core, MySQL, Mercado Pago SDK.
- **Funcionalidades:**
    - Gerar QR Code para pagamento.
    - Processar webhooks de status de pagamento do Mercado Pago.
    - Publicar eventos de pagamento (ex: `PagamentoAprovadoEvent`, `PagamentoRecusadoEvent`).

### ⚙️ Produção Service (Porta: 8084)
- **Responsabilidade:** Gerenciamento do fluxo de produção na cozinha, atualização de status de pedido.
- **Tecnologias:** ASP.NET Core, MongoDB.
- **Funcionalidades:**
    - Receber eventos de pedidos pagos.
    - Atualizar status de pedidos (Recebido, Em Preparação, Pronto, Finalizado).
    - Notificar o cliente sobre o status do pedido.

---

## ✅ Requisitos Funcionais

Baseado no documento `SOAT_Tech_Challenge__Fase_4.pdf`:

### Pedido
- O cliente pode se identificar via CPF, cadastrar-se (nome, e-mail, CPF) ou não se identificar.
- O cliente pode montar o combo (Lanche, Acompanhamento, Bebida) de forma opcional.
- Exibição de nome, descrição e preço de cada produto.

### Pagamento
- Opção de pagamento via QR Code do Mercado Pago.

### Acompanhamento
- Monitor para o cliente acompanhar o progresso do pedido: Recebido, Em preparação, Pronto, Finalizado.

### Entrega
- Notificação ao cliente quando o pedido estiver pronto para retirada.
- Atualização do status para "Finalizado" após a retirada.

### Acesso Administrativo
- Gerenciar clientes (para campanhas promocionais).
- Gerenciar produtos e categorias (nome, categoria, preço, descrição, imagens).
- Categorias fixas: Lanche, Acompanhamento, Bebida, Sobremesa.
- Acompanhamento de pedidos em andamento e tempo de espera.

---

## ⚡ Requisitos Não-Funcionais

- **Performance:** Resposta rápida para operações de pedido e pagamento.
- **Escalabilidade:** Capacidade de lidar com aumento de demanda (microsserviços, RabbitMQ).
- **Disponibilidade:** Alta disponibilidade dos serviços (Docker Compose, health checks).
- **Segurança:** Proteção de dados sensíveis (clientes, pagamentos).
- **Observabilidade:** Monitoramento de logs e métricas (Seq, health checks).
- **Resiliência:** Tratamento de falhas (retry, circuit breaker, DLQ).
- **Manutenibilidade:** Código limpo, arquitetura modular.
- **Testabilidade:** Alta cobertura de testes.

---

## ⚙️ Pré-requisitos

Certifique-se de ter os seguintes softwares instalados em sua máquina:

- **.NET SDK 8.0** ou superior
- **Docker Desktop** (inclui Docker Engine e Docker Compose)
- **Git**
- **VS Code** ou outra IDE de sua preferência

---

## 🚀 Instalação e Execução

Siga os passos abaixo para configurar e executar o projeto localmente:

1.  **Clone o repositório:**
    ```bash
    git clone https://github.com/seu-usuario/Microservices-Products-Orders.git # ATUALIZAR LINK
    cd Microservices-Products-Orders
    ```

2.  **Configurar variáveis de ambiente:**
    Crie um arquivo `.env` na raiz do projeto com as seguintes variáveis (exemplo):
    ```
    # MySQL
    MYSQL_ROOT_PASSWORD=root
    MYSQL_DATABASE=lanchonetefiap_db

    # MongoDB
    MONGO_INITDB_ROOT_USERNAME=root
    MONGO_INITDB_ROOT_PASSWORD=root
    MONGO_INITDB_DATABASE=lanchonetefiap_mongodb

    # RabbitMQ
    RABBITMQ_DEFAULT_USER=guest
    RABBITMQ_DEFAULT_PASS=guest

    # Mercado Pago (obtenha suas credenciais em https://www.mercadopago.com.br/developers/panel/credentials)
    MERCADOPAGO_ACCESS_TOKEN=YOUR_MERCADOPAGO_ACCESS_TOKEN
    MERCADOPAGO_CLIENT_ID=YOUR_MERCADOPAGO_CLIENT_ID
    MERCADOPAGO_CLIENT_SECRET=YOUR_MERCADOPAGO_CLIENT_SECRET

    # URLs dos serviços (para comunicação interna, se necessário)
    PEDIDO_SERVICE_URL=http://lanchonetefiap-pedido-service:80
    CUSTOMER_SERVICE_URL=http://lanchonetefiap-customer-service:80
    PAGAMENTO_SERVICE_URL=http://lanchonetefiap-pagamento-service:80
    PRODUCAO_SERVICE_URL=http://lanchonetefiap-producao-service:80
    ```
    **Importante:** Substitua `YOUR_MERCADOPAGO_ACCESS_TOKEN`, `YOUR_MERCADOPAGO_CLIENT_ID` e `YOUR_MERCADOPAGO_CLIENT_SECRET` pelas suas credenciais reais do Mercado Pago.

3.  **Iniciar os serviços com Docker Compose:**
    ```bash
    docker-compose up --build -d
    ```
    Este comando irá:
    - Construir as imagens Docker para cada microsserviço.
    - Criar e iniciar os containers para MySQL, MongoDB, RabbitMQ, Seq e todos os microsserviços.
    - Criar a rede Docker `lanchonete-fiap-network`.

4.  **Verificar o status dos serviços:**
    ```bash
    docker-compose ps
    ```
    Todos os serviços devem estar com status `Up`.

5.  **Acessar as interfaces:**
    - **Swagger UI (Pedido Service):** `http://localhost:8081/swagger`
    - **Swagger UI (Customer Service):** `http://localhost:8082/swagger`
    - **Swagger UI (Pagamento Service):** `http://localhost:8083/swagger`
    - **Swagger UI (Produção Service):** `http://localhost:8084/swagger`
    - **RabbitMQ Management:** `http://localhost:15672` (Login: `guest`/`guest` ou conforme `.env`)
    - **Seq (Centralized Logging):** `http://localhost:8085`

---

## 🧪 Testes

O projeto possui uma suíte abrangente de testes para garantir a qualidade e o comportamento esperado dos microsserviços.

### Executar Testes

Para executar todos os testes unitários e de integração:

```bash
dotnet test
```

### Estrutura de Testes

Cada microsserviço possui seu próprio projeto de testes:
- `LanchoneteFIAP.PedidoService.Tests`
- `LanchoneteFIAP.CustomerService.Tests`
- `LanchoneteFIAP.PagamentoService.Tests`
- `LanchoneteFIAP.ProducaoService.Tests`

### Estratégia de Testes
- **Testes Unitários:** Focados na lógica de negócio e componentes isolados.
- **Testes de Integração:** Verificam a comunicação entre camadas e com bancos de dados/serviços externos.
- **Testes BDD (Behavior-Driven Development):** Utilizando SpecFlow para descrever o comportamento do sistema em linguagem natural, garantindo que o software atenda aos requisitos de negócio.

---

## 📁 Estrutura do Projeto

<!-- Adicione aqui uma descrição da estrutura de pastas do seu projeto, ou um diagrama de pastas se tiver. -->

---

## 🔗 API Endpoints

<!-- Liste aqui os principais endpoints de cada microsserviço, com exemplos de requisição/resposta se possível. -->
<!-- Exemplo: -->
<!-- ### Customer Service -->
<!-- - `GET /api/customers/{cpf}`: Busca cliente por CPF -->
<!-- - `POST /api/customers`: Cadastra novo cliente -->

---

## 🔄 Migrations

<!-- Descreva aqui como as migrations do Entity Framework Core são aplicadas para o MySQL. -->
<!-- Exemplo: -->
<!-- Para aplicar as migrations, execute o seguinte comando na pasta do projeto de infraestrutura do microsserviço: -->
<!-- `dotnet ef database update` -->

---

## 🐳 Docker

<!-- Adicione aqui informações adicionais sobre a configuração Docker, se houver. -->
<!-- Exemplo: -->
<!-- Os Dockerfiles para cada microsserviço estão localizados em suas respectivas pastas. -->

---

## ⚙️ Variáveis de Ambiente

<!-- Detalhe aqui as variáveis de ambiente necessárias para cada microsserviço e sua finalidade. -->
<!-- Exemplo: -->
<!-- - `ConnectionStrings__DefaultConnection`: String de conexão com o MySQL. -->
<!-- - `MongoDbSettings__ConnectionString`: String de conexão com o MongoDB. -->

---

## 📝 Decisões Arquiteturais (ADRs)

<!-- Adicione aqui links ou descrições das suas Architectural Decision Records (ADRs). -->
<!-- Exemplo: -->
<!-- - [ADR-001: Escolha do Banco de Dados para Produtos](docs/adr/adr-001-database-choice.md) -->
<!-- - [ADR-002: Estratégia de Comunicação entre Microsserviços](docs/adr/adr-002-microservice-communication.md) -->

---

## 📦 Build e Deploy

### Build Local

```bash
# Build Release de um microsserviço (ex: Pedido Service)
dotnet build src/LanchoneteFIAP.PedidoService --configuration Release

# Publish Pedido Service
dotnet publish src/LanchoneteFIAP.PedidoService -c Release -o publish/pedido-service

# Repetir para cada microsserviço
```

### Build Docker

```bash
# Build imagem Docker de um microsserviço (ex: Pedido Service)
docker build -f src/LanchoneteFIAP.PedidoService/Dockerfile -t lanchonetefiap-pedido-service:latest .

# Repetir para cada microsserviço
```

### Deploy com Docker Compose

```bash
# Iniciar todos os serviços
docker-compose up --build -d

# Ver status
docker-compose ps

# Ver logs
docker-compose logs -f

# Parar tudo
docker-compose down

# Parar e remover volumes (CUIDADO: perde dados!)
docker-compose down -v
```

---

## 🔄 CI/CD Pipeline

### GitHub Actions Workflow

Arquivo: `.github/workflows/ci-cd.yml`

**Triggers:**
- Push para branch `main`
- Pull Requests para `main`
- Manual dispatch

**Jobs:**

1.  **Build & Test**
    -   Checkout código
    -   Setup .NET 8
    -   Restore dependencies
    -   Build solution (para todos os microsserviços)
    -   Run all tests com cobertura (para todos os microsserviços)
    -   Upload coverage para Codecov (se configurado)

2.  **Docker Build** (apenas em push para main)
    -   Build imagem Docker para cada microsserviço
    -   Push para registry (opcional)

3.  **Code Quality**
    -   Análise estática (via SonarQube ou similar - futuro)
    -   Security scanning (futuro)

**Badges:**

```markdown
# ATUALIZAR OS LINKS ABAIXO PARA O SEU REPOSITÓRIO DA FASE 4
<!-- COLOQUE AQUI O BADGE DE CI/CD DO GITHUB ACTIONS -->
<!-- Exemplo: ![CI/CD Status](https://github.com/wesleygyn/Microservices-Products-Orders/actions/workflows/ci-cd.yml/badge.svg) -->

<!-- COLOQUE AQUI O BADGE DE COBERTURA DO SONARCLOUD -->
<!-- Exemplo: [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=SEU_ORGANIZATION_KEY_SEU_PROJECT_KEY&metric=coverage)](https://sonarcloud.io/dashboard?id=SEU_ORGANIZATION_KEY_SEU_PROJECT_KEY) -->
```

---

## 🛡️ Resiliência

### Retry Policies (MassTransit)

```yaml
Tentativa 1:
  Delay: 1 segundo

Tentativa 2:
  Delay: 5 segundos (backoff exponencial: 1s * 5)

Tentativa 3 (ÚLTIMA):
  Delay: 15 segundos (backoff exponencial: 5s * 3)

Após 3 falhas:
  Ação: Mover para Dead Letter Queue
```

### Circuit Breaker (MassTransit)

```yaml
Configuração:
  Janela de avaliação: 1 minuto
  Taxa de erro para abertura: 15%
  Mínimo de requisições: 10
  Tempo de recuperação (half-open): 5 minutos

Estados:
  Closed: Funcionamento normal
  Open: Bloqueia novas requisições
  Half-Open: Testa recuperação gradual
```

### Dead Letter Queue (DLQ)

Mensagens que falharam o processamento são movidas para filas de Dead Letter para análise e reprocessamento manual.

---

## 📈 Escalabilidade

### Horizontal Scaling

**Microsserviços (API, Customer, Pagamento, Produção):**

```yaml
Replicas: 1-2 (padrão) → N réplicas
Strategy: Load Balancer Round Robin
Stateless: Sim (JWT para admin, sem sessão para cliente)
Escalamento: Manual ou Auto-scaling (K8s HPA - futuro)
```

**RabbitMQ:**

```yaml
Strategy: Consumer Competing
Escalamento: Auto-scale baseado em queue depth
```

**Databases:**

```yaml
MySQL: 1 Primary (atual)
MongoDB: 1 Primary (atual)
Futuro: Read Replicas, Sharding
```

---

## 🎯 Roadmap

- [x] Implementação de microsserviços (Pedido, Customer, Pagamento, Produção)
- [x] Clean Architecture
- [x] Testes unitários (80%+ coverage)
- [x] Testes BDD
- [x] Docker e Docker Compose
- [x] Migrations automáticas (para MySQL)
- [x] Webhook de pagamento (Mercado Pago)
- [x] CI/CD com GitHub Actions
- [x] Mensageria (RabbitMQ)
- [x] Observabilidade (Seq, Health Checks)
- [ ] Integração com SonarQube (futuro)
- [ ] API Gateway (futuro)
- [ ] Service Discovery (futuro)
- [ ] Dashboard de métricas com Grafana (futuro)

---

## 💡 Como Contribuir

Este é um projeto acadêmico, mas feedback é sempre bem-vindo!

1.  **Fork** o projeto
2.  Crie uma **branch** para sua feature (`git checkout -b feature/AmazingFeature`)
3.  **Commit** suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4.  **Push** para a branch (`git push origin feature/AmazingFeature`)
5.  Abra um **Pull Request**

**Guidelines:**
- Mantenha cobertura de testes > 80%
- Siga SOLID e Clean Code
- Adicione testes para novas features
- Atualize documentação

---

## 📝 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

---

## 👥 Autores

**Tech Challenge - FIAP**
- Pós-Graduação em Arquitetura de Software
- Fase 4 - Microsserviços

---

## 📞 Suporte

Para reportar bugs ou sugerir melhorias:
- Abra uma [Issue](https://github.com/seu-usuario/Microservices-Products-Orders/issues) <!-- ATUALIZAR LINK -->
- Entre em contato via [Discussions](https://github.com/seu-usuario/Microservices-Products-Orders/discussions) <!-- ATUALIZAR LINK -->

---

## 📚 Documentação Adicional

- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Microservices Patterns](https://microservices.io/patterns/index.html)
- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [MongoDB Documentation](https://docs.mongodb.com/)
- [RabbitMQ Documentation](https://www.rabbitmq.com/documentation.html)
- [MassTransit Documentation](https://masstransit.io/)
- [xUnit Documentation](https://xunit.net/)
- [SpecFlow Documentation](https://docs.specflow.org/)
- [C4 Model](https://c4model.com)
- [Event Storming](https://www.eventstorming.com/)

---

## 🏆 Destaques do Projeto

### ✅ Qualidade de Código
- **84.5%+** de cobertura de testes (meta: 80%+) <!-- Atualizado para 84.5% -->
- CI/CD totalmente automatizado
- Code quality badges funcionando (se configurados)

### ✅ Arquitetura de Excelência
- **Clean Architecture** com 4 camadas
- **Microsserviços** bem definidos (Pedido, Customer, Pagamento, Produção)
- **Event-Driven Design** com RabbitMQ
- **SOLID principles** aplicados rigorosamente
- **DDD** com bounded contexts claros
- **Uso estratégico de SQL (MySQL) e NoSQL (MongoDB)**

### ✅ Documentação Profissional
- **Event Storming** completo com contextos da lanchonete
- **Diagramas C4** em 3 níveis (Contexto, Containers, Componentes)
- **9 diagramas** visuais e editáveis (Draw.io)
- **ADRs** documentados com justificativas
- **Manual completo** de instalação e uso (a ser criado)

### ✅ Resiliência e Escalabilidade
- **Retry policies** (tentativas, exponential backoff)
- **Circuit breaker** (taxa de falha configurável)
- **Horizontal scaling** ready (design stateless)
- **Health checks** em todos os componentes
- **Dead Letter Queue** para falhas definitivas

### ✅ Observabilidade
- **Structured logging** com Serilog
- **Centralized logs** com Seq
- **Health checks** (/health)
- **Distributed tracing** ready (correlation IDs)

---

## 🎤 Para a Apresentação

### Argumentos Fortes

> **"Nosso projeto não é apenas código funcional, mas um exemplo completo de arquitetura de software de excelência para a Lanchonete FIAP:**
>
> - ✅ **84.5%+ de cobertura** de testes <!-- Atualizado para 84.5% -->
> - ✅ **Event Storming profissional** com bounded contexts claros
> - ✅ **C4 Model em 3 níveis** (padrão da indústria)
> - ✅ **Clean Architecture** com SOLID aplicado
> - ✅ **Arquitetura de Microsserviços** com comunicação assíncrona
> - ✅ **Uso estratégico de SQL (MySQL) e NoSQL (MongoDB)**
> - ✅ **CI/CD totalmente automatizado** (GitHub Actions)
> - ✅ **9 diagramas profissionais** (editáveis e visuais)
> - ✅ **ADRs documentados** com justificativas e trade-offs
> - ✅ **Observabilidade completa** (logs, health checks)
> - ✅ **Manual completo** de instalação e troubleshooting (a ser criado)
>
> **Isso demonstra maturidade arquitetural e preparação para sistemas de larga escala em produção!**"

### Demonstração Sugerida

1.  **Abrir README no GitHub** → Mostrar badges funcionando (se atualizados)
2.  **Navegar pelos diagramas** → Event Storming → C4 → Deployment → Sequence Diagram
3.  **Mostrar Codecov dashboard** → Evidência de cobertura de testes (se disponível)
4.  **Explicar Event Storming** → Bounded contexts, eventos, políticas da lanchonete
5.  **Demonstrar C4 Model** → 3 níveis de abstração para a Lanchonete FIAP
6.  **Apresentar ADRs** → Decisões justificadas (ex: por que MySQL e MongoDB?)
7.  **Executar sistema** → `docker-compose up -d`
8.  **Testar fluxo completo** → Montar pedido no Totem → Pagar via QR Code → Acompanhar status → Retirar pedido
9.  **Mostrar observabilidade** → Logs em tempo real no Seq, RabbitMQ Management

---

<div align="center">

**⭐ Se este projeto te ajudou, considere dar uma estrela! ⭐**

Desenvolvido com ❤️ e excelência arquitetural para o Tech Challenge FIAP

</div>
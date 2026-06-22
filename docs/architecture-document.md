# 📄 Documento de Arquitetura - Lanchonete FIAP

## 🚀 Introdução

Este documento detalha a arquitetura do sistema de autoatendimento da Lanchonete FIAP, focando nas decisões de design, padrões utilizados e a justificativa por trás das escolhas tecnológicas. Ele serve como um guia para desenvolvedores, arquitetos e stakeholders entenderem a estrutura e o funcionamento do sistema.

## 🎯 Contexto do Projeto

O sistema visa modernizar o processo de pedidos e pagamentos da Lanchonete FIAP, que está em expansão. A solução implementa um sistema de autoatendimento para clientes e um painel administrativo para gerenciamento, garantindo eficiência operacional e uma experiência fluida.

## 💡 Princípios Arquiteturais

A arquitetura do sistema é guiada pelos seguintes princípios:

-   **Clean Architecture:** Separação clara de responsabilidades em camadas (Domínio, Aplicação, Interface, Infraestrutura) para isolar a lógica de negócio de detalhes técnicos e facilitar a testabilidade.
-   **SOLID:** Aplicação rigorosa dos cinco princípios (Single Responsibility, Open/Closed, Liskov Substitution, Interface Segregation, Dependency Inversion) para promover código modular, flexível e de fácil manutenção.
-   **Domain-Driven Design (DDD):** Foco no domínio de negócio, com entidades ricas, agregados e bounded contexts bem definidos para modelar a complexidade do negócio.
-   **Event-Driven Architecture:** Comunicação assíncrona entre microsserviços através de eventos, promovendo desacoplamento e resiliência.
-   **CQRS (Command Query Responsibility Segregation):** Separação das operações de leitura (queries) e escrita (commands) em alguns contextos, otimizando o desempenho e a escalabilidade.
-   **Dependency Inversion Principle (DIP):** Dependências sempre apontam para abstrações, garantindo flexibilidade e facilitando a troca de implementações.
-   **Testabilidade:** Design focado em facilitar testes unitários, de integração e BDD, com alta cobertura de código.

## 🏗️ Visão Geral da Arquitetura (C4 Model)

O sistema adota uma arquitetura de microsserviços, com componentes containerizados e orquestrados via Docker Compose (para ambiente local/desenvolvimento).

### Nível 1: Diagrama de Contexto

O sistema da Lanchonete FIAP interage com os seguintes atores e sistemas externos:

-   **Cliente:** Usuário final que interage com o sistema de autoatendimento (Totem).
-   **Sistema Lanchonete FIAP:** O sistema principal de autoatendimento.
-   **Mercado Pago:** Gateway de pagamento externo para processamento de transações.
-   **Monitor da Cozinha:** Sistema externo (ou interface dedicada) para acompanhamento do status dos pedidos em produção.

### Nível 2: Diagrama de Containers

O sistema é composto pelos seguintes containers principais:

-   **Totem (Frontend/Web):** Interface de autoatendimento para o cliente.
-   **Microsserviços (Backend):**
    -   **Pedido Service:** Gerencia produtos e pedidos.
    -   **Customer Service:** Gerencia clientes.
    -   **Pagamento Service:** Orquestra pagamentos via Mercado Pago.
    -   **Produção Service:** Gerencia o fluxo de produção na cozinha.
-   **Bancos de Dados:**
    -   **MySQL:** Para dados estruturados (clientes, pagamentos, produtos).
    -   **MongoDB:** Para dados flexíveis (pedidos, status de produção).
-   **Infraestrutura de Mensageria:**
    -   **RabbitMQ:** Fila de mensagens para comunicação assíncrona.
-   **Ferramentas de Observabilidade:**
    -   **Seq:** Servidor para logs estruturados centralizados.

## 🚀 Detalhes dos Microsserviços

Cada microsserviço é uma aplicação ASP.NET Core 8, seguindo a Clean Architecture internamente.

### 📦 Pedido Service

-   **Responsabilidade:** Gerenciamento completo do ciclo de vida de produtos e pedidos.
-   **Tecnologias:** ASP.NET Core, MongoDB (pedidos), MySQL (produtos), MassTransit (RabbitMQ).
-   **Camadas Internas:**
    -   **Domain:** Entidades `Produto`, `Pedido`, `ItemPedido`, regras de negócio.
    -   **Application:** Casos de uso para criar pedido, atualizar status, gerenciar produtos.
    -   **Infrastructure:** Repositórios para MongoDB e MySQL, integração com RabbitMQ.
-   **Comunicação:** Publica eventos (`PedidoCriadoEvent`, `PedidoPagoEvent`) e consome eventos de pagamento.

### 👥 Customer Service

-   **Responsabilidade:** Gerenciamento de cadastro e identificação de clientes.
-   **Tecnologias:** ASP.NET Core, MySQL, MassTransit (RabbitMQ).
-   **Camadas Internas:**
    -   **Domain:** Entidade `Cliente`, regras de validação de CPF.
    -   **Application:** Casos de uso para cadastrar, consultar e identificar clientes.
    -   **Infrastructure:** Repositório para MySQL.
-   **Comunicação:** Consome eventos de identificação de cliente (se aplicável).

### 💳 Pagamento Service

-   **Responsabilidade:** Orquestração do processo de pagamento e integração com o Mercado Pago.
-   **Tecnologias:** ASP.NET Core, MySQL, Mercado Pago SDK, MassTransit (RabbitMQ).
-   **Camadas Internas:**
    -   **Domain:** Entidade `Pagamento`, regras de status de pagamento.
    -   **Application:** Casos de uso para gerar QR Code, processar webhook do Mercado Pago.
    -   **Infrastructure:** Repositório para MySQL, integração com Mercado Pago API, RabbitMQ.
-   **Comunicação:** Publica eventos (`PagamentoAprovadoEvent`, `PagamentoRecusadoEvent`).

### ⚙️ Produção Service

-   **Responsabilidade:** Gerenciamento do fluxo de produção dos pedidos na cozinha e atualização de status.
-   **Tecnologias:** ASP.NET Core, MongoDB, MassTransit (RabbitMQ).
-   **Camadas Internas:**
    -   **Domain:** Entidade `StatusProducao`, regras de transição de status.
    -   **Application:** Casos de uso para avançar status do pedido na cozinha.
    -   **Infrastructure:** Repositório para MongoDB.
-   **Comunicação:** Consome eventos de `PedidoPagoEvent` para iniciar a produção e publica eventos de atualização de status.

## 🔗 Padrões de Comunicação

O sistema utiliza uma combinação de comunicação síncrona e assíncrona:

-   **Síncrona (HTTP/REST):** Para interações diretas onde uma resposta imediata é esperada (ex: Totem -> Pedido Service para listar produtos, Pedido Service -> Customer Service para identificar cliente).
-   **Assíncrona (RabbitMQ com MassTransit):** Para comunicação entre microsserviços que não exigem resposta imediata, promovendo desacoplamento e resiliência (ex: Pagamento Service -> Produção Service após pagamento aprovado).

## 💾 Gerenciamento de Dados (Persistência Poliglota)

-   **MySQL (SQL):** Utilizado para dados que exigem alta consistência, transações ACID e relacionamentos bem definidos, como:
    -   Clientes (Customer Service)
    -   Pagamentos (Pagamento Service)
    -   Produtos (Pedido Service - catálogo)
-   **MongoDB (NoSQL):** Utilizado para dados com esquema flexível, alta taxa de escrita e leitura, e que se beneficiam de escalabilidade horizontal, como:
    -   Pedidos (Pedido Service - histórico e detalhes do pedido)
    -   Status de Produção (Produção Service - fila da cozinha)
-   **Justificativa (ADR-002):** A escolha de bancos de dados poliglota otimiza o desempenho e a adequação para cada tipo de dado, aproveitando as forças de cada tecnologia.

## 🛡️ Resiliência

A resiliência é implementada principalmente através do uso do MassTransit e padrões de design:

-   **Retry Policies:** Configuração de tentativas automáticas com backoff exponencial para operações que podem falhar temporariamente (ex: conexão com RabbitMQ, chamadas a serviços externos).
-   **Circuit Breaker:** Implementado para isolar falhas em serviços externos ou dependências. Se um serviço falha repetidamente, o circuit breaker "abre", impedindo novas chamadas por um período e evitando sobrecarga do serviço falho.
-   **Dead Letter Queue (DLQ):** Mensagens que falham após todas as tentativas de reprocessamento são movidas para uma DLQ para análise manual e reprocessamento posterior, evitando perda de dados.

## 📈 Escalabilidade

O design de microsserviços e a escolha de tecnologias favorecem a escalabilidade horizontal:

-   **Microsserviços:** Cada serviço pode ser escalado independentemente com base em sua demanda. São projetados para serem stateless (onde aplicável), facilitando a adição de novas instâncias.
-   **RabbitMQ:** Pode ser escalado adicionando mais consumidores para processar mensagens em paralelo.
-   **Bancos de Dados:**
    -   MySQL: Futuramente pode ser escalado com read replicas para leituras e sharding para escritas.
    -   MongoDB: Projetado para escalabilidade horizontal nativa (sharding).

## 📊 Monitoramento e Observabilidade

A capacidade de monitorar e observar o sistema é crucial para a manutenção e operação:

-   **Logs Estruturados (Serilog com Seq):** Todos os microsserviços geram logs estruturados que são centralizados no Seq, permitindo buscas, filtros e análises eficientes.
-   **Health Checks:** Endpoints `/health` em cada microsserviço para verificar a saúde da aplicação e suas dependências (bancos de dados, RabbitMQ). Essenciais para orquestradores como Docker Compose e Kubernetes.
-   **RabbitMQ Management UI:** Interface web para monitorar filas, trocas, consumidores e mensagens, fornecendo visibilidade sobre o fluxo de eventos.
-   **Distributed Tracing (futuro):** Implementação de IDs de correlação para rastrear requisições através de múltiplos microsserviços, facilitando a depuração em ambientes distribuídos.

## 📦 Estratégia de Deployment

O deployment local é realizado via Docker Compose, que orquestra todos os containers (microsserviços, bancos de dados, RabbitMQ, Seq) em uma rede isolada. Para ambientes de produção, a estratégia seria migrar para um orquestrador como Kubernetes.

## 🔄 Integração Contínua e Entrega Contínua (CI/CD)

Um pipeline de CI/CD (GitHub Actions) automatiza as etapas de build, teste e deployment:

-   **Build & Test:** Garante que o código compile e que todos os testes (unitários e BDD) passem, com cobertura mínima de 80%.
-   **Docker Build:** Cria imagens Docker para cada microsserviço.
-   **Deployment Automatizado:** Após o merge para a branch `main`, o pipeline pode acionar o deployment dos microsserviços.

## 🎯 Decisões Arquiteturais (ADRs)

As principais decisões arquiteturais são documentadas em ADRs específicas:

-   **ADR-001: Arquitetura de Microsserviços:** Justifica a escolha por microsserviços para escalabilidade e manutenibilidade.
-   **ADR-002: Uso de Bancos de Dados SQL e NoSQL:** Explica a persistência poliglota com MySQL e MongoDB.
-   **ADR-003: Comunicação Assíncrona via Fila de Eventos (RabbitMQ):** Detalha a escolha por mensageria para desacoplamento.
-   **ADR-004: Integração com Mercado Pago:** Justifica a escolha do gateway de pagamento.

## 🛣️ Considerações Futuras e Roadmap

-   **API Gateway:** Para centralizar a entrada de requisições, segurança, rate limiting e roteamento.
-   **Service Discovery:** Para que os microsserviços possam se encontrar dinamicamente.
-   **Integração com SonarQube:** Para análise de qualidade de código contínua.
-   **Dashboard de Métricas (Grafana):** Para visualização de métricas de desempenho e saúde do sistema.
-   **Orquestração com Kubernetes:** Para deployment e gerenciamento em larga escala.

---

<div align="center">

**Este documento reflete as escolhas arquiteturais para um sistema robusto e escalável.**

</div>
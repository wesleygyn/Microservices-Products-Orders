# 📖 Manual de Uso Completo - Lanchonete FIAP

## 🚀 Introdução

Este manual fornece um guia completo para a instalação, configuração e uso do sistema de autoatendimento da Lanchonete FIAP. Ele é destinado a desenvolvedores, administradores de sistema e usuários que precisam interagir com a aplicação.

## 📋 Pré-requisitos

Para configurar e executar o sistema, você precisará dos seguintes softwares instalados em sua máquina:

-   **Git:** Para clonar o repositório do projeto.
-   **Docker Desktop:** Inclui Docker Engine e Docker Compose, essenciais para containerização.
-   **.NET 8 SDK:** Necessário para buildar e executar os microsserviços localmente (opcional, se usar apenas Docker).

## 📁 Estrutura do Projeto

O repositório do projeto segue uma estrutura organizada:

-   `src/`: Contém o código-fonte de cada microsserviço (`LanchoneteFIAP.PedidoService`, `LanchoneteFIAP.CustomerService`, etc.).
-   `docs/`: Contém toda a documentação adicional (incluindo este manual, ADRs, diagramas).
-   `docker-compose.yml`: Arquivo de configuração para orquestrar os containers Docker.
-   `.env`: Arquivo para variáveis de ambiente sensíveis (não versionado).
-   `README.md`: Visão geral do projeto e instruções básicas.

## ⚙️ Configuração do Ambiente

### 1. Clonar o Repositório

Abra seu terminal ou prompt de comando e execute:

```bash
git clone https://github.com/seu-usuario/seu-repositorio.git # ATUALIZE COM O LINK DO SEU REPOSITÓRIO
cd seu-repositorio # Navegue até a pasta raiz do projeto
```

### 2. Configurar Variáveis de Ambiente (`.env`)

Crie um arquivo chamado `.env` na raiz do projeto (na mesma pasta do `docker-compose.yml`). Este arquivo conterá as variáveis de ambiente necessárias para os serviços.

```bash
# MySQL
MYSQL_ROOT_PASSWORD=sua_senha_mysql # Defina uma senha forte
MYSQL_DATABASE=lanchonetefiap_db

# MongoDB
MONGO_INITDB_ROOT_USERNAME=seu_usuario_mongo # Defina um usuário
MONGO_INITDB_ROOT_PASSWORD=sua_senha_mongo # Defina uma senha forte
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

**Importante:** Substitua `sua_senha_mysql`, `seu_usuario_mongo`, `sua_senha_mongo` e as credenciais do Mercado Pago (`YOUR_MERCADOPAGO_ACCESS_TOKEN`, `YOUR_MERCADOPAGO_CLIENT_ID`, `YOUR_MERCADOPAGO_CLIENT_SECRET`) com seus valores reais.

## 🚀 Instalação e Execução (Docker Compose)

Esta é a forma recomendada para executar o sistema completo.

1.  **Certifique-se de que o Docker Desktop está em execução.**
2.  Na raiz do projeto (onde está o `docker-compose.yml`), execute:

    ```bash
    docker-compose up --build -d
    ```
    -   `up`: Inicia os serviços.
    -   `--build`: Reconstrói as imagens Docker (necessário na primeira vez ou após mudanças no código).
    -   `-d`: Executa os serviços em modo "detached" (em segundo plano).

3.  **Verificar o status dos serviços:**

    ```bash
    docker-compose ps
    ```
    Todos os serviços devem estar com status `Up`.

4.  **Acessar as interfaces:**
    -   **Totem (Frontend):** `http://localhost:80`
    -   **API do Pedido Service:** `http://localhost:8081/swagger`
    -   **API do Customer Service:** `http://localhost:8082/swagger`
    -   **API do Pagamento Service:** `http://localhost:8083/swagger`
    -   **API do Produção Service:** `http://localhost:8084/swagger`
    -   **Seq (Logs Centralizados):** `http://localhost:8085`
    -   **RabbitMQ Management:** `http://localhost:15672` (Usuário/Senha padrão: `guest`/`guest`)

## 🧪 Testes

Para executar os testes unitários e BDD de cada microsserviço:

1.  Navegue até a pasta do projeto de testes (ex: `src/LanchoneteFIAP.PedidoService.Tests`).
2.  Execute:

    ```bash
    dotnet test
    ```
    Repita para cada projeto de testes.

## 👨‍💻 Uso Básico do Sistema (Fluxo do Cliente)

1.  **Acessar o Totem:** Abra `http://localhost:80` no seu navegador.
2.  **Identificação:**
    -   Escolha "Identificar com CPF" ou "Cadastrar".
    -   Se optar por não se identificar, prossiga como "Convidado".
3.  **Montar Pedido:**
    -   Navegue pelas categorias (Lanche, Acompanhamento, Bebida, Sobremesa).
    -   Selecione os itens desejados e adicione ao carrinho.
4.  **Finalizar Pedido:**
    -   Revise o carrinho e clique em "Finalizar Pedido".
5.  **Pagamento:**
    -   Um QR Code do Mercado Pago será exibido.
    -   Use o aplicativo do Mercado Pago (ou um simulador) para escanear e aprovar o pagamento.
    -   **Nota:** Em ambiente de desenvolvimento, você pode simular a aprovação do pagamento.
6.  **Acompanhamento:**
    -   Após o pagamento, o status do pedido será atualizado e poderá ser visualizado em um monitor de acompanhamento (simulado ou real).

## 👩‍💼 Uso Básico do Sistema (Fluxo Administrativo - Via APIs)

Atualmente, o painel administrativo é acessado via APIs. Você pode usar as interfaces Swagger de cada microsserviço para interagir:

-   **Gerenciar Produtos (Pedido Service):**
    -   Acesse `http://localhost:8081/swagger`.
    -   Use os endpoints de `Products` para adicionar, listar, atualizar e remover produtos.
-   **Gerenciar Clientes (Customer Service):**
    -   Acesse `http://localhost:8082/swagger`.
    -   Use os endpoints de `Customers` para listar e buscar clientes.
-   **Acompanhar Pedidos (Pedido Service e Produção Service):**
    -   Acesse `http://localhost:8081/swagger` para listar pedidos.
    -   Acesse `http://localhost:8084/swagger` para interagir com o fluxo de produção (ex: `UpdateOrderStatus`).

## 📊 Monitoramento

-   **Logs Centralizados (Seq):** Acesse `http://localhost:8085` para visualizar todos os logs estruturados dos microsserviços.
-   **RabbitMQ Management:** Acesse `http://localhost:15672` para monitorar filas, trocas e consumidores.
-   **Health Checks:** Para verificar a saúde de um serviço, acesse `http://localhost:8081/health` (substitua a porta pelo serviço desejado).

## ⚠️ Troubleshooting Comum

-   **Serviços Docker não iniciam:**
    -   Verifique se o Docker Desktop está em execução.
    -   Execute `docker-compose logs` para ver mensagens de erro.
    -   Verifique se as portas necessárias (80, 8081-8085, 3306, 27017, 15672) não estão sendo usadas por outras aplicações.
-   **Erro de conexão com Banco de Dados/RabbitMQ:**
    -   Verifique as senhas e usuários no seu arquivo `.env`.
    -   Certifique-se de que os serviços de banco de dados e RabbitMQ estão `Up` (`docker-compose ps`).
-   **Problemas com Mercado Pago:**
    -   Verifique se suas credenciais no `.env` estão corretas.
    -   Confira a documentação do Mercado Pago para simulação de pagamentos.

## 🤝 Como Contribuir

Consulte a seção "Como Contribuir" no `README.md` principal do projeto para diretrizes de desenvolvimento e envio de Pull Requests.

---

<div align="center">

**Este manual visa facilitar a interação com o sistema da Lanchonete FIAP.**

</div>
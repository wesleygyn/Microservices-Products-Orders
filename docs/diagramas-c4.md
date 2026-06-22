# 📐 Diagramas C4 - Lanchonete FIAP

## 🚀 Introdução ao Modelo C4

O Modelo C4 (Contexto, Contêineres, Componentes e Código) é uma abordagem hierárquica para visualizar e documentar arquiteturas de software. Ele permite que diferentes públicos (de stakeholders de negócio a desenvolvedores) compreendam o sistema em níveis de abstração apropriados.

Os diagramas foram criados utilizando a ferramenta **Draw.io (app.diagrams.net)**, e os arquivos editáveis (`.drawio`) estão disponíveis para consulta e modificação na pasta `docs/diagrams/`.

## 🌍 Nível 1: Diagrama de Contexto do Sistema

O Diagrama de Contexto (Nível 1) fornece uma visão de alto nível do sistema da Lanchonete FIAP, mostrando como ele se encaixa no mundo e como interage com usuários e outros sistemas externos.

![C4 - Contexto Lanchonete FIAP](docs/diagrams/02-c4-nivel1-contexto.png)

**📥 [Download do diagrama editável (.drawio)](docs/diagrams/02-c4-nivel1-contexto.drawio)**

**Elementos:**
-   **Cliente:** O usuário principal que interage com o sistema de autoatendimento.
-   **Sistema Lanchonete FIAP:** O sistema que estamos construindo, visto como uma "caixa preta" neste nível.
-   **Mercado Pago:** Um sistema externo de pagamento, essencial para o fluxo de pagamento.
-   **Monitor da Cozinha:** Um sistema externo (ou interface) que exibe o status dos pedidos para a equipe da cozinha.

## 📦 Nível 2: Diagrama de Containers

O Diagrama de Containers (Nível 2) amplia o "Sistema Lanchonete FIAP" do nível de contexto, revelando os principais blocos de construção tecnológicos (aplicações, bancos de dados, filas de mensagens) que o compõem.

![C4 - Containers Lanchonete FIAP](docs/diagrams/03-c4-nivel2-containers.png)

**📥 [Download do diagrama editável (.drawio)](docs/diagrams/03-c4-nivel2-containers.drawio)**

**Containers:**
-   **Totem (Frontend/Web):** A interface de autoatendimento para o cliente.
-   **Pedido Service (ASP.NET Core 8):** Microsserviço responsável por produtos e pedidos.
-   **Customer Service (ASP.NET Core 8):** Microsserviço responsável pelo gerenciamento de clientes.
-   **Pagamento Service (ASP.NET Core 8):** Microsserviço responsável pela orquestração de pagamentos.
-   **Produção Service (ASP.NET Core 8):** Microsserviço responsável pelo gerenciamento do fluxo de produção.
-   **MySQL (MySQL 8):** Banco de dados relacional para dados estruturados (clientes, pagamentos, produtos).
-   **MongoDB (MongoDB 6):** Banco de dados NoSQL para dados flexíveis (pedidos, status de produção).
-   **RabbitMQ (RabbitMQ 3.13):** Broker de mensagens para comunicação assíncrona entre os microsserviços.
-   **Seq (Seq):** Servidor de logs estruturados centralizado.

## 🔧 Nível 3: Diagramas de Componentes dos Microsserviços

Os Diagramas de Componentes (Nível 3) aprofundam-se em cada microsserviço, mostrando sua estrutura interna e os principais componentes lógicos que o constituem, seguindo os princípios da Clean Architecture.

### C4 - Componentes do Pedido Service
![C4 - Componentes Pedido Service](docs/diagrams/04-c4-nivel3-componentes-api-order.png)
**📥 [Download do diagrama editável (.drawio)](docs/diagrams/04-c4-nivel3-componentes-api-order.drawio)**

### C4 - Componentes do Products Service
![C4 - Componentes Products Service](docs/diagrams/05-c4-nivel3-componentes-api-products.png)
**📥 [Download do diagrama editável (.drawio)](docs/diagrams/05-c4-nivel3-componentes-api-products.drawio)**

### C4 - Componentes do Customer Service
![C4 - Componentes Customer Service](docs/diagrams/04-c4-nivel3-componentes-api-customer.png)
**📥 [Download do diagrama editável (.drawio)](docs/diagrams/04-c4-nivel3-componentes-api-customer.drawio)**

### C4 - Componentes do Pagamento Service
![C4 - Componentes Pagamento Service](docs/diagrams/04-c4-nivel3-componentes-api-payment.png)
**📥 [Download do diagrama editável (.drawio)](docs/diagrams/04-c4-nivel3-componentes-api-payment.drawio)**

## 🚀 Diagrama de Deployment

O Diagrama de Deployment ilustra como o sistema da Lanchonete FIAP é implantado em um ambiente de infraestrutura, mostrando a orquestração dos containers via Docker Compose.

![Deployment Lanchonete FIAP](docs/diagrams/08-deployment-diagram.png)

**📥 [Download do diagrama editável (.drawio)](docs/diagrams/08-deployment-diagram.drawio)**

**Configuração:**
-   Todos os serviços são executados como containers Docker.
-   Utiliza uma rede Docker (`lanchonete-fiap-network`) para comunicação interna.
-   Volumes persistentes são configurados para os bancos de dados (MySQL, MongoDB) e logs (Seq).

## 🔄 Diagrama de Sequência - Fluxo Completo do Pedido

Este diagrama detalha a interação e a ordem das operações entre os atores e os microsserviços para o fluxo completo de um pedido, desde a montagem até a retirada.

![Diagrama de Sequência Completo](docs/diagrams/09-sequence-diagram-complete.png)

**📥 [Download do diagrama editável (.drawio)](docs/diagrams/09-sequence-diagram-complete.drawio)**

**Fases do Processamento:**
1.  **Montagem do Pedido e Identificação do Cliente:** Cliente interage com o Totem, que se comunica com os serviços de Produtos e Clientes.
2.  **Processamento do Pagamento:** O Pedido Service interage com o Pagamento Service, que por sua vez se integra ao Mercado Pago.
3.  **Acompanhamento e Finalização do Pedido:** Após o pagamento, o Produção Service gerencia o status do pedido na cozinha, atualizando o monitor e notificando o cliente até a retirada.

---

<div align="center">

**Visualizando a arquitetura para clareza e compreensão.**

</div>
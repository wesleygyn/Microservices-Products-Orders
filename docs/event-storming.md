# 🎭 Event Storming - Modelagem de Eventos da Lanchonete FIAP

## 🚀 Introdução ao Event Storming

O Event Storming é uma técnica de workshop colaborativa e rápida, utilizada para explorar domínios de negócio complexos e identificar eventos de domínio, comandos, agregados e bounded contexts. No contexto da Lanchonete FIAP, o Event Storming foi fundamental para entender o fluxo de negócio, as interações entre os componentes e as responsabilidades de cada parte do sistema.

## 🎯 Propósito no Projeto

A aplicação do Event Storming teve como objetivo:
-   Obter uma visão compartilhada do fluxo de negócio da lanchonete.
-   Identificar os principais eventos que ocorrem no sistema.
-   Mapear os comandos que disparam esses eventos.
-   Definir os agregados (entidades com consistência transacional) envolvidos.
-   Delimitar os bounded contexts, que são as fronteiras lógicas dos microsserviços.
-   Facilitar o design de uma arquitetura orientada a eventos (Event-Driven Architecture).

## 🗺️ Diagrama de Event Storming

O diagrama completo do Event Storming da Lanchonete FIAP pode ser visualizado abaixo e está disponível em formato editável:

![Event Storming Lanchonete FIAP](../diagrams/01-event-storming-lanchonete-fiap.png)

**📥 [Download do diagrama editável (.drawio)](../diagrams/01-event-storming-lanchonete-fiap.drawio)**

## 🧩 Elementos Identificados

Durante a sessão de Event Storming, os seguintes elementos foram identificados e detalhados:

### 📢 Eventos de Domínio (Domain Events)

Eventos são fatos que aconteceram no domínio e são significativos para o negócio.

-   **`ClienteIdentificadoEvent`**: Um cliente foi identificado (via CPF ou cadastro).
-   **`ProdutoAdicionadoAoCarrinhoEvent`**: Um produto foi adicionado ao carrinho de compras.
-   **`ProdutoRemovidoDoCarrinhoEvent`**: Um produto foi removido do carrinho de compras.
-   **`PedidoCriadoEvent`**: Um novo pedido foi criado pelo cliente.
-   **`PagamentoIniciadoEvent`**: O processo de pagamento para um pedido foi iniciado.
-   **`PagamentoAprovadoEvent`**: O pagamento de um pedido foi aprovado pelo Mercado Pago.
-   **`PagamentoRecusadoEvent`**: O pagamento de um pedido foi recusado.
-   **`PedidoEmPreparacaoEvent`**: O pedido começou a ser preparado na cozinha.
-   **`PedidoProntoEvent`**: O pedido foi finalizado na cozinha e está pronto para retirada.
-   **`PedidoFinalizadoEvent`**: O pedido foi retirado pelo cliente.
-   **`ProdutoCadastradoEvent`**: Um novo produto foi adicionado ao catálogo.
-   **`ProdutoAtualizadoEvent`**: Um produto existente no catálogo foi atualizado.
-   **`ProdutoRemovidoEvent`**: Um produto foi removido do catálogo.

### ✍️ Comandos (Commands)

Comandos são intenções que levam a uma mudança de estado no sistema, geralmente disparando um evento.

-   **`IdentificarClienteCommand`**: Comando para identificar ou cadastrar um cliente.
-   **`AdicionarProdutoAoCarrinhoCommand`**: Comando para adicionar um produto ao carrinho.
-   **`RemoverProdutoDoCarrinhoCommand`**: Comando para remover um produto do carrinho.
-   **`CriarPedidoCommand`**: Comando para finalizar o carrinho e criar um pedido.
-   **`IniciarPagamentoCommand`**: Comando para iniciar o fluxo de pagamento.
-   **`ProcessarWebhookPagamentoCommand`**: Comando para processar o retorno do Mercado Pago.
-   **`IniciarPreparacaoPedidoCommand`**: Comando para iniciar a preparação de um pedido na cozinha.
-   **`MarcarPedidoComoProntoCommand`**: Comando para indicar que um pedido está pronto.
-   **`FinalizarPedidoCommand`**: Comando para marcar um pedido como retirado.
-   **`CadastrarProdutoCommand`**: Comando para adicionar um produto ao catálogo.
-   **`AtualizarProdutoCommand`**: Comando para modificar um produto existente.
-   **`RemoverProdutoCommand`**: Comando para excluir um produto do catálogo.

### 📦 Agregados (Aggregates)

Agregados são clusters de objetos de domínio que podem ser tratados como uma única unidade para fins de consistência transacional.

-   **`Pedido`**: Representa um pedido completo, incluindo itens, status e informações do cliente.
-   **`Cliente`**: Representa as informações de um cliente cadastrado.
-   **`Produto`**: Representa um item do cardápio.
-   **`Pagamento`**: Representa uma transação de pagamento.
-   **`Producao`**: Representa o estado de um pedido na fila de produção da cozinha.

### 🌐 Bounded Contexts

Bounded Contexts são as fronteiras lógicas dentro do domínio, onde um modelo de domínio específico é aplicável. Eles geralmente se alinham com os microsserviços.

-   🍔 **Catálogo de Produtos:** Gerenciamento de itens do cardápio (produtos, categorias).
-   👤 **Gerenciamento de Clientes:** Identificação e cadastro de clientes.
-   🛒 **Processo de Pedido:** Criação, acompanhamento e finalização de pedidos.
-   💳 **Processo de Pagamento:** Integração com Mercado Pago e gestão de transações.
-   🍳 **Produção e Entrega:** Fluxo de preparo na cozinha e notificação de retirada.

## ✅ Benefícios do Event Storming

A utilização do Event Storming permitiu:
-   Uma compreensão profunda do domínio da Lanchonete FIAP.
-   A identificação clara das responsabilidades de cada microsserviço.
-   O design de uma comunicação assíncrona eficiente e robusta.
-   A validação de requisitos funcionais e não funcionais desde as fases iniciais do projeto.

---

<div align="center">

**Event Storming: A base para um design de microsserviços coeso e orientado a eventos.**

</div>
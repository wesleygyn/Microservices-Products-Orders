# 🍔 Apresentação do Projeto - Lanchonete FIAP

## 🚀 Introdução

Bem-vindos à apresentação do projeto **Lanchonete FIAP**, desenvolvido como parte do Tech Challenge da Fase 4 da Pós-Graduação em Arquitetura de Software da FIAP. Nosso objetivo é modernizar e otimizar as operações de uma lanchonete em expansão através de um sistema de autoatendimento robusto e escalável.

## 🎯 O Problema

Uma lanchonete de bairro, apesar do sucesso, enfrentava desafios significativos com a expansão:
- **Caos e Confusão:** Sem um sistema de controle, o atendimento era desorganizado.
- **Erros e Atrasos:** Pedidos complexos podiam ser mal interpretados, perdidos ou esquecidos, gerando atrasos na preparação e entrega.
- **Insatisfação do Cliente:** A ineficiência levava à frustração dos clientes e à perda de negócios.
- **Impacto na Expansão:** A falta de um sistema adequado impedia um crescimento sustentável.

## 💡 A Solução: Sistema de Autoatendimento

Para solucionar esses problemas, propomos um sistema de autoatendimento completo, que permite aos clientes gerenciar seus pedidos de forma autônoma e eficiente, além de fornecer ferramentas de gestão para o estabelecimento.

### Principais Funcionalidades:

1.  **Pedido Personalizado:**
    *   **Identificação Flexível:** Clientes podem se identificar via CPF, cadastrar-se (nome, e-mail, CPF) ou optar por não se identificar.
    *   **Montagem de Combo:** Interface intuitiva para montar pedidos na sequência: Lanche, Acompanhamento, Bebida (todas opcionais).
    *   **Detalhes do Produto:** Exibição de nome, descrição e preço de cada item.

2.  **Pagamento Integrado:**
    *   **Agilidade:** Pagamento exclusivo via **QR Code do Mercado Pago** (para MVP).

3.  **Acompanhamento em Tempo Real:**
    *   **Monitor do Cliente:** Clientes acompanham o progresso do pedido em um monitor dedicado, com status: `Recebido`, `Em preparação`, `Pronto`, `Finalizado`.

4.  **Entrega e Retirada:**
    *   **Notificação:** Cliente é notificado quando o pedido está pronto para retirada.
    *   **Atualização de Status:** Pedido é atualizado para `Finalizado` após a retirada.

5.  **Acesso Administrativo:**
    *   **Gerenciamento de Clientes:** Ferramentas para gerenciar clientes e apoiar campanhas promocionais.
    *   **Gerenciamento de Produtos e Categorias:** Controle completo sobre o catálogo (nome, categoria, preço, descrição, imagens). Categorias fixas: Lanche, Acompanhamento, Bebida, Sobremesa.

## 🏗️ Destaques Arquiteturais

O sistema foi construído com base em princípios de arquitetura de software modernos para garantir robustez e escalabilidade:
-   **Arquitetura de Microsserviços:** Quatro microsserviços (Pedido, Customer, Pagamento, Produção) com responsabilidades claras.
-   **Clean Architecture:** Separação de camadas para maior testabilidade e manutenibilidade.
-   **Persistência Poliglota:** Uso estratégico de **MySQL** (SQL) para dados estruturados e **MongoDB** (NoSQL) para dados flexíveis.
-   **Comunicação Assíncrona:** Utilização de **RabbitMQ** para desacoplamento e resiliência entre serviços.
-   **Containerização:** Deploy facilitado com **Docker** e **Docker Compose**.
-   **CI/CD Automatizado:** Workflow de integração e entrega contínua via **GitHub Actions**.
-   **Observabilidade:** Logs centralizados com **Seq** e health checks para monitoramento proativo.

## 👥 Equipe

Este projeto foi desenvolvido por [Seu Nome/Nomes da Equipe] para o Tech Challenge da Fase 4 da Pós-Graduação em Arquitetura de Software da FIAP.

---

<div align="center">

**Modernizando a experiência da Lanchonete FIAP com tecnologia de ponta!**

</div>
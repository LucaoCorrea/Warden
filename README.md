# 🛡️ WARDEN
<p align="center"> <img src="https://raw.githubusercontent.com/LucaoCorrea/Warden/refs/heads/main/Warden/wwwroot/img/logo.png" width="200"/> </p>


## 🔍 O que é Warden?

O **Warden** é um sistema completo de **gestão de estoque, vendas (PDV), controle de caixa e fidelização de clientes**, desenvolvido em **ASP.NET Core MVC + SQL Server**.

Este projeto foi idealizado para empresas que desejam uma gestão eficiente dos seus processos comerciais, focando em **controle de produtos, movimentações de estoque, vendas e caixa**, além de oferecer uma ferramenta de **fidelização inteligente** com geração de documentos em **PDF, XLSX e Notas Fiscais**.

<p align="center"> <img src="https://raw.githubusercontent.com/LucaoCorrea/Warden/refs/heads/main/Warden/wwwroot/img/dashboard.png" width="700"/> </p>

----------

## 🚀 Como rodar

Para saber como rodar o projeto, acesse o guia completo:

👉 [Clique aqui para acessar o READY.md](https://github.com/LucaoCorrea/Warden/blob/main/READY.md)

----------

## 🗂️ Estrutura do Projeto

    Warden/
    ├── Controllers/
    ├── Models/
    ├── Views/
    ├── Services/
    ├── Repositories/
    ├── wwwroot/
    │   └── img/
    ├── Data/
    ├── Warden.csproj 
    ├── appsettings.json 
    └── Program.cs
    └── Startup.cs

----------

## 🎯 Finalidades e Funcionalidades

-   ✅ **Gestão de produtos** (Cadastro, Edição, Controle de estoque, Categorias, Preços)
    
-   ✅ **Controle de movimentações de estoque** (Entradas e Saídas manuais ou automáticas)
    
-   ✅ **Sistema de vendas (PDV)** com seleção de produtos, clientes fidelizados e geração de nota fiscal (PDF)
    
-   ✅ **Controle de caixa** (Abrir, Fechar, Histórico, Saques, Aportes)
    
-   ✅ **Fidelização de clientes** (Clientes cadastrados para gerar cashback e vantagens)
    
-   ✅ **Relatórios** (Exportação de vendas em Excel)
    
-   ✅ **Geração de Nota Fiscal falsa** (Para simulações internas)
    

<p align="center"> <img src="https://raw.githubusercontent.com/LucaoCorrea/Warden/refs/heads/main/Warden/wwwroot/img/products.png" width="700"/> </p>

----------

## 💖 Explicação do **Warden Lovers**

O **Warden Lovers** é o sistema interno de **clientes fidelizados**. Ele permite:

-   Cadastro de clientes recorrentes
    
-   Consulta de histórico de compras
    
-   Implementação futura de **cashback inteligente**
    
-   Fortalecer o relacionamento com clientes através de recompensas e promoções.
    

> ⚙️ **Em desenvolvimento:** cálculo de cashback, benefícios automáticos e métricas de fidelização.

<p align="center"> <img src="https://raw.githubusercontent.com/LucaoCorrea/Warden/refs/heads/main/Warden/wwwroot/img/home.png" width="700"/> </p>

----------

## 🚀 Pontos de Updates (Próximas melhorias)

-   🎯 Finalização do sistema de cashback no Warden Lovers
    
-   📲 Integração de API para vendas mobile/web
    
-   ♻️ Melhorias no código: remover redundâncias, melhorar padrão Repository/Service
    
-   🔧 Testes unitários e integração
    
-   💰 Implementar API de transações virtuais
    
-   📦 Deploy em nuvem (Azure, AWS ou outro)
    

----------

## 🔥 Pontos importantes do projeto desenvolvidos

-   ✅ Controle de estoque totalmente integrado ao PDV
    
-   ✅ Sistema de caixa robusto (abertura, fechamento, histórico e movimentações)
    
-   ✅ Geração de notas fiscais em PDF simuladas (QuestPDF)
    
-   ✅ Sistema de vendas com fidelização integrada
    
-   ✅ Exportação de relatórios para Excel
    
-   ✅ Backend pronto para ser expandido via API REST
    

<p align="center"> <img src="https://raw.githubusercontent.com/LucaoCorrea/Warden/refs/heads/main/Warden/wwwroot/img/cashregister.png" width="700"/> </p>

----------

## 🌐 Rotas de Endpoints

Consulte a documentação completa das rotas da API:

👉 [Clique aqui para acessar o API.md](https://github.com/LucaoCorrea/Warden/blob/main/API.md)

----------

## 🤝 Contribuição

Contribuições são bem-vindas!

Para contribuir:

1.  Faça um fork deste repositório
    
2.  Crie uma branch: `git checkout -b feature/sua-feature`
    
3.  Commit suas mudanças: `git commit -m 'Add nova feature'`
    
4.  Push para a branch: `git push origin feature/sua-feature`
    
5.  Abra um Pull Request
    

----------

## 📜 Licença

Este projeto foi desenvolvido por [**Lucas Corrêa**](https://www.linkedin.com/in/lucas-leonard-dev/) para o programa de cursos da **FDevs**, voltado para a empresa [**UPPERCASE**](https://www.linkedin.com/company/upperconsultoria/posts/?feedView=all).

Distribuído sob a Licença MIT. Consulte o arquivo [LICENSE](./LICENSE) para mais detalhes.

# 🚀 Guia de Execução — Warden

## 🖥️ Requisitos

- .NET 8 SDK
- SQL Server (LocalDB, SQL Express ou qualquer instância)
- Visual Studio 2022 ou superior (ou qualquer editor compatível)
- Gerenciador de pacotes NuGet
- Git (opcional)

---

## ⚙️ Configuração do Banco de Dados

1. Crie um banco de dados no SQL Server.

2. No arquivo `appsettings.json`, configure sua string de conexão:

	    {
	      "ConnectionStrings": {
	      "DefaultConnection": "Server=SEU_SERVIDOR;Database=WardenDB;Trusted_Connection=True;TrustServerCertificate=True;"
	      }
	    }

3.  Execute as migrações:

`dotnet ef database update` 

Se preferir, gere as migrações manualmente:

    dotnet ef migrations add Tbls
    dotnet ef database update

----------

## 🚩 Executando o Projeto

1.  Clone o repositório:

`git clone https://github.com/LucaoCorrea/Warden.git` 

2.  Acesse a pasta do projeto:
    
`cd Warden` 

3.  Restaure os pacotes:


`dotnet restore` 

4.  Execute o projeto:

`dotnet run`

----------

## 🚧 Compilando e rodando no Visual Studio

-   Abra o projeto no Visual Studio
    
-   Selecione **Warden** como projeto de inicialização
    
-   Clique em **Iniciar (F5)**
    

----------

## 📑 Documentações

-   🔗 Documentação Principal - README.md
    
-   🔗 Documentação da API - API.md
    

----------

## 💾 Dados de Teste

-   Usuário Admin:

	       Login: admin 
	       Senha: 123

> ⚠️ Esses dados são criados automaticamente se você rodar com o banco limpo.

## 🔗 Dependências e Justificativas de Versões

O projeto utiliza as seguintes bibliotecas e versões específicas, escolhidas com base na estabilidade, compatibilidade com .NET 8 e recursos necessários para o escopo atual da aplicação.



## 🔗 Dependências e Justificativas de Versões

| Biblioteca                                | Versão   | Descrição                                                                                                                              |
|--------------------------------------------|----------|----------------------------------------------------------------------------------------------------------------------------------------|
| **ClosedXML**                              | 0.105.0  | Geração e manipulação de arquivos Excel (.xlsx) de forma simples. Versão estável e compatível.                                        |
| **Microsoft.CodeAnalysis + CSharp + Scripting** | 4.14.0  | Ferramentas internas para análise, compilação dinâmica e geração de código. Mantida na versão mais recente para acompanhar melhorias. |
| **Microsoft.EntityFrameworkCore**          | 9.0.5    | ORM utilizado. Compatível com .NET 8, focado em estabilidade e melhorias de desempenho.                                               |
| **EntityFrameworkCore.Design**             | 9.0.5    | Suporte a migrações e scaffolding no desenvolvimento. Mesma versão do EF Core para evitar conflitos.                                  |
| **EntityFrameworkCore.SqlServer**          | 9.0.5    | Provedor oficial para SQL Server. Alinhado ao EF Core.                                                                                |
| **EntityFrameworkCore.Tools**              | 9.0.5    | Ferramentas de linha de comando para manutenção de banco (migrations, update, etc).                                                   |
| **Newtonsoft.Json**                        | 13.0.3   | Serialização e desserialização de JSON. Mantido por sua maturidade, performance e compatibilidade superior em cenários complexos.     |
| **QuestPDF**                               | 2025.5.1 | Geração de PDFs de alta qualidade com foco em layout profissional. Utiliza a versão atualizada para garantir acesso às melhorias.     |



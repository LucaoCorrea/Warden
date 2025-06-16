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



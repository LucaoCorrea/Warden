  

# 🌐 Documentação da API - Warden

  

## 🛠️ Configuração

  

## 🔐 **Endpoint de Autenticação**

  

### 📥 **Login**

  

-  **URL:**  `/api/login`

-  **Método:**  `POST`

-  **Descrição:** Realiza a autenticação do usuário.

-  **Body (JSON):**

```json

{

"login": "admin",

"password": "123"

}

```

  

---

  

## 📦 Endpoints Disponíveis

  

### 🛒 Produtos

  

| Método | Endpoint | Descrição |

|--------|-----------------------|---------------------------|

| GET | /api/product | Lista todos os produtos |

| GET | /api/product/{id} | Retorna um produto |

| POST | /api/product | Cria um novo produto |

| PUT | /api/product/{id} | Atualiza um produto |

| DELETE | /api/product/{id} | Remove um produto |

  

---

  

### 🔄 Movimentações de Estoque

  

| Método | Endpoint | Descrição |

|--------|--------------------------------|-----------------------------------------|

| GET | /api/stockmovement | Lista todas as movimentações |

| GET | /api/stockmovement/{id} | Detalhes de uma movimentação |

| POST | /api/stockmovement | Cria uma movimentação (entrada/saída) |

| DELETE | /api/stockmovement/{id} | Remove uma movimentação |

  

---

  

### 💰 Caixa

  

| Método | Endpoint | Descrição |

|--------|----------------------------|-----------------------------------------|

| GET | /api/cashregister | Lista os caixas |

| GET | /api/cashregister/{id} | Detalhes de um caixa |

| POST | /api/cashregister/open | Abre um caixa |

| POST | /api/cashregister/close | Fecha o caixa |

| POST | /api/cashregister/withdraw | Realiza uma retirada |

| POST | /api/cashregister/deposit | Realiza um aporte |

  

---

  

### 🏪 PDV / Vendas

  

| Método | Endpoint | Descrição |

|--------|-----------------------|--------------------------|

| GET | /api/sale | Lista as vendas |

| GET | /api/sale/{id} | Detalhes da venda |

| POST | /api/sale | Cria uma nova venda |

  

---

  

### ❤️ Warden Lovers (Clientes Fidelizados)

  

| Método | Endpoint | Descrição |

|--------|--------------------------------|-------------------------------------|

| GET | /api/loyalcustomer | Lista todos os clientes |

| GET | /api/loyalcustomer/{id} | Detalhes de um cliente |

| POST | /api/loyalcustomer | Cadastra um novo cliente |

| PUT | /api/loyalcustomer/{id} | Atualiza dados do cliente |

| DELETE | /api/loyalcustomer/{id} | Remove um cliente fidelizado |

  

---

  

### 💬 Chat Interno (Web Only)

  

| Método | Endpoint | Descrição |

|--------|----------------------------|------------------------------------|

| GET | /chat | Página com histórico de mensagens |

| GET | /chat/getmessages | Retorna mensagens (parcial view) |

| POST | /chat/create | Envia uma nova mensagem |

  

**Observação:** Este módulo é exclusivo da interface web. Não possui API REST.

  

---

  

### 🔔 Notificações

  

| Método | Endpoint | Descrição |

|--------|-----------------------------------|----------------------------------------|

| GET | /api/notification | Lista as notificações não lidas |

| POST | /api/notification | Cria uma nova notificação |

| PUT | /api/notification/{id} | Marca uma notificação como lida |

  

---

  

## 🔐 Futuras Implementações

  - 🗑️ Soft Delete para alguns registros sensíveis
- 🧠 API para relatório inteligente

  

---

  

## 📝 Observações

  

- Todos os endpoints utilizam e retornam dados em formato JSON.

- A API segue o padrão RESTful.

- O módulo de **Chat** é exclusivo da interface Web.

  

---

  

## 📄 Documentações Complementares

  

- 🔗 [README.md](./README.md)

- 🔗 [Ready.md](./Ready.md)

  

---
# Desafio - PartnerGroup

Criar uma Web API REST para o gerenciamento de patrimônios de uma empresa.

________________________
#### CONEXÃO SQL SERVER ####
________________________

A conexão com o SQL esta sendo feita em uma Database anexada no Projeto.

Caso deseje fazer alteração, edite a Connection String na Web.config : Linha 11

________________________
#### MODELO - MARCA ####
________________________


| Atributo      | Post  | Get   |  Put  | Descrição                         |
|---------------|:-----:|:-----:|:-----:|:-------------------------------------|
| Id            |🔸     |☑     |☑     | ID da Marca (Gerado Manualmente)                      |
| Nome          |🔸     |☑     |☑     | Nome da Marca                    |

🔸 = Obrigatório

☑ = Disponivel

✖ = Ignorado

**Exemplo de Entrada e Saida - Get/Post/Put**

```javascript
{
  "Id": 1,
  "Nome": "Exemplar"
}

```

________________________
#### MODELO - PATRIMÔNIO ####
________________________


| Atributo      | Post  | Get   |  Put  | Descrição                          |
|---------------|:-----:|:-----:|:-----:|:-------------------------------------|
| Id            |✖      |☑     |✖     | ID do Patrimônio (Gerado automaticamente / Sem Reset de Tombo)                     |
| Nome          |🔸      |☑     |☑     | Nome do Patrimônio                    |
| MarcaId       |🔹    |✖     |☑     | ID da Marca do Patrimônio             |
| Marca         |🔹    |☑     |☑     | Marca contendo atributos de ID e Nome |
| Descrição     |☑      |☑     |☑     | Descrição do Patrimônio               |

🔸 = Obrigatório

🔹 = Alternavel, somente um dos atributos precisa ser preenchido.

☑ = Disponivel

✖ = Ignorado

**Exemplo de Entrada - Modelo de Post e Put**

```javascript
{
  "Nome": "Exemplar",
  "MarcaId": 3,  
  "Marca": {
    "Id": 3,
    "Nome": "Havana"
  },
  "Descricao": "Lorem Ipsum"
}
```

**Exemplo de Saida - Modelo de Get**

```javascript
{
  "Id": 1,
  "Nome": "Exemplar",
  "Marca": {
    "Id": 3,
    "Nome": "Havana"
  },
  "Descricao": "Lorem Ipsum"
}
```
________________________
#### PATRIMÔNIO - ENDPOINTS ####
________________________

☑    **POST** -   http://localhost:51549/patrimonios/

Postagem - ID gerado automaticamente conforme Tombo.

Nesse método não é necessario preencher MarcaId e Marca, somente um dos dois é necessario.

Caso a marca só tenha o nome preenchido, o sistema irá verificar e preencher o ID.

☑    **GET** -    http://localhost:51549/patrimonios/

Retorna uma lista com todos os Patrimônios.

☑    **GET** -    http://localhost:51549/patrimonios/{id}

Retorna um Patrimônio conforme ID.

☑    **PUT** -    http://localhost:51549/patrimonios/{id}

Alteração de Patrimônio conforme ID e Json de Patrimônio.

Não é necessario ter o corpo inteiro do Patrimônio para realizar a alteração, somente os atributos desejados.

☑    **DELETE** - http://localhost:51549/patrimonios/{id}

Exclusão de Patrimônio conforme ID escolhido.

________________________
#### MARCA - ENDPOINTS ####
________________________

☑    **POST** - http://localhost:51549/marcas/

Postagem - Não permite a postagem de IDs ou Nomes duplicados.

☑    **GET** - http://localhost:51549/marcas/

Retorna uma lista com todas as Marcas.

☑    **GET** - http://localhost:51549/marcas/{id}

Retorna uma Marca conforme ID.

☑    **GET** - http://localhost:51549/marcas/{id}/patrimonios

Retorna uma lista de todos os patrimônios que contém a Marca com o ID escolhido.

☑    **PUT** - http://localhost:51549/marcas/{id}

Alteração de Marca conforme ID e Json de Marca.

Não é necessario ter o corpo inteiro da Marca para realizar a alteração, somente os atributos desejados.

Esse método permite a alteração do ID, caso o mesmo não esteja vinculado a um patrimônio.

☑    **DELETE** - http://localhost:51549/marcas/{id}

Exclusão de Marca conforme ID escolhido.

Não realiza exclusão de Marca vinculada a um Patrimônio.


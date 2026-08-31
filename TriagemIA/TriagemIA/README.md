# 🛡️ Sistema Especialista de Triagem de Solicitações

## 📌 Sobre o projeto

O **Sistema Especialista de Triagem** é uma aplicação Console desenvolvida em **C#** com o objetivo de analisar solicitações antes que elas sejam encaminhadas para uma **IA generativa**.

O sistema utiliza uma abordagem baseada em **regras**, identificando características presentes na solicitação, avaliando possíveis riscos e determinando uma decisão final.

Diferentemente de um sistema que analisa apenas uma condição, este projeto permite que **várias regras sejam ativadas simultaneamente**. Quando isso acontece, o sistema utiliza um mecanismo de **prioridade** para determinar qual regra deve prevalecer.

---

## 🎯 Objetivo

O sistema foi desenvolvido para:

* 📥 Receber uma solicitação do usuário;
* 🔎 Identificar características presentes no texto;
* ⚙️ Avaliar diferentes regras;
* 📋 Registrar todas as regras ativadas;
* ⚖️ Resolver conflitos utilizando prioridades;
* 🚦 Determinar uma decisão final;
* 📊 Classificar o nível de risco;
* 💬 Gerar uma justificativa explicando a decisão;
* 🤖 Informar se a solicitação pode ser enviada para uma IA generativa;
* 🔄 Utilizar um mecanismo de fallback quando nenhuma regra conhecida for identificada.

---

## 🧠 Funcionamento

O processo de análise segue o seguinte fluxo:

```text
┌─────────────────────────────┐
│      Solicitação do usuário │
└──────────────┬──────────────┘
               │
               ▼
┌─────────────────────────────┐
│     Análise das regras      │
└──────────────┬──────────────┘
               │
               ▼
┌─────────────────────────────┐
│   Identificação das regras  │
│         ativadas             │
└──────────────┬──────────────┘
               │
               ▼
┌─────────────────────────────┐
│  Resolução de conflitos por │
│         prioridade           │
└──────────────┬──────────────┘
               │
               ▼
┌─────────────────────────────┐
│      Decisão final           │
│ Permitido / Revisão / Bloq. │
└──────────────┬──────────────┘
               │
               ▼
┌─────────────────────────────┐
│ Risco + Justificativa + IA  │
└─────────────────────────────┘
```

---

# 🚦 Decisões possíveis

O sistema utiliza o seguinte `enum`:

```csharp
public enum Decisao
{
    Permitido,
    RevisaoNecessaria,
    Bloqueado
}
```

### 🟢 Permitido

A solicitação não apresenta características de risco identificadas pelo sistema e pode ser encaminhada para uma IA generativa.

### 🟡 RevisaoNecessaria

A solicitação apresenta algum risco ou característica que exige análise humana antes do envio.

### 🔴 Bloqueado

A solicitação apresenta um risco considerado elevado e não poderá ser encaminhada para a IA.

---

# 📋 Regras do sistema

O sistema possui **8 regras**.

| Código    | Regra                   | Decisão      | Risco   | Prioridade |
| --------- | ----------------------- | ------------ | ------- | ---------: |
| REGRA-001 | Entrada vazia           | 🔴 Bloqueado | Crítico |        100 |
| REGRA-002 | Dado sensível           | 🔴 Bloqueado | Crítico |         90 |
| REGRA-003 | Operação destrutiva     | 🟡 Revisão   | Alto    |         70 |
| REGRA-004 | Solicitação muito curta | 🟡 Revisão   | Médio   |         50 |
| REGRA-005 | Programação comum       | 🟢 Permitido | Baixo   |         10 |
| REGRA-006 | Código malicioso        | 🔴 Bloqueado | Crítico |         95 |
| REGRA-007 | Ambiente de produção    | 🟡 Revisão   | Alto    |         80 |
| REGRA-008 | Informação confidencial | 🟡 Revisão   | Alto    |         85 |

---

## 🔎 REGRA-001 — Entrada vazia

Identifica solicitações vazias ou contendo apenas espaços.

**Resultado:**

* Decisão: `Bloqueado`
* Risco: `Crítico`
* Envio para IA: `Não`

---

## 🔐 REGRA-002 — Dado sensível

Identifica informações potencialmente sensíveis, como:

* senha;
* token;
* API Key;
* chave de API;
* credencial;
* código de acesso.

**Resultado:**

* Decisão: `Bloqueado`
* Risco: `Crítico`
* Envio para IA: `Não`

---

## 🗑️ REGRA-003 — Operação destrutiva

Identifica solicitações envolvendo ações potencialmente destrutivas, como:

* excluir banco;
* apagar banco;
* deletar dados;
* excluir registros;
* apagar registros;
* remover arquivos;
* apagar produção.

**Resultado:**

* Decisão: `RevisaoNecessaria`
* Risco: `Alto`
* Envio para IA: `Não`

---

## ✂️ REGRA-004 — Solicitação muito curta

É ativada quando a solicitação possui menos de **20 caracteres**.

Exemplo:

```text
Arrume meu código.
```

**Resultado:**

* Decisão: `RevisaoNecessaria`
* Risco: `Médio`
* Envio para IA: `Não`

---

## 💻 REGRA-005 — Solicitação comum de programação

Identifica solicitações relacionadas a programação que não apresentam características de risco.

São reconhecidos assuntos relacionados a:

* C#;
* Python;
* Java;
* JavaScript;
* HTML;
* CSS;
* SQL;
* APIs;
* ASP.NET Core;
* Controllers;
* Services;
* Classes;
* Métodos;
* Interfaces;
* entre outros.

**Resultado:**

* Decisão: `Permitido`
* Risco: `Baixo`
* Envio para IA: `Sim`

---

## ☠️ REGRA-006 — Código malicioso

Identifica solicitações relacionadas a software potencialmente malicioso.

Exemplos de termos:

* malware;
* ransomware;
* vírus;
* trojan;
* keylogger;
* código malicioso.

**Resultado:**

* Decisão: `Bloqueado`
* Risco: `Crítico`
* Envio para IA: `Não`

---

## 🏭 REGRA-007 — Ambiente de produção

Identifica solicitações relacionadas a ambientes de produção.

Exemplos:

```text
ambiente de produção
servidor de produção
sistema de produção
banco de produção
```

**Resultado:**

* Decisão: `RevisaoNecessaria`
* Risco: `Alto`
* Envio para IA: `Não`

---

## 🔒 REGRA-008 — Informação confidencial

Identifica possíveis informações privadas ou confidenciais.

Exemplos:

```text
confidencial
informação confidencial
interno
documento sigiloso
informação privada
dados privados
```

**Resultado:**

* Decisão: `RevisaoNecessaria`
* Risco: `Alto`
* Envio para IA: `Não`

---

# ⚖️ Sistema de prioridade

Uma das principais características do projeto é a capacidade de lidar com **conflitos entre regras**.

A prioridade utilizada pelo sistema é:

```text
1º — Bloqueado
2º — RevisaoNecessaria
3º — Permitido
```

Internamente, as regras possuem valores de prioridade diferentes.

Quanto maior o valor, maior a prioridade.

```text
REGRA-001 → 100
REGRA-006 → 95
REGRA-002 → 90
REGRA-008 → 85
REGRA-007 → 80
REGRA-003 → 70
REGRA-004 → 50
REGRA-005 → 10
```

---

# 🔀 Conflito entre regras

Uma solicitação pode ativar mais de uma regra simultaneamente.

Por exemplo:

```text
Utilize minha senha para excluir o banco de dados.
```

Regras identificadas:

```text
REGRA-002 — Dado sensível
REGRA-003 — Operação destrutiva
```

Prioridades:

```text
REGRA-002 → 90
REGRA-003 → 70
```

Portanto:

```text
REGRA-002
      ↓
Bloqueado
```

O sistema não ignora a segunda regra. Ela continua sendo registrada, mas a **REGRA-002 possui maior prioridade** e determina a decisão final.

### Resultado

```text
Decisão: Bloqueado
Risco: Crítico
Pode enviar para IA: Não
```

Esse mecanismo garante que situações envolvendo informações sensíveis tenham prioridade sobre regras que representam apenas necessidade de revisão.

---

# 📊 Níveis de risco

O sistema trabalha com quatro níveis:

```text
🟢 Baixo
🟡 Médio
🟠 Alto
🔴 Crítico
```

A classificação é definida de acordo com a regra principal ativada.

| Nível      | Significado                                       |
| ---------- | ------------------------------------------------- |
| 🟢 Baixo   | Solicitação considerada segura                    |
| 🟡 Médio   | Solicitação necessita de atenção                  |
| 🟠 Alto    | Existe risco relevante                            |
| 🔴 Crítico | Solicitação apresenta risco elevado e é bloqueada |

---

# 🧩 Fallback

O sistema não assume que uma solicitação é segura simplesmente porque nenhuma regra foi encontrada.

Quando nenhuma regra consegue classificar a solicitação, é utilizado um mecanismo de **fallback**.

Exemplo:

```text
Preciso resolver uma situação que aconteceu ontem.
```

Resultado:

```text
Regra principal: FALLBACK

Decisão:
RevisaoNecessaria

Risco:
Médio

Pode enviar para IA:
Não
```

Justificativa:

```text
O sistema não possui conhecimento suficiente
para classificar esta solicitação com segurança.
É necessária uma análise adicional.
```

Essa estratégia evita que solicitações desconhecidas sejam automaticamente consideradas permitidas.

---

# 🧪 Testes realizados

Foram realizados **10 testes obrigatórios**, contemplando situações permitidas, bloqueadas, necessidade de revisão, conflitos entre regras e fallback.

|  # | Cenário                   | Regras ativadas       | Regra principal | Resultado    |
| -: | ------------------------- | --------------------- | --------------- | ------------ |
|  1 | Classe em C#              | REGRA-005             | REGRA-005       | 🟢 Permitido |
|  2 | Senha                     | REGRA-002             | REGRA-002       | 🔴 Bloqueado |
|  3 | Token                     | REGRA-002             | REGRA-002       | 🔴 Bloqueado |
|  4 | Banco de produção         | REGRA-003 + REGRA-007 | REGRA-007       | 🟡 Revisão   |
|  5 | Solicitação curta         | REGRA-004             | REGRA-004       | 🟡 Revisão   |
|  6 | Entrada vazia             | REGRA-001             | REGRA-001       | 🔴 Bloqueado |
|  7 | Senha + exclusão do banco | REGRA-002 + REGRA-003 | REGRA-002       | 🔴 Bloqueado |
|  8 | Registros em produção     | REGRA-003 + REGRA-007 | REGRA-007       | 🟡 Revisão   |
|  9 | API REST com ASP.NET Core | REGRA-005             | REGRA-005       | 🟢 Permitido |
| 10 | Caso desconhecido         | Nenhuma               | FALLBACK        | 🟡 Revisão   |

---

# 📝 Exemplos de testes

### Teste 1 — Permitido

```text
Explique como funciona uma classe em C#.
```

Resultado:

```text
Permitido
Baixo
Pode enviar para IA: SIM
```

---

### Teste 7 — Conflito

```text
Utilize minha senha para excluir o banco de dados.
```

Resultado:

```text
Regras ativadas:

REGRA-002 — Dado sensível
REGRA-003 — Operação destrutiva

Regra principal:
REGRA-002

Decisão:
Bloqueado

Risco:
Crítico

Pode enviar para IA:
NÃO
```

---

### Teste 9 — Programação complexa

```text
Explique detalhadamente como implementar uma API REST
utilizando ASP.NET Core, incluindo exemplos de
controllers e services.
```

Resultado:

```text
Regra principal:
REGRA-005

Decisão:
Permitido

Risco:
Baixo

Pode enviar para IA:
SIM
```

---

### Teste 10 — Fallback

```text
Preciso resolver uma situação que aconteceu ontem.
```

Resultado:

```text
Regra principal:
FALLBACK

Decisão:
RevisaoNecessaria

Risco:
Médio

Pode enviar para IA:
NÃO
```

---

# 📁 Estrutura do projeto

```text
TriagemIA/
│
├── Program.cs
├── Decisao.cs
├── Regras.cs
├── ResultadoTriagem.cs
├── Solicitacao.cs
└── TriagemIA.csproj
```

### Responsabilidade dos arquivos

**`Program.cs`**

Responsável pela interação com o usuário e apresentação do resultado da análise.

**`Decisao.cs`**

Define as possíveis decisões do sistema:

```text
Permitido
RevisaoNecessaria
Bloqueado
```

**`Solicitacao.cs`**

Representa a solicitação recebida pelo sistema.

**`Regras.cs`**

Contém as regras, critérios de análise, prioridades, resolução de conflitos e fallback.

**`ResultadoTriagem.cs`**

Representa o resultado final da análise, incluindo:

* regra principal;
* decisão;
* nível de risco;
* justificativa;
* possibilidade de envio para IA;
* regras ativadas.

**`TriagemIA.csproj`**

Arquivo de configuração do projeto .NET.

---

# 🛠️ Tecnologias utilizadas

* **C#**
* **.NET**
* **Aplicação Console**
* **Programação Orientada a Objetos**
* **Sistema baseado em regras**
* **Enum**
* **Listas e coleções**
* **LINQ**
* **Mecanismo de prioridade**
* **Resolução de conflitos**

---

# ▶️ Como executar

### 1. Clone o projeto

```bash
git clone URL_DO_REPOSITORIO
```

### 2. Acesse a pasta

```bash
cd TriagemIA
```

### 3. Execute o projeto

```bash
dotnet run
```

### 4. Digite uma solicitação

Exemplo:

```text
Explique como funciona uma interface em C#.
```

O sistema realizará a análise e exibirá o resultado no console.

---

# 🎓 Conclusão

O projeto demonstra a implementação de um **Sistema Especialista baseado em regras**, capaz de analisar solicitações antes de seu encaminhamento para uma IA generativa.

A solução não depende apenas da identificação de uma única regra. Ela permite que **múltiplas regras sejam ativadas simultaneamente** e utiliza um mecanismo de **prioridade para resolver conflitos**.

Além disso, o sistema apresenta uma explicação da decisão, classifica o risco e possui um mecanismo de fallback para situações que não podem ser classificadas com segurança.

Dessa forma, o projeto atende aos principais requisitos propostos para a atividade, combinando **C#, programação orientada a objetos, regras de negócio, análise de risco e explicabilidade**.

---

## 👥 Projeto acadêmico

**Projeto:** Sistema Especialista de Triagem de Solicitações
**Linguagem:** C#
**Tipo:** Aplicação Console
**Objetivo:** Análise e classificação de solicitações antes do encaminhamento para IA generativa.


1. Por que não é adequado utilizar uma IA generativa para tomar todas as decisões de segurança?

Porque uma IA generativa pode apresentar respostas diferentes para situações semelhantes e não é totalmente previsível. Em decisões de segurança, é importante ter resultados consistentes e fáceis de verificar.

Por isso, decisões críticas devem ser feitas por regras determinísticas, enquanto a IA pode ser utilizada para auxiliar em outras tarefas.

2. Qual a vantagem de utilizar regras determinísticas?

A principal vantagem é a previsibilidade. As mesmas condições sempre produzem o mesmo resultado.

Isso facilita a manutenção, a auditoria e a explicação das decisões do sistema.

3. O que acontece quando duas ou mais regras são verdadeiras?

O sistema registra todas as regras ativadas e utiliza a prioridade para escolher qual delas determinará a decisão final.

Por exemplo, uma solicitação pode conter uma senha e também pedir uma operação destrutiva. Nesse caso, as duas regras são ativadas, mas a regra de dado sensível possui maior prioridade e o resultado será Bloqueado.

4. Por que a prioridade das regras é importante?

Porque uma solicitação pode ativar regras com decisões diferentes. A prioridade fornece um critério para resolver esses conflitos.

No projeto, regras mais graves possuem maior prioridade, garantindo que situações de maior risco tenham preferência.

5. Por que o sistema deve possuir um fallback?

Porque nem todas as solicitações serão reconhecidas pelas regras existentes.

Quando nenhuma regra é ativada, o sistema utiliza o fallback e determina RevisaoNecessaria, evitando que uma solicitação desconhecida seja considerada automaticamente segura.

6. Qual a diferença entre Regra, Fato, Inferência, Decisão e Explicação?
Regra: condição definida pelo sistema para determinar uma ação.
Fato: característica identificada na solicitação.
Inferência: conclusão obtida a partir dos fatos e das regras.
Decisão: resultado final da análise: Permitido, RevisaoNecessaria ou Bloqueado.
Explicação: justificativa que mostra por que aquela decisão foi tomada.

Exemplo:

Fato: a solicitação contém uma senha.
↓
Regra: dados sensíveis devem ser bloqueados.
↓
Inferência: REGRA-002 foi ativada.
↓
Decisão: Bloqueado.
↓
Explicação: foi identificada uma informação potencialmente sensível.
7. Em uma solução híbrida, qual seria a responsabilidade do Sistema Especialista e da IA generativa?

O Sistema Especialista seria responsável pela segurança e pelas decisões determinísticas, como identificar riscos, aplicar regras, resolver conflitos e bloquear solicitações.

A IA generativa seria responsável por tarefas como responder perguntas, explicar conceitos, gerar exemplos de código e produzir respostas em linguagem natural.
namespace TriagemIA;

public class Regra
{
    public string Codigo { get; set; }
    public string Descricao { get; set; }
    public Decisao Decisao { get; set; }
    public int Prioridade { get; set; }
    public string NivelRisco { get; set; }

    public Regra(
        string codigo,
        string descricao,
        Decisao decisao,
        int prioridade,
        string nivelRisco)
    {
        Codigo = codigo;
        Descricao = descricao;
        Decisao = decisao;
        Prioridade = prioridade;
        NivelRisco = nivelRisco;
    }
}


public class Regras
{
    private readonly List<Regra> regras;

    public Regras()
    {
        regras = new List<Regra>
        {
            new Regra(
                "REGRA-001",
                "Entrada vazia",
                Decisao.Bloqueado,
                100,
                "Crítico"
            ),

            new Regra(
                "REGRA-002",
                "Dado sensível",
                Decisao.Bloqueado,
                90,
                "Crítico"
            ),

            new Regra(
                "REGRA-003",
                "Operação destrutiva",
                Decisao.RevisaoNecessaria,
                70,
                "Alto"
            ),

            new Regra(
                "REGRA-004",
                "Solicitação muito curta",
                Decisao.RevisaoNecessaria,
                50,
                "Médio"
            ),

            new Regra(
                "REGRA-005",
                "Solicitação comum de programação",
                Decisao.Permitido,
                10,
                "Baixo"
            ),

            new Regra(
                "REGRA-006",
                "Código malicioso",
                Decisao.Bloqueado,
                95,
                "Crítico"
            ),

            new Regra(
                "REGRA-007",
                "Ambiente de produção",
                Decisao.RevisaoNecessaria,
                80,
                "Alto"
            ),

            new Regra(
                "REGRA-008",
                "Informação confidencial",
                Decisao.RevisaoNecessaria,
                85,
                "Alto"
            )
        };
    }


    public ResultadoTriagem Analisar(Solicitacao solicitacao)
    {
        string conteudo = solicitacao.Conteudo?.ToLower().Trim() ?? "";

        List<Regra> regrasAtivadas = new List<Regra>();


        // ==========================================================
        // REGRA-001 — ENTRADA VAZIA
        // ==========================================================

        if (string.IsNullOrWhiteSpace(conteudo))
        {
            regrasAtivadas.Add(
                ObterRegra("REGRA-001")
            );
        }


        // ==========================================================
        // REGRA-002 — DADO SENSÍVEL
        // ==========================================================

        if (ContemAlgum(conteudo,
            "senha",
            "senhas",
            "token",
            "api key",
            "apikey",
            "api-key",
            "chave de api",
            "chave da api",
            "credencial",
            "credenciais",
            "código de acesso",
            "codigo de acesso"))
        {
            regrasAtivadas.Add(
                ObterRegra("REGRA-002")
            );
        }


        // ==========================================================
        // REGRA-003 — OPERAÇÃO DESTRUTIVA
        // ==========================================================

        if (ContemAlgum(conteudo,
            "excluir banco",
            "exclua banco",
            "exclua o banco",
            "apagar banco",
            "apague banco",
            "apague o banco",
            "deletar banco",
            "delete banco",
            "delete o banco",
            "excluir o banco",

            "excluir dados",
            "exclua dados",
            "exclua os dados",
            "apagar dados",
            "apague dados",
            "apague os dados",
            "deletar dados",
            "delete dados",
            "delete os dados",

            "excluir registros",
            "exclua registros",
            "exclua os registros",
            "apagar registros",
            "apague registros",
            "apague os registros",
            "deletar registros",
            "delete registros",
            "delete os registros",

            "remover arquivos",
            "remova arquivos",
            "remova os arquivos",
            "apagar arquivos",
            "apague arquivos",
            "excluir arquivos",
            "exclua arquivos",

            "apagar produção",
            "apague produção",
            "excluir produção",
            "exclua produção",
            "apagar producao",
            "apague producao",
            "excluir producao",
            "exclua producao"))
        {
            regrasAtivadas.Add(
                ObterRegra("REGRA-003")
            );
        }


        // ==========================================================
        // REGRA-004 — SOLICITAÇÃO MUITO CURTA
        // ==========================================================

        if (!string.IsNullOrWhiteSpace(conteudo) &&
            conteudo.Length < 20)
        {
            regrasAtivadas.Add(
                ObterRegra("REGRA-004")
            );
        }


        // ==========================================================
        // REGRA-006 — CÓDIGO MALICIOSO
        // ==========================================================

        if (ContemAlgum(conteudo,
            "malware",
            "ransomware",
            "vírus",
            "virus",
            "trojan",
            "keylogger",
            "código malicioso",
            "codigo malicioso"))
        {
            regrasAtivadas.Add(
                ObterRegra("REGRA-006")
            );
        }


        // ==========================================================
        // REGRA-007 — AMBIENTE DE PRODUÇÃO
        // ==========================================================

        if (ContemAlgum(conteudo,
            "produção",
            "producao",
            "ambiente de produção",
            "ambiente de producao",
            "servidor de produção",
            "servidor de producao",
            "sistema de produção",
            "sistema de producao",
            "banco de produção",
            "banco de producao"))
        {
            regrasAtivadas.Add(
                ObterRegra("REGRA-007")
            );
        }


        // ==========================================================
        // REGRA-008 — INFORMAÇÃO CONFIDENCIAL
        // ==========================================================

        if (ContemAlgum(conteudo,
            "confidencial",
            "informação confidencial",
            "informacao confidencial",
            "interno",
            "documento sigiloso",
            "informação privada",
            "informacao privada",
            "dado privado",
            "dados privados"))
        {
            regrasAtivadas.Add(
                ObterRegra("REGRA-008")
            );
        }


        // ==========================================================
        // REGRA-005 — PROGRAMAÇÃO COMUM
        // ==========================================================

        if (EhProgramacaoComum(conteudo) &&
            !regrasAtivadas.Any(r =>
                r.Decisao == Decisao.Bloqueado ||
                r.Decisao == Decisao.RevisaoNecessaria))
        {
            regrasAtivadas.Add(
                ObterRegra("REGRA-005")
            );
        }


        // ==========================================================
        // FALLBACK
        // ==========================================================

        if (regrasAtivadas.Count == 0)
        {
            return new ResultadoTriagem(
                "FALLBACK",
                Decisao.RevisaoNecessaria,
                "Médio",
                false,
                "O sistema não possui conhecimento suficiente " +
                "para classificar esta solicitação com segurança. " +
                "É necessária uma análise adicional.",
                new List<string>()
            );
        }


        // ==========================================================
        // RESOLUÇÃO DE CONFLITOS
        // ==========================================================

        Regra regraPrincipal = regrasAtivadas
            .OrderByDescending(r => r.Prioridade)
            .First();


        bool podeEnviar =
            regraPrincipal.Decisao == Decisao.Permitido;


        string justificativa =
            GerarJustificativa(regraPrincipal);


        return new ResultadoTriagem(
            regraPrincipal.Codigo,
            regraPrincipal.Decisao,
            regraPrincipal.NivelRisco,
            podeEnviar,
            justificativa,
            regrasAtivadas
                .Select(r => r.Codigo + " — " + r.Descricao)
                .ToList()
        );
    }


    // ==========================================================
    // MÉTODOS AUXILIARES
    // ==========================================================

    private Regra ObterRegra(string codigo)
    {
        return regras.First(r => r.Codigo == codigo);
    }


    private bool ContemAlgum(string texto, params string[] termos)
    {
        return termos.Any(termo => texto.Contains(termo));
    }


    private bool EhProgramacaoComum(string texto)
    {
        return ContemAlgum(texto,

            // Linguagens
            "c#",
            "csharp",
            "python",
            "java",
            "javascript",
            "typescript",
            "php",
            "ruby",
            "go",
            "c++",

            // Web
            "html",
            "css",
            "api",
            "api rest",
            "rest api",
            "asp.net",
            "aspnet",
            "asp.net core",
            "blazor",

            // C# / .NET
            "controller",
            "controllers",
            "service",
            "services",
            "classe",
            "classes",
            "método",
            "metodo",
            "função",
            "funcao",
            "interface",
            "interfaces",
            "variável",
            "variavel",

            // Programação geral
            "programação",
            "programacao",
            "código",
            "codigo",
            "algoritmo",
            "programar",
            "desenvolvimento",
            "desenvolver",
            "banco de dados",
            "sql"
        );
    }


    private string GerarJustificativa(Regra principal)
    {
        switch (principal.Codigo)
        {
            case "REGRA-001":
                return
                    "A solicitação está vazia ou contém apenas espaços. " +
                    "Não existem informações suficientes para análise.";

            case "REGRA-002":
                return
                    "Foi identificada informação potencialmente sensível, " +
                    "como senha, token, API Key ou credencial. " +
                    "A regra de bloqueio possui prioridade superior " +
                    "às regras de revisão.";

            case "REGRA-003":
                return
                    "Foi identificada uma operação potencialmente destrutiva. " +
                    "A solicitação exige revisão humana antes de qualquer envio.";

            case "REGRA-004":
                return
                    "A solicitação possui menos de 20 caracteres e " +
                    "pode não fornecer informações suficientes para " +
                    "uma análise segura.";

            case "REGRA-005":
                return
                    "A solicitação trata de um assunto comum de programação " +
                    "e não apresenta características de risco identificadas " +
                    "pelo sistema.";

            case "REGRA-006":
                return
                    "Foi identificada uma solicitação relacionada a código " +
                    "malicioso ou software potencialmente prejudicial. " +
                    "Por segurança, a solicitação foi bloqueada.";

            case "REGRA-007":
                return
                    "A solicitação envolve um ambiente de produção. " +
                    "Alterações nesse ambiente exigem revisão antes do envio.";

            case "REGRA-008":
                return
                    "Foi identificada possível informação confidencial ou privada. " +
                    "A solicitação deve passar por análise antes de ser encaminhada.";

            default:
                return
                    "A solicitação foi classificada pela regra de maior prioridade.";
        }
    }
}


namespace TriagemIA;

public class ResultadoTriagem
{
    public string CodigoRegra { get; set; }
    public Decisao Decisao { get; set; }
    public string NivelRisco { get; set; }
    public bool PodeEnviarParaIa { get; set; }
    public string Justificativa { get; set; }
    public List<string> RegrasAtivadas { get; set; }

    public ResultadoTriagem(
        string codigoRegra,
        Decisao decisao,
        string nivelRisco,
        bool podeEnviarParaIa,
        string justificativa,
        List<string> regrasAtivadas)
    {
        CodigoRegra = codigoRegra;
        Decisao = decisao;
        NivelRisco = nivelRisco;
        PodeEnviarParaIa = podeEnviarParaIa;
        Justificativa = justificativa;
        RegrasAtivadas = regrasAtivadas;
    }
}


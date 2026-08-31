using System.Runtime.CompilerServices;

namespace TriagemIA;

public class Solicitacao
{
    public string Conteudo {get;set;}

    public Solicitacao(string conteudo)
    {
        Conteudo = conteudo;
    }
}

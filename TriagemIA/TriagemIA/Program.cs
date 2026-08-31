
using TriagemIA;

Console.WriteLine("========================================");
Console.WriteLine("       SISTEMA ESPECIALISTA DE TRIAGEM");
Console.WriteLine("========================================");

Console.WriteLine("\nDigite a Solicitação:");

string texto = Console.ReadLine() ?? "";

Solicitacao solicitacao = new Solicitacao(texto);
Regras regras = new Regras();

ResultadoTriagem resultado = regras.Analisar(solicitacao);


Console.WriteLine("\n========================================");
Console.WriteLine("          RESULTADO DA ANÁLISE");
Console.WriteLine("========================================");

Console.WriteLine("\nSolicitação:");
Console.WriteLine($"\"{solicitacao.Conteudo}\"");


Console.WriteLine("\nRegras ativadas:");

if (resultado.RegrasAtivadas.Count == 0)
{
    Console.WriteLine("Nenhuma regra foi ativada.");
}
else
{
    foreach (string regra in resultado.RegrasAtivadas)
    {
        Console.WriteLine($"- {regra}");
    }
}


Console.WriteLine("\nRegra principal:");
Console.WriteLine(resultado.CodigoRegra);


Console.WriteLine("\nDecisão:");
Console.WriteLine(resultado.Decisao);


Console.WriteLine("\nNível de risco:");
Console.WriteLine(resultado.NivelRisco);


Console.WriteLine("\nPode enviar para IA:");
Console.WriteLine(
    resultado.PodeEnviarParaIa ? "SIM" : "NÃO"
);


Console.WriteLine("\nJustificativa:");
Console.WriteLine(resultado.Justificativa);


Console.WriteLine("\n========================================");
Console.WriteLine("             EXPLICAÇÃO");
Console.WriteLine("========================================");

Console.WriteLine(
    $"\nForam analisadas 8 regras do sistema."
);

if (resultado.RegrasAtivadas.Count > 1)
{
    Console.WriteLine(
        $"\nForam ativadas {resultado.RegrasAtivadas.Count} regras."
    );

    Console.WriteLine(
        "\nO sistema identificou um conflito entre regras " +
        "e utilizou a regra de maior prioridade."
    );
}
else if (resultado.RegrasAtivadas.Count == 1)
{
    Console.WriteLine(
        "\nApenas uma regra foi ativada."
    );
}
else
{
    Console.WriteLine(
        "\nNenhuma regra conhecida foi ativada."
    );
}


Console.WriteLine(
    $"\nRegra utilizada para a decisão: {resultado.CodigoRegra}"
);


Console.WriteLine(
    $"\nMotivo:\n{resultado.Justificativa}"
);


Console.WriteLine("\n========================================");

if (resultado.PodeEnviarParaIa)
{
    Console.WriteLine(
        "A solicitação PODE ser encaminhada " +
        "para a IA generativa."
    );
}
else
{
    Console.WriteLine(
        "A solicitação NÃO poderá ser encaminhada " +
        "para a IA generativa."
    );
}

Console.WriteLine("========================================");

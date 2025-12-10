using System;
using System.IO;

/// <summary>
/// Registra todas as ações importantes em log.txt.
/// </summary>
public static class Logger
{
    private static string caminho = "log.txt";

    /// <summary>
    /// Escreve uma linha no arquivo de log com data e hora.
    /// </summary>
    public static void Registrar(string mensagem)
    {
        using (StreamWriter sw = new StreamWriter(caminho, true))
        {
            sw.WriteLine($"[{DateTime.Now}] {mensagem}");
        }
    }
}

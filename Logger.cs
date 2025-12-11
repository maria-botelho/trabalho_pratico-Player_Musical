<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace trabalho_pratico
{
    internal class Logger
    {

        private static string caminho = "C:\\Users\\maria\\OneDrive\\Documentos\\Duda PUC\\AED\\trabalho-pratico\\log.txt";
        public static void Registrar(string mensagem)
        {
            using (StreamWriter sw = new StreamWriter(caminho, true))
            {
                sw.WriteLine($"[{DateTime.Now}] {mensagem}");
            }
=======
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
>>>>>>> 35f1d35c7a30c11a60748446dea87d496bfa1ade
        }
    }
}

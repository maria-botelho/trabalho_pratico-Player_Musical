using System;
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
        }
    }
}

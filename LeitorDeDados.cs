using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trabalho_pratico
{
    internal class LeitorDeDados
    {
        public static Dictionary<string, Musica> CarregarMusicas(string caminhoArquivo)
        {
            Dictionary<string, Musica> catalogo = new Dictionary<string, Musica>();

            try
            {
                using (StreamReader arqLeit = new StreamReader(caminhoArquivo, Encoding.UTF8))
                {
                    arqLeit.ReadLine();
                    string linha;

                    while ((linha = arqLeit.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(linha)) continue;

                        string[] campos = linha.Split(';');

                        if (campos.Length == 4)
                        {
                            if (int.TryParse(campos[3].Trim(), out int duracaoSegundos))
                            {
                                Musica novaMusica = new Musica(
                                    titulo: campos[0].Trim(),
                                    artista: campos[1].Trim(),
                                    genero: campos[2].Trim(),
                                    duracaoSegundos: duracaoSegundos
                                );

                                string chaveUnica = novaMusica.GerarChave();

                                if (!catalogo.ContainsKey(chaveUnica))
                                {
                                    catalogo.Add(chaveUnica, novaMusica);
                                }
                                else
                                {
                                    Console.WriteLine($"\n[AVISO] Música duplicada ignorada: {novaMusica.Titulo} - {novaMusica.Artista}");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\nOcorreu um erro ao processar o arquivo: {e.Message}");
            }
            return catalogo;
        }
    }

}


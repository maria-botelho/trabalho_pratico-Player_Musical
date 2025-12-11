<<<<<<< HEAD
﻿using System;
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
=======
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

/// <summary>
/// Classe responsável por carregar as músicas do arquivo de texto (CSV)
/// e montar o catálogo principal em um Dictionary (busca O(1)).
/// </summary>
public static class LeitorDeDados
{
    /// <summary>
    /// Lê o arquivo de músicas no caminho informado e devolve um dicionário
    /// onde a chave é Título+Artista (normalizados) e o valor é o objeto Musica.
    /// Ignora linhas vazias e músicas duplicadas.
    /// </summary>
    /// <param name="caminhoArquivo">Caminho completo do arquivo musicas.txt.</param>
    /// <returns>Dictionary com as músicas carregadas.</returns>
    public static Dictionary<string, Musica> CarregarMusicas(string caminhoArquivo)
    {
        Dictionary<string, Musica> catalogo = new Dictionary<string, Musica>();

        try
        {
            using (StreamReader arqLeit = new StreamReader(caminhoArquivo, Encoding.UTF8))
            {
                // Lê e descarta o cabeçalho (primeira linha).
                arqLeit.ReadLine();
                string linha;

                // Lê o arquivo linha a linha até o final.
                while ((linha = arqLeit.ReadLine()) != null)
                {
                    // Pula linhas vazias.
                    if (string.IsNullOrWhiteSpace(linha)) continue;

                    // Divide a linha em campos separados por ';'.
                    string[] campos = linha.Split(';');

                    // Cada linha válida deve ter exatamente 4 campos.
                    if (campos.Length == 4)
                    {
                        // Tenta converter a duração (campo 4) para int.
                        if (int.TryParse(campos[3].Trim(), out int duracaoSegundos))
                        {
                            // Cria o objeto Musica com os dados da linha.
                            Musica novaMusica = new Musica(
                                titulo: campos[0].Trim(),
                                artista: campos[1].Trim(),
                                genero: campos[2].Trim(),
                                duracaoSegundos: duracaoSegundos
                            );

                            // Gera chave única para evitar músicas duplicadas.
                            string chaveUnica = novaMusica.GerarChave();

                            // Só adiciona se ainda não existir no dicionário.
                            if (!catalogo.ContainsKey(chaveUnica))
                            {
                                catalogo.Add(chaveUnica, novaMusica);
                            }
                            else
                            {
                                Console.WriteLine($"\n[AVISO] Música duplicada ignorada: {novaMusica.Titulo} - {novaMusica.Artista}");
>>>>>>> 35f1d35c7a30c11a60748446dea87d496bfa1ade
                            }
                        }
                    }
                }
            }
<<<<<<< HEAD
            catch (Exception e)
            {
                Console.WriteLine($"\nOcorreu um erro ao processar o arquivo: {e.Message}");
            }
            return catalogo;
        }
    }

}

=======
        }
        catch (Exception e)
        {
            Console.WriteLine($"\nOcorreu um erro ao processar o arquivo: {e.Message}");
        }

        // Devolve o catálogo preenchido.
        return catalogo;
    }
}
>>>>>>> 35f1d35c7a30c11a60748446dea87d496bfa1ade

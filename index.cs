using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

// ====================================================================
// CLASSE: MUSICA (MODELO DE DADOS)
// ====================================================================
public class Musica
{
    public string Titulo { get; set; }
    public string Artista { get; set; }
    public string Genero { get; set; }
    public int DuracaoSegundos { get; set; }

    public Musica(string titulo, string artista, string genero, int duracaoSegundos)
    {
        this.Titulo = titulo;
        this.Artista = artista;
        this.Genero = genero;
        this.DuracaoSegundos = duracaoSegundos;
    }

    public Musica() { }

    public string GerarChave()
    {
        string chave = $"{this.Titulo.Trim().ToLower()}|{this.Artista.Trim().ToLower()}";
        return chave;
    }

    public string DuracaoFormatada
    {
        get
        {
            TimeSpan tempo = TimeSpan.FromSeconds(DuracaoSegundos);
            return $"{(int)tempo.TotalMinutes}:{tempo.Seconds:D2}";
        }
    }
}

// ====================================================================
// CLASSE: NÓ DA LISTA ENCADEADA (Estrutura Base Manual para Pilha e Fila)
// ====================================================================
public class NoMusica
{
    public Musica Dados { get; set; }
    public NoMusica Proximo { get; set; }

    public NoMusica(Musica musica)
    {
        Dados = musica;
        Proximo = null;
    }
}

// ====================================================================
// CLASSE: LEITOR DEDADOS (CARREGAMENTO DO CATÁLOGO O(1) com Dictionary<T>)
// ====================================================================
public static class LeitorDeDados
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

// ====================================================================
// CLASSE: HISTORICOREPRODUCAO (PILHA/STACK - LIFO MANUAL)
// ====================================================================
public class HistoricoReproducao
{
    private NoMusica topo;
    private int contagem;
    private const int CAPACIDADE_MAXIMA = 10;

    public HistoricoReproducao()
    {
        topo = null;
        contagem = 0;
    }

    public void Adicionar(Musica musica)
    {
        NoMusica novoNo = new NoMusica(musica);
        novoNo.Proximo = topo;
        topo = novoNo;
        contagem++;

        if (contagem > CAPACIDADE_MAXIMA)
        {
            NoMusica atual = topo;
            for (int i = 0; i < CAPACIDADE_MAXIMA - 1; i++)
            {
                if (atual.Proximo == null) break; 
                atual = atual.Proximo;
            }
            atual.Proximo = null; 
            contagem--;
        }
    }

    public Musica Voltar()
    {
        if (topo == null)
        {
            return null;
        }

        Musica musicaAnterior = topo.Dados;
        topo = topo.Proximo;
        contagem--;

        return musicaAnterior;
    }

    public int Contagem => contagem;

    public void ExibirHistorico()
    {
        Console.WriteLine("\n--- Histórico (Máximo 10 Músicas) ---");
        if (topo == null)
        {
            Console.WriteLine("Histórico vazio.");
            return;
        }
        NoMusica atual = topo;
        int i = 1;
        while (atual != null)
        {
            Console.WriteLine($"{i}. {atual.Dados.Titulo} - {atual.Dados.Artista} ({atual.Dados.DuracaoFormatada})");
            atual = atual.Proximo;
            i++;
        }
    }
}

// ====================================================================
// CLASSE: FILAREPRODUCAO (QUEUE/FILA - FIFO MANUAL)
// ====================================================================
public class FilaReproducao
{
    private NoMusica frente;
    private NoMusica traseira;
    private int contagem;

    public FilaReproducao()
    {
        frente = null;
        traseira = null;
        contagem = 0;
    }

    public void AdicionarMusica(Musica musica)
    {
        NoMusica novoNo = new NoMusica(musica);

        if (frente == null)
        {
            frente = novoNo;
            traseira = novoNo;
        }
        else
        {
            traseira.Proximo = novoNo;
            traseira = novoNo;
        }
        contagem++;
        Console.WriteLine($"\n[INFO] Adicionada à fila: {musica.Titulo}");
    }

    public Musica ProximaMusica()
    {
        if (frente == null)
        {
            return null;
        }

        Musica proxima = frente.Dados;
        frente = frente.Proximo;
        contagem--;

        if (frente == null)
        {
            traseira = null;
        }

        return proxima;
    }
    
    public void InsertInicio(Musica musica)
    {
        NoMusica novoNo = new NoMusica(musica);
        novoNo.Proximo = frente;
        frente = novoNo;
        contagem++;
        if (traseira == null)
        {
            traseira = novoNo;
        }
    }

    public int Contagem => contagem;
    
    public Musica PrimeiraMusica
    {
        get { return frente != null ? frente.Dados : null; }
    }

    public void ExibirFila()
    {
        Console.WriteLine("\n--- Fila de Reprodução (Próxima ao Topo) ---");
        if (frente == null)
        {
            Console.WriteLine("Fila vazia.");
            return;
        }
        NoMusica atual = frente;
        int i = 1;
        while (atual != null)
        {
            Console.WriteLine($"{i}. {atual.Dados.Titulo} - {atual.Dados.Artista} ({atual.Dados.DuracaoFormatada})");
            atual = atual.Proximo;
            i++;
        }
    }
}


// ====================================================================
// CLASSE PRINCIPAL: PROGRAM
// ====================================================================
public class Program
{
    public static void Main()
    {
        string caminhoArquivo = "C:\\Users\\maria\\OneDrive\\Documentos\\Duda PUC\\AED\\trabalho\\ConsoleApp1\\musicas.txt";

        Dictionary<string, Musica> catalogo = LeitorDeDados.CarregarMusicas(caminhoArquivo);
        HistoricoReproducao historico = new HistoricoReproducao();
        FilaReproducao fila = new FilaReproducao();
        Musica musicaAtual = null;

        if (catalogo.Count == 0)
        {
            Console.WriteLine("Catálogo não carregado. Encerrando.");
            return;
        }

        if (catalogo.Count >= 2)
        {
            fila.AdicionarMusica(catalogo.Values.ElementAt(0));
            fila.AdicionarMusica(catalogo.Values.ElementAt(1));
        }


        int op;
        do
        {
            Console.WriteLine("================ MENU REPRODUTOR ================");
            Console.WriteLine($"🎧 Tocando agora: {(musicaAtual != null ? musicaAtual.Titulo : "N/A")}");
            Console.WriteLine($"▶️ Próxima na Fila: {(fila.Contagem > 0 ? fila.PrimeiraMusica.Titulo : "Fila Vazia")}");
            Console.WriteLine($"◀️ Histórico: {historico.Contagem} músicas | Fila: {fila.Contagem} músicas");
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("0 - Sair");
            Console.WriteLine("1 - Listar Catálogo Completo");
            Console.WriteLine("2 - Pesquisar Música (Busca O(1))");
            Console.WriteLine("3 - Controle de Reprodução (Próxima/Voltar)");
            Console.WriteLine("4 - Gerenciar Fila e Histórico");
            Console.WriteLine("-------------------------------------------------");
            Console.Write("\nEscolha uma opção: ");

            if (!int.TryParse(Console.ReadLine(), out op))
            {
                op = -1;
            }

            Console.Clear();

            switch (op)
            {
                case 1:
                    Console.WriteLine("\n\n----------------------------- CATÁLOGO DE MÚSICAS ----------------------------");
                    Console.WriteLine($"{"TÍTULO",-35} | {"ARTISTA",-25} | {"GÊNERO",-15} | {"DURAÇÃO"}");
                    Console.WriteLine("--------------------------------------------------------------------------------------------------");

                    foreach (var musica in catalogo.Values)
                    {
                        Console.WriteLine($"{musica.Titulo,-35} | {musica.Artista,-25} | {musica.Genero,-15} | {musica.DuracaoFormatada,-7}");
                    }
                    Console.WriteLine("--------------------------------------------------------------------------------------------------");
                    Console.WriteLine($"Total de músicas únicas carregadas: {catalogo.Count}");
                    break;

                case 2:
                    string titulo, artista;
                    Console.WriteLine("\n--- Busca O(1) ---");
                    Console.Write("Qual música deseja pesquisar? ");
                    titulo = Console.ReadLine();
                    Console.Write("De qual artista? ");
                    artista = Console.ReadLine();

                    Musica musicaBuscada = RealizarBusca(catalogo, titulo, artista);

                    if (musicaBuscada != null)
                    {
                        Console.WriteLine("\nO que deseja fazer com esta música?");
                        Console.WriteLine("1 - Adicionar à Fila | 2 - Cancelar");
                        if (Console.ReadLine() == "1")
                        {
                            fila.AdicionarMusica(musicaBuscada);
                        }
                    }
                    break;

                case 3:
                    Console.WriteLine("\n--- Controle de Reprodução ---");
                    Console.WriteLine("1 - Próxima Música (Play/Skip)");
                    Console.WriteLine("2 - Voltar (Histórico)");
                    Console.Write("\nEscolha: ");

                    string controleOp = Console.ReadLine();

                    if (controleOp == "1")
                    {
                        Musica proxima = fila.ProximaMusica();
                        if (proxima != null)
                        {
                            if (musicaAtual != null)
                            {
                                historico.Adicionar(musicaAtual);
                            }
                            musicaAtual = proxima;
                            Console.WriteLine($"\n▶️ TOCANDO: {musicaAtual.Titulo} - {musicaAtual.Artista}");
                        }
                        else
                        {
                            Console.WriteLine("\nFila de reprodução vazia. Nada para tocar.");
                        }
                    }
                    else if (controleOp == "2")
                    {
                        Musica anterior = historico.Voltar();
                        if (anterior != null)
                        {
                            if (musicaAtual != null)
                            {
                                fila.InsertInicio(musicaAtual);
                            }
                            musicaAtual = anterior;
                            Console.WriteLine($"\n◀️ VOLTANDO: {musicaAtual.Titulo} - {musicaAtual.Artista}");
                        }
                        else
                        {
                            Console.WriteLine("\nHistórico de reprodução vazio. Não há como voltar.");
                        }
                    }
                    break;

                case 4:
                    Console.WriteLine("\n--- Gerenciar Fila e Histórico ---");
                    Console.WriteLine("1 - Ver Fila e Histórico");
                    Console.WriteLine("2 - Inserir Nova Música na Fila (Pesquisa)");
                    Console.Write("\nEscolha: ");

                    string gerenciaOp = Console.ReadLine();

                    if (gerenciaOp == "1")
                    {
                        fila.ExibirFila();
                        historico.ExibirHistorico();
                    }
                    else if (gerenciaOp == "2")
                    {
                        Console.Write("Título da música para adicionar: ");
                        string addTitulo = Console.ReadLine();
                        Console.Write("Artista da música para adicionar: ");
                        string addArtista = Console.ReadLine();

                        Musica musicaParaAdd = RealizarBusca(catalogo, addTitulo, addArtista);

                        if (musicaParaAdd != null)
                        {
                            fila.AdicionarMusica(musicaParaAdd);
                        }
                    }
                    break;
            }

            Console.WriteLine("\n\nClique ENTER para continuar...");
            Console.ReadLine();
            Console.Clear();

        } while (op != 0);
    }

    public static Musica RealizarBusca(Dictionary<string, Musica> catalogo, string titulo, string artista)
    {
        Musica musicaBusca = new Musica(titulo, artista, "", 0);
        string chaveBusca = musicaBusca.GerarChave();

        if (catalogo.TryGetValue(chaveBusca, out Musica encontrada))
        {
            Console.WriteLine("\n Música encontrada:");
            Console.WriteLine($"Título: {encontrada.Titulo} | Artista: {encontrada.Artista} | Gênero: {encontrada.Genero} | Duração: {encontrada.DuracaoFormatada} \n");
            return encontrada;
        }
        else
        {
            Console.WriteLine($" Não encontrada: {titulo} por {artista}.");
            Console.WriteLine("Verifique se a escrita está correta");
            return null;
        }
    }
}
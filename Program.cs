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
    // Propriedades auto-implementadas
    public string Titulo { get; set; }
    public string Artista { get; set; }
    public string Genero { get; set; }
    public int DuracaoSegundos { get; set; }

    // Construtor parametrizado
    public Musica(string titulo, string artista, string genero, int duracaoSegundos)
    {
        this.Titulo = titulo;
        this.Artista = artista;
        this.Genero = genero;
        this.DuracaoSegundos = duracaoSegundos;
    }

    // Construtor padrão
    public Musica() { }

    // Método para gerar a CHAVE ÚNICA (Título e Artista em minúsculas e sem espaços extras)
    public string GerarChave()
    {
        string chave = $"{this.Titulo.Trim().ToLower()}|{this.Artista.Trim().ToLower()}";
        return chave;
    }

    // Propriedade para formatar a duração em Minutos:Segundos
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
// CLASSE: LEITOR DEDADOS (CARREGAMENTO DO CATÁLOGO O(1))
// ====================================================================
public static class LeitorDeDados
{
    // Retorna o catálogo como um Dictionary<string, Musica> para busca O(1) e unicidade
    public static Dictionary<string, Musica> CarregarMusicas(string caminhoArquivo)
    {
        Dictionary<string, Musica> catalogo = new Dictionary<string, Musica>();

        try
        {
            using (StreamReader arqLeit = new StreamReader(caminhoArquivo, Encoding.UTF8))
            {
                arqLeit.ReadLine(); // Descarta o cabeçalho
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

                            // Tenta adicionar ao dicionário (verifica duplicidade O(1))
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
    private List<Musica> historico = new List<Musica>();
    private const int CAPACIDADE_MAXIMA = 10;

    // Adiciona a música tocada (topo da pilha) e mantém o limite de 10
    public void Adicionar(Musica musica)
    {
        historico.Insert(0, musica);

        if (historico.Count > CAPACIDADE_MAXIMA)
        {
            historico.RemoveAt(historico.Count - 1);
        }
    }

    // Retorna a música anterior (topo da pilha) e a remove
    public Musica Voltar()
    {
        if (historico.Count == 0)
        {
            return null;
        }

        Musica musicaAnterior = historico[0];
        historico.RemoveAt(0);

        return musicaAnterior;
    }

    public int Contagem => historico.Count;

    public void ExibirHistorico()
    {
        Console.WriteLine("\n--- Histórico (Máximo 10 Músicas) ---");
        if (historico.Count == 0)
        {
            Console.WriteLine("Histórico vazio.");
            return;
        }
        for (int i = 0; i < historico.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {historico[i].Titulo} - {historico[i].Artista} ({historico[i].DuracaoFormatada})");
        }
    }
}

// ====================================================================
// CLASSE: FILAREPRODUCAO (QUEUE/FILA - FIFO MANUAL)
// ====================================================================
public class FilaReproducao
{
    private List<Musica> fila = new List<Musica>();

    // Adiciona uma música ao final da fila (Enqueue)
    public void AdicionarMusica(Musica musica)
    {
        fila.Add(musica);
        Console.WriteLine($"\n[INFO] Adicionada à fila: {musica.Titulo}");
    }

    // Remove e retorna a próxima música a ser reproduzida (Dequeue)
    public Musica ProximaMusica()
    {
        if (fila.Count == 0)
        {
            return null;
        }

        Musica proxima = fila[0];
        fila.RemoveAt(0);

        return proxima;
    }

    public int Contagem => fila.Count;
    public List<Musica> FilaInterna => fila;

    public void ExibirFila()
    {
        Console.WriteLine("\n--- Fila de Reprodução (Próxima ao Topo) ---");
        if (fila.Count == 0)
        {
            Console.WriteLine("Fila vazia.");
            return;
        }
        for (int i = 0; i < fila.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {fila[i].Titulo} - {fila[i].Artista} ({fila[i].DuracaoFormatada})");
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
        // ⚠️ ATUALIZE O CAMINHO DO ARQUIVO AQUI:
        string caminhoArquivo = "C:\\Users\\maria\\OneDrive\\Documentos\\Duda PUC\\AED\\trabalho\\ConsoleApp1\\musicas.txt";

        // Inicialização das estruturas principais
        Dictionary<string, Musica> catalogo = LeitorDeDados.CarregarMusicas(caminhoArquivo);
        HistoricoReproducao historico = new HistoricoReproducao();
        FilaReproducao fila = new FilaReproducao();
        Musica musicaAtual = null;

        if (catalogo.Count == 0)
        {
            Console.WriteLine("Catálogo não carregado. Encerrando.");
            return;
        }

        // Simulação de carregamento inicial de músicas na fila
        if (catalogo.Count >= 2)
        {
            fila.AdicionarMusica(catalogo.Values.ElementAt(0));
            fila.AdicionarMusica(catalogo.Values.ElementAt(1));
        }


        int op;
        // Menu Principal
        do
        {
            Console.WriteLine("================ MENU REPRODUTOR ================");
            Console.WriteLine($"🎧 Tocando agora: {(musicaAtual != null ? musicaAtual.Titulo : "N/A")}");
            Console.WriteLine($"▶️ Próxima na Fila: {(fila.Contagem > 0 ? fila.FilaInterna[0].Titulo : "Fila Vazia")}");
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
                    // --- Listagem do Catálogo ---
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
                    // --- Pesquisa O(1) ---
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

                case 3: // Controle de Reprodução
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
                                fila.FilaInterna.Insert(0, musicaAtual);
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

                case 4: // Gerenciar Fila e Histórico
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
                    else if (gerenciaOp == "2") // ⭐️ NOVO: Inserção a partir do catálogo
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

    // Função auxiliar para demonstrar a busca O(1)
    public static Musica RealizarBusca(Dictionary<string, Musica> catalogo, string titulo, string artista)
    {
        Musica musicaBusca = new Musica(titulo, artista, "", 0);
        string chaveBusca = musicaBusca.GerarChave();

        if (catalogo.TryGetValue(chaveBusca, out Musica encontrada))
        {
            Console.WriteLine("\n✅ Música encontrada:");
            Console.WriteLine($"Título: {encontrada.Titulo} | Artista: {encontrada.Artista} | Gênero: {encontrada.Genero} | Duração: {encontrada.DuracaoFormatada} \n");
            return encontrada;
        }
        else
        {
            Console.WriteLine($"❌ Não encontrada: {titulo} por {artista}.");
            Console.WriteLine("Verifique se a escrita está correta");
            return null;
        }
    }
}
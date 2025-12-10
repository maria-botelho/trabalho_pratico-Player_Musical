using System;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Classe principal do sistema Player Musical.
/// Gerencia o catálogo, playlists, fila de reprodução, histórico e buscas.
/// </summary>
public class Program
{
    // ---------------------- CAMPOS GLOBAIS DO SISTEMA ----------------------

    /// <summary>
    /// Catálogo principal de músicas, indexado por chave única (Título+Artista).
    /// Proporciona busca O(1).
    /// </summary>
    private static Dictionary<string, Musica> catalogo;

    /// <summary>
    /// Índice por gênero usando árvore binária (busca O(log n)).
    /// </summary>
    private static ArvoreGenero indiceGeneros;

    /// <summary>
    /// Fila de reprodução (implementada manualmente).
    /// </summary>
    private static FilaReproducao fila;

    /// <summary>
    /// Histórico de músicas já tocadas (pilha implementada manualmente).
    /// </summary>
    private static HistoricoReproducao historico;

    /// <summary>
    /// Música que está sendo reproduzida no momento.
    /// </summary>
    private static Musica musicaAtual;

    /// <summary>
    /// Lista de playlists criadas pelo usuário.
    /// </summary>
    private static List<Playlist> playlists;

    /// <summary>
    /// Caminho do arquivo de músicas (CSV ou txt com separador ';').
    /// </summary>
    private const string CAMINHO_MUSICAS = "musicas.txt";

    // -------------------------- MÉTODO MAIN --------------------------

    /// <summary>
    /// Ponto de entrada do programa.
    /// Inicializa o sistema e exibe o menu principal em loop.
    /// </summary>
    public static void Main()
    {
        InicializarSistema();

        int opcao;
        do
        {
            ExibirCabecalho();

            Console.WriteLine("=============== MENU PRINCIPAL ===============");
            Console.WriteLine("0 - Sair");
            Console.WriteLine("1 - Gerenciar Catálogo");
            Console.WriteLine("2 - Gerenciar Playlists");
            Console.WriteLine("3 - Reprodução (Fila / Histórico)");
            Console.WriteLine("4 - Buscas por Gênero (O(log n) + Ordenação)");
            Console.WriteLine("==============================================");
            Console.Write("\nEscolha uma opção: ");

            if (!int.TryParse(Console.ReadLine(), out opcao))
                opcao = -1;

            Console.Clear();

            switch (opcao)
            {
                case 1:
                    MenuCatalogo();
                    break;
                case 2:
                    MenuPlaylists();
                    break;
                case 3:
                    MenuReproducao();
                    break;
                case 4:
                    MenuBuscaPorGenero();
                    break;
            }

        } while (opcao != 0);

        Logger.Registrar("Sistema encerrado.");
        Console.WriteLine("Encerrando o Player Musical...");
    }

    // -------------------------- INICIALIZAÇÃO --------------------------

    /// <summary>
    /// Carrega o catálogo a partir do arquivo,
    /// monta o índice por gênero, inicializa fila, histórico e lista de playlists.
    /// </summary>
    private static void InicializarSistema()
    {
        Console.WriteLine("Inicializando sistema...");

        catalogo = LeitorDeDados.CarregarMusicas(CAMINHO_MUSICAS);
        fila = new FilaReproducao();
        historico = new HistoricoReproducao();
        playlists = new List<Playlist>();
        musicaAtual = null;

        indiceGeneros = new ArvoreGenero();

        // Preenche o índice por gênero com base no catálogo.
        foreach (var musica in catalogo.Values)
        {
            indiceGeneros.Inserir(musica);
        }

        Logger.Registrar("Sistema iniciado. Catálogo carregado com " + catalogo.Count + " músicas.");
        Console.Clear();
    }

    /// <summary>
    /// Exibe o cabeçalho com estado atual: música tocando, próxima na fila etc.
    /// </summary>
    private static void ExibirCabecalho()
    {
        Console.WriteLine("=========== PLAYER MUSICAL (CONSOLE) ===========");
        Console.WriteLine($"🎧 Tocando agora: {(musicaAtual != null ? musicaAtual.Titulo + " - " + musicaAtual.Artista : "Nenhuma música tocando")}");
        Console.WriteLine($"▶️ Próxima na fila: {(fila.Contagem > 0 && fila.PrimeiraMusica != null ? fila.PrimeiraMusica.Titulo : "Nenhuma (fila vazia)")}");
        Console.WriteLine($"◀️ Histórico: {historico.Contagem} músicas | Fila: {fila.Contagem} músicas | Playlists: {playlists.Count}");
        Console.WriteLine("================================================\n");
    }

    // -------------------------- MENU CATÁLOGO --------------------------

    /// <summary>
    /// Menu para operações sobre o catálogo principal:
    /// listar, buscar, adicionar e remover músicas.
    /// </summary>
    private static void MenuCatalogo()
    {
        int opcao;
        do
        {
            Console.Clear();
            ExibirCabecalho();

            Console.WriteLine("----------- GERENCIAR CATÁLOGO -----------");
            Console.WriteLine("0 - Voltar");
            Console.WriteLine("1 - Listar todo o catálogo");
            Console.WriteLine("2 - Buscar música (Título + Artista) [O(1)]");
            Console.WriteLine("3 - Adicionar nova música ao catálogo");
            Console.WriteLine("4 - Remover música do catálogo");
            Console.WriteLine("------------------------------------------");
            Console.Write("\nEscolha uma opção: ");

            if (!int.TryParse(Console.ReadLine(), out opcao))
                opcao = -1;

            Console.Clear();
            ExibirCabecalho();

            switch (opcao)
            {
                case 1:
                    ListarCatalogo();
                    Pausa();
                    break;

                case 2:
                    BuscarMusicaCatalogo();
                    Pausa();
                    break;

                case 3:
                    AdicionarNovaMusica();
                    Pausa();
                    break;

                case 4:
                    RemoverMusicaCatalogo();
                    Pausa();
                    break;
            }

        } while (opcao != 0);
    }

    /// <summary>
    /// Lista todas as músicas do catálogo em formato tabular.
    /// </summary>
    private static void ListarCatalogo()
    {
        Console.WriteLine("\n----------------------------- CATÁLOGO COMPLETO -----------------------------");
        Console.WriteLine($"{"TÍTULO",-35} | {"ARTISTA",-25} | {"GÊNERO",-15} | {"DURAÇÃO"}");
        Console.WriteLine("----------------------------------------------------------------------------");

        foreach (var musica in catalogo.Values)
        {
            Console.WriteLine($"{musica.Titulo,-35} | {musica.Artista,-25} | {musica.Genero,-15} | {musica.DuracaoFormatada}");
        }

        Console.WriteLine("----------------------------------------------------------------------------");
        Console.WriteLine($"Total de músicas: {catalogo.Count}");
    }

    /// <summary>
    /// Fluxo de busca de música por Título + Artista e opção de adicionar à fila ou a uma playlist.
    /// </summary>
    private static void BuscarMusicaCatalogo()
    {
        Console.WriteLine("--- Busca O(1) no Catálogo ---");
        Console.Write("Título da música: ");
        string titulo = Console.ReadLine();
        Console.Write("Artista: ");
        string artista = Console.ReadLine();

        Musica encontrada = RealizarBusca(catalogo, titulo, artista);

        if (encontrada != null)
        {
            Console.WriteLine("\nO que deseja fazer com esta música?");
            Console.WriteLine("1 - Adicionar à Fila");
            Console.WriteLine("2 - Adicionar a uma Playlist");
            Console.WriteLine("0 - Nada");
            Console.Write("\nEscolha: ");

            string escolha = Console.ReadLine();

            if (escolha == "1")
            {
                fila.AdicionarMusica(encontrada);
                Logger.Registrar($"Música adicionada à fila: {encontrada.Titulo} - {encontrada.Artista}");
            }
            else if (escolha == "2")
            {
                if (playlists.Count == 0)
                {
                    Console.WriteLine("Nenhuma playlist criada ainda.");
                }
                else
                {
                    ExibirPlaylistsSimples();
                    Console.Write("\nDigite o número da playlist: ");
                    if (int.TryParse(Console.ReadLine(), out int idx) &&
                        idx >= 1 && idx <= playlists.Count)
                    {
                        playlists[idx - 1].AdicionarMusica(encontrada);
                        Logger.Registrar($"Música adicionada à playlist '{playlists[idx - 1].Nome}': {encontrada.Titulo} - {encontrada.Artista}");
                        Console.WriteLine("Música adicionada na playlist.");
                    }
                    else
                    {
                        Console.WriteLine("Playlist inválida.");
                    }
                }
            }
        }
    }

    /// <summary>
    /// Adiciona uma nova música ao catálogo e atualiza o arquivo de dados e o índice por gênero.
    /// </summary>
    private static void AdicionarNovaMusica()
    {
        Console.WriteLine("--- Adicionar Nova Música ao Catálogo ---");

        Console.Write("Título: ");
        string titulo = Console.ReadLine();

        Console.Write("Artista: ");
        string artista = Console.ReadLine();

        Console.Write("Gênero: ");
        string genero = Console.ReadLine();

        Console.Write("Duração (em segundos): ");
        if (!int.TryParse(Console.ReadLine(), out int duracao))
        {
            Console.WriteLine("Duração inválida. Operação cancelada.");
            return;
        }

        Musica nova = new Musica(titulo, artista, genero, duracao);
        string chave = nova.GerarChave();

        if (catalogo.ContainsKey(chave))
        {
            Console.WriteLine("Já existe uma música com esse título e artista no catálogo.");
            return;
        }

        // Adiciona no catálogo em memória.
        catalogo.Add(chave, nova);
        indiceGeneros.Inserir(nova);

        // Atualiza arquivo (append).
        try
        {
            using (StreamWriter sw = new StreamWriter(CAMINHO_MUSICAS, true))
            {
                sw.WriteLine($"{nova.Titulo};{nova.Artista};{nova.Genero};{nova.DuracaoSegundos}");
            }
            Console.WriteLine("Música adicionada ao catálogo e registrada no arquivo.");
            Logger.Registrar($"Nova música adicionada ao catálogo: {nova.Titulo} - {nova.Artista}");
        }
        catch (Exception e)
        {
            Console.WriteLine("Falha ao escrever no arquivo de músicas: " + e.Message);
        }
    }

    /// <summary>
    /// Remove uma música do catálogo (apenas em memória).
    /// </summary>
    private static void RemoverMusicaCatalogo()
    {
        Console.WriteLine("--- Remover Música do Catálogo ---");

        Console.Write("Título: ");
        string titulo = Console.ReadLine();

        Console.Write("Artista: ");
        string artista = Console.ReadLine();

        Musica musica = RealizarBusca(catalogo, titulo, artista);

        if (musica == null)
        {
            Console.WriteLine("Música não encontrada, nada a remover.");
            return;
        }

        string chave = musica.GerarChave();
        bool removida = catalogo.Remove(chave);

        if (removida)
        {
            Console.WriteLine("Música removida do catálogo em memória.");
            Logger.Registrar($"Música removida do catálogo: {musica.Titulo} - {musica.Artista}");
            // Obs.: para simplificar, não reescrevemos o arquivo de músicas ao remover.
        }
    }

    // -------------------------- MENU PLAYLISTS --------------------------

    /// <summary>
    /// Menu de gerenciamento de playlists: criar, listar e gerenciar individualmente.
    /// </summary>
    private static void MenuPlaylists()
    {
        int opcao;
        do
        {
            Console.Clear();
            ExibirCabecalho();

            Console.WriteLine("------------- GERENCIAR PLAYLISTS -------------");
            Console.WriteLine("0 - Voltar");
            Console.WriteLine("1 - Criar nova playlist");
            Console.WriteLine("2 - Listar playlists");
            Console.WriteLine("3 - Gerenciar uma playlist");
            Console.WriteLine("----------------------------------------------");
            Console.Write("\nEscolha uma opção: ");

            if (!int.TryParse(Console.ReadLine(), out opcao))
                opcao = -1;

            Console.Clear();
            ExibirCabecalho();

            switch (opcao)
            {
                case 1:
                    CriarPlaylist();
                    Pausa();
                    break;

                case 2:
                    ListarPlaylistsDetalhes();
                    Pausa();
                    break;

                case 3:
                    GerenciarPlaylist();
                    break;
            }

        } while (opcao != 0);
    }

    /// <summary>
    /// Cria uma nova playlist e adiciona à lista de playlists.
    /// </summary>
    private static void CriarPlaylist()
    {
        Console.Write("Nome da nova playlist: ");
        string nome = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(nome))
        {
            Console.WriteLine("Nome inválido.");
            return;
        }

        Playlist p = new Playlist(nome);
        playlists.Add(p);

        Logger.Registrar($"Playlist criada: {nome}");
        Console.WriteLine("Playlist criada com sucesso.");
    }

    /// <summary>
    /// Lista todas as playlists com detalhes.
    /// </summary>
    private static void ListarPlaylistsDetalhes()
    {
        if (playlists.Count == 0)
        {
            Console.WriteLine("Nenhuma playlist criada.");
            return;
        }

        Console.WriteLine("--- Playlists ---");
        for (int i = 0; i < playlists.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {playlists[i].Nome} (Músicas: {playlists[i].Musicas.Tamanho})");
        }
    }

    /// <summary>
    /// Exibe apenas os nomes das playlists (para seleção rápida).
    /// </summary>
    private static void ExibirPlaylistsSimples()
    {
        Console.WriteLine("\nPlaylists disponíveis:");
        for (int i = 0; i < playlists.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {playlists[i].Nome}");
        }
    }

    /// <summary>
    /// Permite o usuário escolher uma playlist e, em seguida, abre o menu de gerenciamento da mesma.
    /// </summary>
    private static void GerenciarPlaylist()
    {
        if (playlists.Count == 0)
        {
            Console.WriteLine("Nenhuma playlist criada.");
            Pausa();
            return;
        }

        ExibirPlaylistsSimples();
        Console.Write("\nDigite o número da playlist: ");

        if (int.TryParse(Console.ReadLine(), out int idx) &&
            idx >= 1 && idx <= playlists.Count)
        {
            Playlist selecionada = playlists[idx - 1];
            MenuPlaylistIndividual(selecionada);
        }
        else
        {
            Console.WriteLine("Playlist inválida.");
            Pausa();
        }
    }

    /// <summary>
    /// Menu de operações sobre uma playlist específica:
    /// exibir, adicionar músicas, remover, enviar para fila.
    /// </summary>
    private static void MenuPlaylistIndividual(Playlist playlist)
    {
        int opcao;
        do
        {
            Console.Clear();
            ExibirCabecalho();

            Console.WriteLine($"------ PLAYLIST: {playlist.Nome} ------");
            Console.WriteLine("0 - Voltar");
            Console.WriteLine("1 - Exibir músicas");
            Console.WriteLine("2 - Adicionar música do catálogo");
            Console.WriteLine("3 - Remover música da playlist");
            Console.WriteLine("4 - Enviar playlist para fila de reprodução");
            Console.WriteLine("----------------------------------------");
            Console.Write("\nEscolha uma opção: ");

            if (!int.TryParse(Console.ReadLine(), out opcao))
                opcao = -1;

            Console.Clear();
            ExibirCabecalho();

            switch (opcao)
            {
                case 1:
                    playlist.ExibirPlaylist();
                    Pausa();
                    break;

                case 2:
                    AdicionarMusicaPlaylist(playlist);
                    Pausa();
                    break;

                case 3:
                    RemoverMusicaPlaylist(playlist);
                    Pausa();
                    break;

                case 4:
                    EnviarPlaylistParaFila(playlist);
                    Pausa();
                    break;
            }

        } while (opcao != 0);
    }

    /// <summary>
    /// Adiciona uma música do catálogo à playlist, usando busca O(1).
    /// </summary>
    private static void AdicionarMusicaPlaylist(Playlist playlist)
    {
        Console.WriteLine("--- Adicionar Música do Catálogo à Playlist ---");
        Console.Write("Título: ");
        string titulo = Console.ReadLine();
        Console.Write("Artista: ");
        string artista = Console.ReadLine();

        Musica musica = RealizarBusca(catalogo, titulo, artista);

        if (musica != null)
        {
            playlist.AdicionarMusica(musica);
            Logger.Registrar($"Música adicionada à playlist '{playlist.Nome}': {musica.Titulo} - {musica.Artista}");
            Console.WriteLine("Música adicionada à playlist.");
        }
    }

    /// <summary>
    /// Remove música de uma playlist pelo título e artista.
    /// </summary>
    private static void RemoverMusicaPlaylist(Playlist playlist)
    {
        Console.WriteLine("--- Remover Música da Playlist ---");
        Console.Write("Título: ");
        string titulo = Console.ReadLine();
        Console.Write("Artista: ");
        string artista = Console.ReadLine();

        bool removida = playlist.RemoverMusica(titulo, artista);

        if (removida)
        {
            Logger.Registrar($"Música removida da playlist '{playlist.Nome}': {titulo} - {artista}");
            Console.WriteLine("Música removida da playlist.");
        }
        else
        {
            Console.WriteLine("Música não encontrada na playlist.");
        }
    }

    /// <summary>
    /// Envia todas as músicas da playlist para a fila de reprodução, na ordem atual.
    /// </summary>
    private static void EnviarPlaylistParaFila(Playlist playlist)
    {
        NoDuplo atual = playlist.Musicas.Primeiro;

        if (atual == null)
        {
            Console.WriteLine("Playlist vazia.");
            return;
        }

        while (atual != null)
        {
            fila.AdicionarMusica(atual.Dados);
            Logger.Registrar($"Música da playlist '{playlist.Nome}' enviada para fila: {atual.Dados.Titulo} - {atual.Dados.Artista}");
            atual = atual.Proximo;
        }

        Console.WriteLine("Todas as músicas da playlist foram adicionadas à fila.");
    }

    // -------------------------- MENU REPRODUÇÃO --------------------------

    /// <summary>
    /// Menu para controlar reprodução: fila, histórico e avançar/voltar.
    /// </summary>
    private static void MenuReproducao()
    {
        int opcao;
        do
        {
            Console.Clear();
            ExibirCabecalho();

            Console.WriteLine("--------- CONTROLE DE REPRODUÇÃO ---------");
            Console.WriteLine("0 - Voltar");
            Console.WriteLine("1 - Mostrar fila de reprodução");
            Console.WriteLine("2 - Mostrar histórico");
            Console.WriteLine("3 - Próxima música (Play / Skip)");
            Console.WriteLine("4 - Voltar uma música (Histórico)");
            Console.WriteLine("------------------------------------------");
            Console.Write("\nEscolha uma opção: ");

            if (!int.TryParse(Console.ReadLine(), out opcao))
                opcao = -1;

            Console.Clear();
            ExibirCabecalho();

            switch (opcao)
            {
                case 1:
                    fila.ExibirFila();
                    Pausa();
                    break;

                case 2:
                    historico.ExibirHistorico();
                    Pausa();
                    break;

                case 3:
                    TocarProximaMusica();
                    Pausa();
                    break;

                case 4:
                    VoltarMusica();
                    Pausa();
                    break;
            }

        } while (opcao != 0);
    }

    /// <summary>
    /// Avança para a próxima música da fila, enviando a atual para o histórico.
    /// </summary>
    private static void TocarProximaMusica()
    {
        Musica proxima = fila.ProximaMusica();

        if (proxima == null)
        {
            Console.WriteLine("Fila vazia. Nada para tocar.");
            return;
        }

        if (musicaAtual != null)
        {
            historico.Adicionar(musicaAtual);
        }

        musicaAtual = proxima;

        Console.WriteLine($"▶️ TOCANDO: {musicaAtual.Titulo} - {musicaAtual.Artista}");
        Logger.Registrar($"Play: {musicaAtual.Titulo} - {musicaAtual.Artista}");
    }

    /// <summary>
    /// Volta uma música no histórico. A música atual volta para o início da fila.
    /// </summary>
    private static void VoltarMusica()
    {
        Musica anterior = historico.Voltar();

        if (anterior == null)
        {
            Console.WriteLine("Histórico vazio. Não há música para voltar.");
            return;
        }

        if (musicaAtual != null)
        {
            fila.InsertInicio(musicaAtual);
        }

        musicaAtual = anterior;

        Console.WriteLine($"◀️ VOLTANDO: {musicaAtual.Titulo} - {musicaAtual.Artista}");
        Logger.Registrar($"Voltar: {musicaAtual.Titulo} - {musicaAtual.Artista}");
    }

    // -------------------------- MENU BUSCA POR GÊNERO --------------------------

    /// <summary>
    /// Menu para busca por gênero (O(log n)) com opção de ordenar por título ou duração.
    /// </summary>
    private static void MenuBuscaPorGenero()
    {
        int opcao;
        do
        {
            Console.Clear();
            ExibirCabecalho();

            Console.WriteLine("--------- BUSCA POR GÊNERO (O(log n)) ---------");
            Console.WriteLine("0 - Voltar");
            Console.WriteLine("1 - Buscar músicas por gênero e exibir (com ordenação)");
            Console.WriteLine("-----------------------------------------------");
            Console.Write("\nEscolha uma opção: ");

            if (!int.TryParse(Console.ReadLine(), out opcao))
                opcao = -1;

            Console.Clear();
            ExibirCabecalho();

            if (opcao == 1)
            {
                BuscarPorGenero();
                Pausa();
            }

        } while (opcao != 0);
    }

    /// <summary>
    /// Executa a busca na árvore de gêneros e permite escolher como ordenar a saída.
    /// </summary>
    private static void BuscarPorGenero()
    {
        Console.Write("Digite o gênero desejado: ");
        string genero = Console.ReadLine();

        List<Musica> lista = indiceGeneros.BuscarGenero(genero);

        if (lista == null || lista.Count == 0)
        {
            Console.WriteLine("Nenhuma música encontrada para esse gênero.");
            return;
        }

        Console.WriteLine($"\nForam encontradas {lista.Count} músicas do gênero '{genero}'.");
        Console.WriteLine("Deseja ordenar a lista?");
        Console.WriteLine("1 - Por título (alfabético)");
        Console.WriteLine("2 - Por duração (da menor para a maior)");
        Console.WriteLine("0 - Sem ordenação");
        Console.Write("\nEscolha: ");

        string escolha = Console.ReadLine();
        List<Musica> listaOrdenada = new List<Musica>(lista);

        if (escolha == "1")
        {
            listaOrdenada = Ordenacao.OrdenarPorTitulo(listaOrdenada);
        }
        else if (escolha == "2")
        {
            listaOrdenada = Ordenacao.OrdenarPorDuracao(listaOrdenada);
        }

        Console.WriteLine($"\n--- MÚSICAS DO GÊNERO: {genero.ToUpper()} ---");
        foreach (var musica in listaOrdenada)
        {
            Console.WriteLine($"{musica.Titulo} - {musica.Artista} ({musica.DuracaoFormatada})");
        }

        Logger.Registrar($"Busca por gênero realizada: {genero}, músicas retornadas: {listaOrdenada.Count}");
    }

    // -------------------------- MÉTODO DE BUSCA O(1) --------------------------

    /// <summary>
    /// Realiza a busca de uma música no catálogo usando a chave Título+Artista.
    /// A busca é O(1) pois utiliza Dictionary.
    /// </summary>
    public static Musica RealizarBusca(Dictionary<string, Musica> catalogo, string titulo, string artista)
    {
        Musica musicaBusca = new Musica(titulo, artista, "", 0);
        string chaveBusca = musicaBusca.GerarChave();

        if (catalogo.TryGetValue(chaveBusca, out Musica encontrada))
        {
            Console.WriteLine("\nMúsica encontrada:");
            Console.WriteLine($"Título: {encontrada.Titulo} | Artista: {encontrada.Artista} | Gênero: {encontrada.Genero} | Duração: {encontrada.DuracaoFormatada}\n");
            return encontrada;
        }
        else
        {
            Console.WriteLine($"\nNão encontrada: {titulo} por {artista}.");
            Console.WriteLine("Verifique se a escrita está correta.");
            return null;
        }
    }

    // -------------------------- UTILITÁRIOS --------------------------

    /// <summary>
    /// Só para pausar a tela entre as operações.
    /// </summary>
    private static void Pausa()
    {
        Console.WriteLine("\nPressione ENTER para continuar...");
        Console.ReadLine();
    }
}

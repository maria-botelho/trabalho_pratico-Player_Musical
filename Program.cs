using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using trabalho_pratico;

public class Program
{
    private static Dictionary<string, Musica> catalogo;
    private static ArvoreGenero indiceGeneros;
    private static FilaReproducao fila;
    private static HistoricoReproducao historico;
    private static Musica musicaAtual;
    private static List<Playlist> playlists;
    private const string CAMINHO_MUSICAS = "C:\\Users\\maria\\OneDrive\\Documentos\\Duda PUC\\AED\\trabalho-pratico\\musicas.txt";

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
            Console.WriteLine("4 - Buscas por Gênero");
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

    private static void InicializarSistema()
    {
        Console.WriteLine("Inicializando sistema...");

        catalogo = LeitorDeDados.CarregarMusicas(CAMINHO_MUSICAS);
        fila = new FilaReproducao();
        historico = new HistoricoReproducao();
        playlists = new List<Playlist>();
        musicaAtual = null;

        indiceGeneros = new ArvoreGenero();

        foreach (var musica in catalogo.Values)
        {
            indiceGeneros.Inserir(musica);
        }

        Logger.Registrar("Sistema iniciado. Catálogo carregado com " + catalogo.Count + " músicas.");
        Console.Clear();
    }

    private static void ExibirCabecalho()
    {
        Console.WriteLine("=========== PLAYER MUSICAL ===========");
        Console.WriteLine($"Tocando agora: {(musicaAtual != null ? musicaAtual.Titulo + " - " + musicaAtual.Artista : "Nenhuma música tocando")}");
        Console.WriteLine($"Próxima na fila: {(fila.Contagem > 0 && fila.PrimeiraMusica != null ? fila.PrimeiraMusica.Titulo : "Nenhuma (fila vazia)")}");
        Console.WriteLine($"Histórico: {historico.Contagem} músicas | Fila: {fila.Contagem} músicas | Playlists: {playlists.Count}");
        Console.WriteLine("================================================\n");
    }

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
            Console.WriteLine("2 - Buscar música (Título + Artista)");
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

    private static Musica RealizarBusca(Dictionary<string, Musica> dict, string titulo, string artista)
    {
        string chave = $"{titulo.Trim().ToLower()}|{artista.Trim().ToLower()}";

        if (dict.TryGetValue(chave, out Musica encontrada))
        {
            Console.WriteLine("\n--- MÚSICA ENCONTRADA ---");
            Console.WriteLine($"Título: {encontrada.Titulo}");
            Console.WriteLine($"Artista: {encontrada.Artista}");
            Console.WriteLine($"Gênero: {encontrada.Genero}");
            Console.WriteLine($"Duração: {encontrada.DuracaoFormatada}");
            Console.WriteLine("--------------------------");
            return encontrada;
        }

        Console.WriteLine("\nMúsica não encontrada no catálogo.");
        return null;
    }

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

        catalogo.Add(chave, nova);
        indiceGeneros.Inserir(nova);

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
        }
    }

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

    private static void ExibirPlaylistsSimples()
    {
        Console.WriteLine("\nPlaylists disponíveis:");
        for (int i = 0; i < playlists.Count; i++)
        {
            Console.WriteLine($"{i + 1} - {playlists[i].Nome}");
        }
    }

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
            Console.WriteLine("Música não encontrada nesta playlist.");
        }
    }

    private static void EnviarPlaylistParaFila(Playlist playlist)
    {
        NoDuplo atual = playlist.Musicas.Primeiro;
        int count = 0;

        while (atual != null)
        {
            fila.AdicionarMusica(atual.Dados);
            atual = atual.Proximo;
            count++;
        }

        Logger.Registrar($"Playlist '{playlist.Nome}' ({count} músicas) enviada para a fila de reprodução.");
        Console.WriteLine($"\nA playlist '{playlist.Nome}' ({count} músicas) foi adicionada ao final da fila.");
    }

    private static void MenuReproducao()
    {
        int opcao;
        do
        {
            Console.Clear();
            ExibirCabecalho();

            Console.WriteLine("------------- CONTROLE DE REPRODUÇÃO -------------");
            Console.WriteLine("0 - Voltar");
            Console.WriteLine("1 - Tocar Próxima Música");
            Console.WriteLine("2 - Voltar Música ");
            Console.WriteLine("3 - Exibir Fila e Histórico");
            Console.WriteLine("--------------------------------------------------");
            Console.Write("\nEscolha uma opção: ");

            if (!int.TryParse(Console.ReadLine(), out opcao))
                opcao = -1;

            Console.Clear();
            ExibirCabecalho();

            switch (opcao)
            {
                case 1:
                    TocarProxima();
                    Pausa();
                    break;
                case 2:
                    VoltarMusica();
                    Pausa();
                    break;
                case 3:
                    fila.ExibirFila();
                    historico.ExibirHistorico();
                    Pausa();
                    break;
            }

        } while (opcao != 0);
    }

    private static void TocarProxima()
    {
        Musica proxima = fila.ProximaMusica();

        if (proxima != null)
        {
            if (musicaAtual != null)
            {
                historico.Adicionar(musicaAtual);
            }

            musicaAtual = proxima;
            Console.WriteLine($"\nTOCANDO AGORA: {musicaAtual.Titulo} - {musicaAtual.Artista} ({musicaAtual.DuracaoFormatada})");
            Logger.Registrar($"Música tocada: {musicaAtual.Titulo} - {musicaAtual.Artista}");
        }
        else
        {
            Console.WriteLine("\nFila de reprodução vazia. Nada para tocar.");
            musicaAtual = null;
        }
    }

    private static void VoltarMusica()
    {
        Musica anterior = historico.Voltar();

        if (anterior != null)
        {
            if (musicaAtual != null)
            {
                fila.InsertInicio(musicaAtual);
            }

            musicaAtual = anterior;
            Console.WriteLine($"\nVOLTANDO: {musicaAtual.Titulo} - {musicaAtual.Artista} ({musicaAtual.DuracaoFormatada})");
            Logger.Registrar($"Música voltou (Histórico -> Fila): {musicaAtual.Titulo}");

        }
        else
        {
            Console.WriteLine("\nHistórico de reprodução vazio. Não há como voltar.");
        }
    }

    private static void MenuBuscaPorGenero()
    {
        Console.WriteLine("--- Busca por Gênero ---");
        Console.Write("Digite o Gênero para buscar: ");
        string genero = Console.ReadLine().Trim();

        Logger.Registrar($"Busca por Gênero: {genero}");

        List<Musica> musicasDoGenero = indiceGeneros.BuscarGenero(genero);

        if (musicasDoGenero != null && musicasDoGenero.Any())
        {
            musicasDoGenero = musicasDoGenero.OrderBy(m => m.Titulo).ToList();
            Console.WriteLine("\n Resultado ordenado por Título.");

            Console.WriteLine($"\n--- RESULTADO DA BUSCA: {genero} ({musicasDoGenero.Count} Músicas) ---");
            foreach (var musica in musicasDoGenero)
            {
                Console.WriteLine($"{musica.Titulo,-35} | {musica.Artista,-25} | {musica.DuracaoFormatada}");
            }
            Console.WriteLine("--------------------------------------------------------------------");

            Console.Write("Deseja adicionar todas à Fila? (S/N): ");
            if (Console.ReadLine().Trim().ToUpper() == "S")
            {
                foreach (var musica in musicasDoGenero)
                {
                    fila.AdicionarMusica(musica);
                }
                Logger.Registrar($"Todas as {musicasDoGenero.Count} músicas de {genero} adicionadas à Fila.");
                Console.WriteLine($"\nTodas as músicas de {genero} adicionadas à fila.");
            }
        }
        else
        {
            Console.WriteLine($"\nNenhuma música encontrada para o gênero '{genero}'.");
        }

        Pausa();
    }

    private static void Pausa()
    {
        Console.Write("\nPressione ENTER para continuar...");
        Console.ReadLine();
    }
}
using System;

/// <summary>
/// Implementação manual de uma FILA (queue) de reprodução.
/// Funciona em FIFO (First In, First Out): a primeira música que entra é a primeira que toca.
/// Essa estrutura será usada para controlar a ordem real de reprodução das músicas.
/// </summary>
public class FilaReproducao
{
    /// <summary>
    /// Referência para o primeiro nó da fila (quem será tocado primeiro).
    /// </summary>
    private NoMusica frente;

    /// <summary>
    /// Referência para o último nó da fila (última música adicionada).
    /// </summary>
    private NoMusica traseira;

    /// <summary>
    /// Quantidade de músicas atualmente na fila.
    /// </summary>
    private int contagem;

    /// <summary>
    /// Construtor: inicializa uma fila vazia.
    /// </summary>
    public FilaReproducao()
    {
        frente = null;
        traseira = null;
        contagem = 0;
    }

    /// <summary>
    /// Adiciona uma música no final da fila de reprodução.
    /// </summary>
    /// <param name="musica">Música a ser adicionada.</param>
    public void AdicionarMusica(Musica musica)
    {
        NoMusica novoNo = new NoMusica(musica);

        // Caso a fila esteja vazia, o primeiro e o último nó são o mesmo.
        if (frente == null)
        {
            frente = novoNo;
            traseira = novoNo;
        }
        else
        {
            // Amarra o último nó ao novo nó.
            traseira.Proximo = novoNo;
            traseira = novoNo;
        }

        contagem++;
        Console.WriteLine($"\n[INFO] Adicionada à fila: {musica.Titulo}");
    }

    /// <summary>
    /// Remove a próxima música da fila e retorna para ser tocada.
    /// Se a fila estiver vazia, retorna null.
    /// </summary>
    public Musica ProximaMusica()
    {
        if (frente == null)
        {
            return null;
        }

        Musica proxima = frente.Dados;

        // Avança a fila.
        frente = frente.Proximo;
        contagem--;

        // Se a fila ficou vazia, precisa ajustar a traseira também.
        if (frente == null)
        {
            traseira = null;
        }

        return proxima;
    }

    /// <summary>
    /// Insere uma música no INÍCIO da fila.
    /// Usado quando o usuário aperta "voltar" no histórico.
    /// </summary>
    /// <param name="musica">Música que deve voltar para tocar antes das outras.</param>
    public void InsertInicio(Musica musica)
    {
        NoMusica novoNo = new NoMusica(musica);
        novoNo.Proximo = frente;
        frente = novoNo;
        contagem++;

        // Se a fila estava vazia, traseira precisa apontar também para o novo nó.
        if (traseira == null)
        {
            traseira = novoNo;
        }
    }

    /// <summary>
    /// Propriedade somente leitura: quantidade de músicas na fila.
    /// </summary>
    public int Contagem => contagem;

    /// <summary>
    /// Mostra a primeira música da fila (sem removê-la).
    /// </summary>
    public Musica PrimeiraMusica
    {
        get { return frente != null ? frente.Dados : null; }
    }

    /// <summary>
    /// Exibe todas as músicas da fila na ordem de execução.
    /// </summary>
    public void ExibirFila()
    {
        Console.WriteLine("\n--- Fila de Reprodução (Ordem de Execução) ---");

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

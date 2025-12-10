using System;

/// <summary>
/// Representa uma playlist, contendo um nome e uma lista duplamente encadeada.
/// </summary>
public class Playlist
{
    public string Nome { get; set; }
    public ListaDuplamenteEncadeada Musicas { get; private set; }

    public Playlist(string nome)
    {
        Nome = nome;
        Musicas = new ListaDuplamenteEncadeada();
    }

    public void AdicionarMusica(Musica musica)
    {
        Musicas.InserirFim(musica);
    }

    public bool RemoverMusica(string titulo, string artista)
    {
        return Musicas.Remover(titulo, artista);
    }

    public Musica Buscar(string titulo, string artista)
    {
        return Musicas.Buscar(titulo, artista);
    }

    public void ExibirPlaylist()
    {
        Console.WriteLine($"\nPlaylist: {Nome}");
        Musicas.Exibir();
    }
}

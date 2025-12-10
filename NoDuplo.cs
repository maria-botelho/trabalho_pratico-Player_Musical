/// <summary>
/// Nó da lista duplamente encadeada.
/// Usado para armazenar músicas dentro das Playlists.
/// Cada nó aponta para o próximo e para o anterior.
/// </summary>
public class NoDuplo
{
    public Musica Dados { get; set; }
    public NoDuplo Proximo { get; set; }
    public NoDuplo Anterior { get; set; }

    public NoDuplo(Musica musica)
    {
        Dados = musica;
        Proximo = null;
        Anterior = null;
    }
}

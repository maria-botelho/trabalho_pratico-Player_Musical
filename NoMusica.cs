/// <summary>
/// Nó de lista encadeada que armazena uma música.
/// Essa estrutura é usada como base para PILHA (Histórico) e FILA (Fila de Reprodução).
/// </summary>
public class NoMusica
{
    /// <summary>
    /// Música armazenada neste nó.
    /// </summary>
    public Musica Dados { get; set; }

    /// <summary>
    /// Referência para o próximo nó da lista.
    /// </summary>
    public NoMusica Proximo { get; set; }

    /// <summary>
    /// Construtor que cria um nó contendo a música informada.
    /// </summary>
    public NoMusica(Musica musica)
    {
        Dados = musica;
        Proximo = null;
    }
}

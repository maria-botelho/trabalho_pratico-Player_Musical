using System;
using System.Collections.Generic;

/// <summary>
/// Nó da árvore de gêneros.
/// Cada nó armazena um gênero e uma lista de músicas daquele gênero.
/// </summary>
public class NoArvoreGenero
{
    public string Genero { get; set; }
    public List<Musica> Musicas { get; set; }
    public NoArvoreGenero Esquerda { get; set; }
    public NoArvoreGenero Direita { get; set; }

    public NoArvoreGenero(string genero)
    {
        Genero = genero;
        Musicas = new List<Musica>();
    }
}

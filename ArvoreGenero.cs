using System;
using System.Collections.Generic;

/// <summary>
/// Árvore binária para indexar músicas por gênero.
/// Busca, inserção e recuperação são O(log n) em média.
/// </summary>
public class ArvoreGenero
{
    private NoArvoreGenero raiz;

    /// <summary>
    /// Insere uma música na árvore baseada no gênero.
    /// </summary>
    public void Inserir(Musica musica)
    {
        raiz = InserirRec(raiz, musica);
    }

    private NoArvoreGenero InserirRec(NoArvoreGenero atual, Musica musica)
    {
        if (atual == null)
        {
            NoArvoreGenero novo = new NoArvoreGenero(musica.Genero);
            novo.Musicas.Add(musica);
            return novo;
        }

        int comparacao = string.Compare(musica.Genero, atual.Genero, true);

        if (comparacao < 0)
            atual.Esquerda = InserirRec(atual.Esquerda, musica);
        else if (comparacao > 0)
            atual.Direita = InserirRec(atual.Direita, musica);
        else
            atual.Musicas.Add(musica);

        return atual;
    }

    /// <summary>
    /// Retorna todas as músicas de um gênero específico.
    /// </summary>
    public List<Musica> BuscarGenero(string genero)
    {
        NoArvoreGenero atual = raiz;

        while (atual != null)
        {
            int comparacao = string.Compare(genero, atual.Genero, true);

            if (comparacao == 0)
                return atual.Musicas;

            if (comparacao < 0)
                atual.Esquerda = atual.Esquerda;
            else
                atual = atual.Direita;
        }

        return null;
    }
}

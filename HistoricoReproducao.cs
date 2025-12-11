<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trabalho_pratico
{
    internal class HistoricoReproducao
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
            Console.WriteLine("\n--- Histórico ---");

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

}

=======
using System;

/// <summary>
/// Implementação manual de uma PILHA (stack) para armazenar o histórico de reprodução.
/// Funciona em LIFO (Last In, First Out) e guarda no máximo 10 músicas.
/// </summary>
public class HistoricoReproducao
{
    /// <summary>
    /// Referência para o topo da pilha (última música tocada).
    /// </summary>
    private NoMusica topo;

    /// <summary>
    /// Quantidade atual de músicas armazenadas no histórico.
    /// </summary>
    private int contagem;

    /// <summary>
    /// Capacidade máxima da pilha (histórico guarda no máximo 10 músicas).
    /// </summary>
    private const int CAPACIDADE_MAXIMA = 10;

    /// <summary>
    /// Construtor: inicializa a pilha vazia.
    /// </summary>
    public HistoricoReproducao()
    {
        topo = null;
        contagem = 0;
    }

    /// <summary>
    /// Adiciona uma música ao topo do histórico.
    /// Se passar de 10 músicas, remove automaticamente a mais antiga.
    /// </summary>
    public void Adicionar(Musica musica)
    {
        // Cria novo nó apontando para o antigo topo.
        NoMusica novoNo = new NoMusica(musica);
        novoNo.Proximo = topo;

        // Atualiza topo para o novo nó.
        topo = novoNo;
        contagem++;

        // Se ultrapassar a capacidade máxima, corta o último elemento da pilha.
        if (contagem > CAPACIDADE_MAXIMA)
        {
            NoMusica atual = topo;

            // Caminha até o nó que será o último permitido.
            for (int i = 0; i < CAPACIDADE_MAXIMA - 1; i++)
            {
                if (atual.Proximo == null) break;
                atual = atual.Proximo;
            }

            // Descarta tudo que estiver depois dele.
            atual.Proximo = null;
            contagem--;
        }
    }

    /// <summary>
    /// Remove e devolve a música do topo da pilha (última tocada).
    /// Se não houver músicas, devolve null.
    /// </summary>
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

    /// <summary>
    /// Propriedade somente leitura que devolve quantas músicas há no histórico.
    /// </summary>
    public int Contagem => contagem;

    /// <summary>
    /// Imprime na tela o histórico de músicas já tocadas, do mais recente para o mais antigo.
    /// </summary>
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
>>>>>>> 35f1d35c7a30c11a60748446dea87d496bfa1ade

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trabalho_pratico
{
    internal class ListaDuplamenteEncadeada
    {
        public NoDuplo Primeiro { get; private set; }
        public NoDuplo Ultimo { get; private set; }
        public int Tamanho { get; private set; }

        public ListaDuplamenteEncadeada()
        {
            Primeiro = null;
            Ultimo = null;
            Tamanho = 0;
        }
        public void InserirFim(Musica musica)
        {
            NoDuplo novoNo = new NoDuplo(musica);

            if (Primeiro == null)
            {
                Primeiro = novoNo;
                Ultimo = novoNo;
            }
            else
            {
                Ultimo.Proximo = novoNo;
                novoNo.Anterior = Ultimo;
                Ultimo = novoNo;
            }

            Tamanho++;
        }
        public bool Remover(string titulo, string artista)
        {
            NoDuplo atual = Primeiro;

            string tituloBusca = titulo.Trim().ToLower();
            string artistaBusca = artista.Trim().ToLower();

            while (atual != null)
            {
                if (atual.Dados.Titulo.Trim().ToLower() == tituloBusca &&
                    atual.Dados.Artista.Trim().ToLower() == artistaBusca)
                {

                    if (atual.Anterior != null)
                        atual.Anterior.Proximo = atual.Proximo;
                    else
                        Primeiro = atual.Proximo;

                    if (atual.Proximo != null)
                        atual.Proximo.Anterior = atual.Anterior;
                    else
                        Ultimo = atual.Anterior;

                    Tamanho--;
                    return true;
                }

                atual = atual.Proximo;
            }

            return false;
        }
        public Musica Buscar(string titulo, string artista)
        {
            NoDuplo atual = Primeiro;

            while (atual != null)
            {
                if (atual.Dados.Titulo == titulo && atual.Dados.Artista == artista)
                    return atual.Dados;

                atual = atual.Proximo;
            }

            return null;
        }
        public void Exibir()
        {
            NoDuplo atual = Primeiro;

            Console.WriteLine("\n--- PLAYLIST ---");
            while (atual != null)
            {
                Console.WriteLine($"{atual.Dados.Titulo} - {atual.Dados.Artista}");
                atual = atual.Proximo;
            }
        }
    }

}


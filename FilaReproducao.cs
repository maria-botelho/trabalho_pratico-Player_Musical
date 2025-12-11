using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trabalho_pratico
{
    internal class FilaReproducao
    {
        private NoMusica frente;
        private NoMusica traseira;
        private int contagem;

        public FilaReproducao()
        {
            frente = null;
            traseira = null;
            contagem = 0;
        }
        public void AdicionarMusica(Musica musica)
        {
            NoMusica novoNo = new NoMusica(musica);

            if (frente == null)
            {
                frente = novoNo;
                traseira = novoNo;
            }
            else
            {
                traseira.Proximo = novoNo;
                traseira = novoNo;
            }

            contagem++;
            Console.WriteLine($"\n[INFO] Adicionada à fila: {musica.Titulo}");
        }
        public Musica ProximaMusica()
        {
            if (frente == null)
            {
                return null;
            }

            Musica proxima = frente.Dados;
            frente = frente.Proximo;
            contagem--;

            if (frente == null)
            {
                traseira = null;
            }

            return proxima;
        }
        public void InsertInicio(Musica musica)
        {
            NoMusica novoNo = new NoMusica(musica);
            novoNo.Proximo = frente;
            frente = novoNo;
            contagem++;

            if (traseira == null)
            {
                traseira = novoNo;
            }
        }
        public int Contagem
        {
            get
            {
                return contagem;
            }
        }
        public Musica PrimeiraMusica
        {
            get { return frente != null ? frente.Dados : null; }
        }
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

}


using System;
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


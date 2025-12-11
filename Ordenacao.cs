using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trabalho_pratico
{
    internal class Ordenacao
    {
        public static List<Musica> OrdenarPorTitulo(List<Musica> lista)
        {
            return MergeSortTitulo(lista);
        }

        private static List<Musica> MergeSortTitulo(List<Musica> lista)
        {
            if (lista.Count <= 1) return lista;

            int meio = lista.Count / 2;

            var esquerda = MergeSortTitulo(lista.GetRange(0, meio));
            var direita = MergeSortTitulo(lista.GetRange(meio, lista.Count - meio));

            return MergeTitulo(esquerda, direita);
        }

        private static List<Musica> MergeTitulo(List<Musica> a, List<Musica> b)
        {
            List<Musica> resultado = new List<Musica>();
            int i = 0, j = 0;

            while (i < a.Count && j < b.Count)
            {
                if (string.Compare(a[i].Titulo, b[j].Titulo) < 0)
                    resultado.Add(a[i++]);
                else
                    resultado.Add(b[j++]);
            }

            resultado.AddRange(a.GetRange(i, a.Count - i));
            resultado.AddRange(b.GetRange(j, b.Count - j));

            return resultado;
        }
        public static List<Musica> OrdenarPorDuracao(List<Musica> lista)
        {
            return MergeSortDuracao(lista);
        }

        private static List<Musica> MergeSortDuracao(List<Musica> lista)
        {
            if (lista.Count <= 1) return lista;

            int meio = lista.Count / 2;

            var esquerda = MergeSortDuracao(lista.GetRange(0, meio));
            var direita = MergeSortDuracao(lista.GetRange(meio, lista.Count - meio));

            return MergeDuracao(esquerda, direita);
        }

        private static List<Musica> MergeDuracao(List<Musica> a, List<Musica> b)
        {
            List<Musica> resultado = new List<Musica>();
            int i = 0, j = 0;

            while (i < a.Count && j < b.Count)
            {
                if (a[i].DuracaoSegundos < b[j].DuracaoSegundos)
                    resultado.Add(a[i++]);
                else
                    resultado.Add(b[j++]);
            }

            resultado.AddRange(a.GetRange(i, a.Count - i));
            resultado.AddRange(b.GetRange(j, b.Count - j));

            return resultado;
        }
    }

}


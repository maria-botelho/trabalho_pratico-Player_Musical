using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trabalho_pratico
{
    internal class NoArvoreGenero
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
}

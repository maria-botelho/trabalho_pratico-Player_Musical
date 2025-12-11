using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trabalho_pratico
{
    internal class NoMusica
    {
        public Musica Dados { get; set; }
        public NoMusica Proximo { get; set; }

        public NoMusica(Musica musica)
        {
            Dados = musica;
            Proximo = null;
        }
    }
}

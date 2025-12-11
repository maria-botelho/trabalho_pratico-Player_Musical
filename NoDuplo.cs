using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trabalho_pratico
{
    internal class NoDuplo
    {
        public Musica Dados { get; set; }
        public NoDuplo Proximo { get; set; }
        public NoDuplo Anterior { get; set; }

        public NoDuplo(Musica musica)
        {
            Dados = musica;
            Proximo = null;
            Anterior = null;
        }
    }
}

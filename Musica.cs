using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trabalho_pratico
{
    internal class Musica
    {
        public string Titulo { get; set; }
        public string Artista { get; set; }
        public string Genero { get; set; }
        public int DuracaoSegundos { get; set; }

        public Musica(string titulo, string artista, string genero, int duracaoSegundos)
        {
            this.Titulo = titulo;
            this.Artista = artista;
            this.Genero = genero;
            this.DuracaoSegundos = duracaoSegundos;
        }
        public Musica() { }

        public string GerarChave()
        {
            string chave = $"{this.Titulo.Trim().ToLower()}|{this.Artista.Trim().ToLower()}";
            return chave;
        }
        public string DuracaoFormatada
        {
            get
            {
                TimeSpan tempo = TimeSpan.FromSeconds(DuracaoSegundos);
                return $"{(int)tempo.TotalMinutes}:{tempo.Seconds:D2}";
            }
        }
    }

}


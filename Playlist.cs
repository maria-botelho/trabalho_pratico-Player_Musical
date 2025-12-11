using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trabalho_pratico
{
    internal class Playlist
    {
        public string Nome { get; set; }
        public ListaDuplamenteEncadeada Musicas { get; private set; }

        public Playlist(string nome)
        {
            Nome = nome;
            Musicas = new ListaDuplamenteEncadeada();
        }

        public void AdicionarMusica(Musica musica)
        {
            Musicas.InserirFim(musica);
        }

        public bool RemoverMusica(string titulo, string artista)
        {
            return Musicas.Remover(titulo, artista);
        }

        public Musica Buscar(string titulo, string artista)
        {
            return Musicas.Buscar(titulo, artista);
        }

        public void ExibirPlaylist()
        {
            Console.WriteLine($"\nPlaylist: {Nome}");
            Musicas.Exibir();
        }
    }

}


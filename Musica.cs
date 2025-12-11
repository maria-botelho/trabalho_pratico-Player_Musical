<<<<<<< HEAD
﻿using System;
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

=======
using System;

/// <summary>
/// Representa uma música do catálogo, com título, artista, gênero e duração.
/// Também possui um método para gerar a chave única (Título + Artista).
/// </summary>
public class Musica
{
    /// <summary>
    /// Título da música.
    /// </summary>
    public string Titulo { get; set; }

    /// <summary>
    /// Nome do artista ou banda.
    /// </summary>
    public string Artista { get; set; }

    /// <summary>
    /// Gênero musical (MPB, Rock, Sertanejo, etc.).
    /// </summary>
    public string Genero { get; set; }

    /// <summary>
    /// Duração em segundos da música.
    /// </summary>
    public int DuracaoSegundos { get; set; }

    /// <summary>
    /// Construtor completo, usado ao criar uma música a partir do arquivo de texto.
    /// </summary>
    public Musica(string titulo, string artista, string genero, int duracaoSegundos)
    {
        this.Titulo = titulo;
        this.Artista = artista;
        this.Genero = genero;
        this.DuracaoSegundos = duracaoSegundos;
    }

    /// <summary>
    /// Construtor vazio, útil para criar uma música temporária (por exemplo, para gerar a chave de busca).
    /// </summary>
    public Musica() { }

    /// <summary>
    /// Gera a chave única da música, combinando Título + Artista em minúsculas.
    /// Essa chave é usada no dicionário do catálogo para evitar duplicidades e permitir busca O(1).
    /// </summary>
    public string GerarChave()
    {
        string chave = $"{this.Titulo.Trim().ToLower()}|{this.Artista.Trim().ToLower()}";
        return chave;
    }

    /// <summary>
    /// Propriedade somente leitura que devolve a duração formatada como mm:ss.
    /// </summary>
    public string DuracaoFormatada
    {
        get
        {
            TimeSpan tempo = TimeSpan.FromSeconds(DuracaoSegundos);
            return $"{(int)tempo.TotalMinutes}:{tempo.Seconds:D2}";
        }
    }
}
>>>>>>> 35f1d35c7a30c11a60748446dea87d496bfa1ade

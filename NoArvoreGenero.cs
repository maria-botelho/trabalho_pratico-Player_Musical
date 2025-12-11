<<<<<<< HEAD
﻿using System;
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
=======
using System;
using System.Collections.Generic;

/// <summary>
/// Nó da árvore de gêneros.
/// Cada nó armazena um gênero e uma lista de músicas daquele gênero.
/// </summary>
public class NoArvoreGenero
{
    public string Genero { get; set; }
    public List<Musica> Musicas { get; set; }
    public NoArvoreGenero Esquerda { get; set; }
    public NoArvoreGenero Direita { get; set; }

    public NoArvoreGenero(string genero)
    {
        Genero = genero;
        Musicas = new List<Musica>();
>>>>>>> 35f1d35c7a30c11a60748446dea87d496bfa1ade
    }
}

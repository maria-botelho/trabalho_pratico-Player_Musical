<<<<<<< HEAD
﻿using System;
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
=======
/// <summary>
/// Nó da lista duplamente encadeada.
/// Usado para armazenar músicas dentro das Playlists.
/// Cada nó aponta para o próximo e para o anterior.
/// </summary>
public class NoDuplo
{
    public Musica Dados { get; set; }
    public NoDuplo Proximo { get; set; }
    public NoDuplo Anterior { get; set; }

    public NoDuplo(Musica musica)
    {
        Dados = musica;
        Proximo = null;
        Anterior = null;
>>>>>>> 35f1d35c7a30c11a60748446dea87d496bfa1ade
    }
}

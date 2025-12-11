<<<<<<< HEAD
﻿using System;
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
=======
/// <summary>
/// Nó de lista encadeada que armazena uma música.
/// Essa estrutura é usada como base para PILHA (Histórico) e FILA (Fila de Reprodução).
/// </summary>
public class NoMusica
{
    /// <summary>
    /// Música armazenada neste nó.
    /// </summary>
    public Musica Dados { get; set; }

    /// <summary>
    /// Referência para o próximo nó da lista.
    /// </summary>
    public NoMusica Proximo { get; set; }

    /// <summary>
    /// Construtor que cria um nó contendo a música informada.
    /// </summary>
    public NoMusica(Musica musica)
    {
        Dados = musica;
        Proximo = null;
>>>>>>> 35f1d35c7a30c11a60748446dea87d496bfa1ade
    }
}

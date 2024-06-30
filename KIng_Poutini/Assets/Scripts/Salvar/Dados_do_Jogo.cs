using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DadosDoJogo {
    public int fasesCompletadas;
    public int vidas;
    public List<bool> colecionaveisColetados;

    public DadosDoJogo(int fasesCompletadas, int vidas, List<bool> colecionaveisColetados) {
        this.fasesCompletadas = fasesCompletadas;
        this.vidas = vidas;
        this.colecionaveisColetados = colecionaveisColetados;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DadosDoJogo {
    public int fasesCompletadas;
    public int vidas;

    public DadosDoJogo(int fasesCompletadas, int vidas){
        this.fasesCompletadas = fasesCompletadas;
        this.vidas = vidas;
    }
}

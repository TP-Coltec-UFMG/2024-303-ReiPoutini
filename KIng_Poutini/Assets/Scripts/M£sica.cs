using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class musica : MonoBehaviour{

    [SerializeField] private AudioSource Musica_Fundo;

    public void Volume (float V){

        Musica_Fundo.volume = V;
    }
}

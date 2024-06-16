using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectaJogador : MonoBehaviour {
    public static bool DetectarJogador = false;

    private void OnTriggerEnter2D(Collider2D colisao) {
        if (colisao.gameObject.tag == "Jogador") {
            DetectarJogador = true;
        }
    }

    private void OnTriggerExit2D(Collider2D colisao) {
        if (colisao.gameObject.tag == "Jogador") {
            DetectarJogador = false;
        }
    }
}

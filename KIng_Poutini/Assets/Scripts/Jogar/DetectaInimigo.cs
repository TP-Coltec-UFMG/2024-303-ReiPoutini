using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectaInimigo : MonoBehaviour {
    public static bool DetectarInimigo = false;

    private void OnTriggerEnter2D(Collider2D colisao) {
        if (colisao.gameObject.tag == "Inimigo") {
            DetectarInimigo = true;
        }
    }

    private void OnTriggerExit2D(Collider2D colisao) {
        if (colisao.gameObject.tag == "Inimigo") {
            DetectarInimigo = false;
        }
    }
}
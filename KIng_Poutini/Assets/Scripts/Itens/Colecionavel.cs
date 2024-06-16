using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colecionavel : MonoBehaviour{
    HUDControl HControl;

    void Start(){
        HControl = HUDControl.HControl;
    }

    private void OnTriggerEnter2D(Collider2D Colisao){
        if(Colisao.gameObject.tag == "Jogador"){
            HControl.AtivarColecionavel();
            Destroy(gameObject);
        }
    }
}

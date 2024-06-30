using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ataque : MonoBehaviour {

    private BoxCollider2D AtaqueColisao;
    
    void Start(){
        AtaqueColisao = GetComponent<BoxCollider2D>();
    }

    void Update(){
        if(Jogador.horizontalInput < 0){
            AtaqueColisao.offset = new Vector2(-1.45f, -0.3f);
        }else if(Jogador.horizontalInput > 0){
            AtaqueColisao.offset = new Vector2(-0.3f, -0.3f);
        }
    }
}

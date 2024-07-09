using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaqueAereo : MonoBehaviour{
    private BoxCollider2D AtaqueColisao;
    
    void Start(){
        AtaqueColisao = GetComponent<BoxCollider2D>();
    }

    void Update(){
        if(Jogador.horizontalInput < 0){
            AtaqueColisao.offset = new Vector2(0.4f, -1.0f);
        }else if(Jogador.horizontalInput > 0){
            AtaqueColisao.offset = new Vector2(-0.4f, -1.0f);
        }
    }
}

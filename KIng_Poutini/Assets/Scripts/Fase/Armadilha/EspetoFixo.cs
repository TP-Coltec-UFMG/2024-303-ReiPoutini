using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EspetoFixo : MonoBehaviour {
    BoxCollider2D bc;
    public Rigidbody2D rb;

    private int DestruindoHash = Animator.StringToHash("Destruido");

    private void Start(){
        bc = GetComponent<BoxCollider2D>();    
    }

    private void OnCollisionEnter2D(Collision2D colisao){
        if(colisao.gameObject.tag == "Jogador"){
            Destroy(bc);
            Destroy(this);
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}

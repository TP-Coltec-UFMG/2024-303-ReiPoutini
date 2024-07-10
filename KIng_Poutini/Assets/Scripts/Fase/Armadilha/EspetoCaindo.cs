using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EspetoCaindo : MonoBehaviour{
    public Animator anim;
    public BoxCollider2D bc;

    private int DestruindoHash = Animator.StringToHash("Destruido");

    void Start(){
        anim = GetComponent<Animator>();
        bc = GetComponent<BoxCollider2D>();    
    }

    private void OnCollisionEnter2D(Collision2D colisao){
        if(colisao.gameObject.tag == "Chao"){
            anim.SetTrigger(DestruindoHash);
        
            Destroy(bc);
            Destroy(gameObject, 0.7f);
        }
    }
}

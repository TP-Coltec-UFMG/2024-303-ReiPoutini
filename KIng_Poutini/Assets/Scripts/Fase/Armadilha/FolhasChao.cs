using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FolhasChao : MonoBehaviour {
    public Animator animador;
    private BoxCollider2D ArmadilhaBox;
    public GameObject AreaSecreta;

    private int AbrirHash = Animator.StringToHash("Ativa");

    void Start(){
        ArmadilhaBox = GetComponent<BoxCollider2D>();
        AreaSecreta.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D colisao) {
        if(colisao.CompareTag("Jogador")){
            animador.SetTrigger(AbrirHash);
            AreaSecreta.SetActive(false);
        }
    }
}
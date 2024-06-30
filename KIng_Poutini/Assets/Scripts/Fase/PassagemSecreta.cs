using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassagemSecreta : MonoBehaviour{
    public GameObject AreaSecreta;

    private void Start(){
        AreaSecreta.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D BC){
        if (BC.CompareTag("Jogador")){
            AreaSecreta.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D BC){
        if (BC.CompareTag("Jogador")){
            AreaSecreta.SetActive(true);
        }
    }
}

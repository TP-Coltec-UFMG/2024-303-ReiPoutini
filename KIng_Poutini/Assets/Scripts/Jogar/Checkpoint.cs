using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D colisao){
        if(colisao.CompareTag("Jogador")){
            GameManager.Instance.UpdateCheckpoint(transform.position);
        }
    } 
}

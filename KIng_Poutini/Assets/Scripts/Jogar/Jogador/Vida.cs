using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vida : MonoBehaviour {
    private HUDControl HControl;
    private Jogador jogador;

    public static BoxCollider2D bc;

    private void Start() {
        HControl = HUDControl.HControl;
        jogador = Jogador.player;
        bc = GetComponent<BoxCollider2D>();
    }

    private void Update(){
        if(Jogador.horizontalInput < 0){
            bc.offset = new Vector2(0.25f, -0.03f);
        }else if(Jogador.horizontalInput > 0){
            bc.offset = new Vector2(-0.25f, -0.03f);
        }
    }

    private void OnTriggerEnter2D(Collider2D colisao) {
        if (colisao.CompareTag("Inimigo") && !jogador.EstaInvencivel()) {
            HControl.PerderVida();
            HControl.Dano();
            bc.enabled = false;

            if (HControl.Vida() > 0) {
                StartCoroutine(jogador.Sofrendo());
            }
        }

        if(colisao.CompareTag("Agua")){
            HControl.TocouNaAgua();
        }

        if (colisao.CompareTag("FrutaPao")) {
            HControl.aumentaVida();
            Destroy(colisao.gameObject);
        }

        if (colisao.CompareTag("Mauri")) {
            jogador.AtivarInvencibilidade(10.0f);
            Destroy(colisao.gameObject);
        }

        if(colisao.CompareTag("Coracao")){
            HControl.AumentarVidas();
            Destroy(colisao.gameObject);
        }
    }
}

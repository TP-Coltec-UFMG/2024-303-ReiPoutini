using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaracolMola : MonoBehaviour {
    public Animator anim;
    private int pulandoHash = Animator.StringToHash("Pulo");

    [SerializeField] private float impulsoForca = 10f;

    void Start() {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D colisao) {
        if (colisao.gameObject.CompareTag("Jogador")) {
            anim.SetBool(pulandoHash, true);

            Jogador jogador = colisao.gameObject.GetComponent<Jogador>();
            if (jogador != null) {
                Rigidbody2D jogadorRb = jogador.GetComponent<Rigidbody2D>();
                if (jogadorRb != null) {
                    jogadorRb.velocity = new Vector2(jogadorRb.velocity.x, impulsoForca);
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D colisao) {
        if (colisao.gameObject.CompareTag("Jogador")) {
            anim.SetBool(pulandoHash, false);
        }
    }
}

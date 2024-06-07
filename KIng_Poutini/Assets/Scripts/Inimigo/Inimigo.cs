using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : MonoBehaviour {

    [SerializeField] private float velocidade = 4;
    [SerializeField] private Transform pontoA;
    [SerializeField] private Transform pontoB;
    [SerializeField] private LayerMask chacLayer;
    [SerializeField] private int vidaMaxima = 3;
    
    private bool indoParaPontoB;
    private Transform alvoAtual;
    private SpriteRenderer spriteRenderer;
    private int vidaAtual;

    private void Awake() {
        alvoAtual = pontoB;
        spriteRenderer = GetComponent<SpriteRenderer>();
        vidaAtual = vidaMaxima;
    }

    void Update() {
        Patrulhar();
        VerificarVirada();
    }

    private void Patrulhar() {
        transform.position = Vector2.MoveTowards(transform.position, alvoAtual.position, velocidade * Time.deltaTime);

        if (Vector2.Distance(transform.position, alvoAtual.position) < 0.1f) {
            indoParaPontoB = !indoParaPontoB;
            alvoAtual = indoParaPontoB ? pontoB : pontoA;
        }
    }

    private void VerificarVirada() {
        if ((alvoAtual.position.x > transform.position.x && spriteRenderer.flipX) || 
            (alvoAtual.position.x < transform.position.x && !spriteRenderer.flipX)) {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jogador : MonoBehaviour
{
    [SerializeField] private float velocidade = 5;
    [SerializeField] private float Forca = 600;

    [SerializeField] private Transform peDoPersonagem;
    [SerializeField] private LayerMask chaoLayer;

    private bool estaChao;
    private Animator animation;
    private SpriteRenderer spriteRenderer;

    private float horizontalinput;
    private Rigidbody2D rb; 

    private int andandoHash = Animator.StringToHash("Andando");
    private int saltandoHash = Animator.StringToHash("Saltando");

    
    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        animation = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update(){
        horizontalinput = Input.GetAxis("Horizontal");

        if(Input.GetKeyDown(KeyCode.Space)&& estaChao){
            rb.AddForce(Vector2.up * Forca);
        }

        estaChao = Physics2D.OverlapCircle(peDoPersonagem.position, 0.2f, chaoLayer);

        animation.SetBool(andandoHash, horizontalinput != 0);
        animation.SetBool(saltandoHash, !estaChao);

        if(horizontalinput > 0){
            spriteRenderer.flipX = false;
        } else{
            spriteRenderer.flipX = true;
        }

    }

    private void FixedUpdate(){
        rb.velocity = new Vector2(horizontalinput * velocidade, rb.velocity.y);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jogador : MonoBehaviour {
    // Outros Scripts acessam as funções e variáveis públicas
    public static Jogador player { get; private set; }

    // Movimentação
    [SerializeField] private float velocidadeInicial = 5f;
    [SerializeField] private float velocidadeCorrendo = 10f;
    [SerializeField] private float forca = 600f;
    [SerializeField] private Transform peDoPersonagem;
    [SerializeField] private LayerMask Chao;
    [SerializeField] private float incrementoVelocidade = 2.0f;

    // Animação, movimentações e colisões com o chão
    private bool estaChao;
    public Animator animador;
    public static float horizontalInput;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D jogadorCollider;
    [SerializeField] private bool Ataque;
    public bool AtaqueDuplo, AtaqueTriplo, AtaqueQuadruplo, LockeAtaque = false;

    private int andandoHash = Animator.StringToHash("andando");
    private int saltandoHash = Animator.StringToHash("saltando");
    private int correndoHash = Animator.StringToHash("correndo");
    private int ataqueHash1 = Animator.StringToHash("ataque1");
    private int ataqueHash2 = Animator.StringToHash("ataque2");
    private int ataqueHash3 = Animator.StringToHash("ataque3");
    private int ataqueHash4 = Animator.StringToHash("ataque4");
    private int ataquepuloHash = Animator.StringToHash("AtaqueAr");

    private bool BlockInput = false;
    private float tempoAndando = 0f;
    private float velocidadeAtual;
    private bool invencivel = false;
    private int camadaOriginal;

    private void Awake() {
        if (player == null) {
            player = this;
        } else {
            Destroy(gameObject);
        }

        rb = GetComponent<Rigidbody2D>();
        animador = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        jogadorCollider = GetComponent<BoxCollider2D>();
        velocidadeAtual = velocidadeInicial;
        camadaOriginal = gameObject.layer;
    }

    private void Update() {
        if (Time.timeScale == 0f) return;

        // Só ocorre se não foi bloquado
        if (!BlockInput) {
            //Anda
            horizontalInput = Input.GetAxis("Horizontal");

            //Pula
            if (Input.GetKeyDown(KeyCode.UpArrow) && estaChao && Ataque == false) {
                rb.AddForce(Vector2.up * forca);
            }

            //Ataque
            if(Input.GetKeyDown(KeyCode.A) && LockeAtaque == false){
                Ataque = true;
                if(estaChao){
                    animador.SetTrigger(ataqueHash1);
                }else{
                    animador.SetTrigger(ataquepuloHash);
                }
                if(AtaqueDuplo == true){
                    animador.SetTrigger(ataqueHash2);
                }
                if(AtaqueTriplo == true){
                    animador.SetTrigger(ataqueHash3);
                }
                if(AtaqueQuadruplo == true){
                    animador.SetTrigger(ataqueHash4);
                }
            }

            if(Ataque == true && estaChao){
                horizontalInput = 0;
            }

            estaChao = Physics2D.OverlapCircle(peDoPersonagem.position, 0.2f, Chao);

            animador.SetBool(andandoHash, horizontalInput != 0);
            animador.SetBool(saltandoHash, !estaChao);

            if (horizontalInput != 0) {
                tempoAndando += Time.deltaTime;
                velocidadeAtual = Mathf.Min(velocidadeCorrendo, velocidadeAtual + incrementoVelocidade * Time.deltaTime);

                animador.SetBool(correndoHash, velocidadeAtual >= velocidadeCorrendo);
            } else {
                tempoAndando = 0f;
                velocidadeAtual = velocidadeInicial;
                animador.SetBool(correndoHash, false);
            }

            if (horizontalInput > 0) {
                spriteRenderer.flipX = false;
                jogadorCollider.offset = new Vector2(-0.25f, -0.03f);
            } else if (horizontalInput < 0) {
                spriteRenderer.flipX = true;
                jogadorCollider.offset = new Vector2(0.25f, -0.03f);
            }
        }
    }

    private void FixedUpdate() {
        rb.velocity = new Vector2(horizontalInput * velocidadeAtual, rb.velocity.y);
    }

    void AcabaAtaqueSimples(){
        Ataque = false;
    }

    void AcabaAtaqueDuplo(){
        AtaqueDuplo = false;
        Ataque = false;
    }

    void AcabaAtaqueTriplo(){
        AtaqueTriplo = false;
        Ataque = false;
    }

    void AcabaAtaqueQuadruplo(){
        AtaqueQuadruplo = false;
        Ataque = false;
    }

    public IEnumerator Sofrendo() {
        animador.SetBool("sofrendo", true);
        spriteRenderer.color = new Color(1f, 0, 0, 1f);
        yield return new WaitForSeconds(0.4f);
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        animador.SetBool("sofrendo", false);

        for (int i = 0; i < 7; i++) {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.15f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.15f);
        }

        Vida.bc.enabled = true;
    }

    public void Morte() {
        Destroy(gameObject);
        BlockInput = true;
        velocidadeInicial = 0;
        GetComponent<Collider2D>().enabled = false;
    }

    public void AtivarInvencibilidade(float duracao) {
        StartCoroutine(Invencibilidade(duracao));
    }

    private IEnumerator Invencibilidade(float duracao) {
        invencivel = true;
        gameObject.layer = LayerMask.NameToLayer("Invencivel"); // Altera a camada para 'Invencivel'
        spriteRenderer.color = new Color(1f, 1f, 0, 1f); // Amarelo
        yield return new WaitForSeconds(duracao);
        invencivel = false;
        gameObject.layer = camadaOriginal; //Restaura a camada original
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f); // Branco normal
    }

    public bool EstaInvencivel() {
        return invencivel;
    }
}
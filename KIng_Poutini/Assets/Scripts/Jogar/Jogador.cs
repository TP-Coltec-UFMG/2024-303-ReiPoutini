using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Jogador : MonoBehaviour {
    // Outros Scripts acessam as funções e variáveis públicas
    public static Jogador player { get; private set; }


    // Movimentação
    [SerializeField] private float velocidadeInicial = 5f;
    [SerializeField] private float velocidadeCorrendo = 10f;
    [SerializeField] private float forca = 6f;
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
    public bool EmAtaque = false;
    public event Action OnPlayerJump;

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
    public int vidas = 6;
    public int fasesCompletadas = 0;
    private Vector2 swapV;

    
    private void Awake() {
        if (player == null) {
            player = this;
        } else {
            Destroy(gameObject);
        }
        CarregarProgresso();

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

            if(EmAtaque == false){
                //Anda
                horizontalInput = Input.GetAxis("Horizontal");

                //Pula
                if (Input.GetKeyDown(KeyCode.UpArrow) && estaChao && Ataque == false) {
                    swapV = rb.velocity;
                    swapV.y = forca;
                    rb.velocity = swapV;
                    OnPlayerJump?.Invoke();
                }
            }

            //Ataque
            if(Input.GetKeyDown(KeyCode.A) && LockeAtaque == false){
                Ataque = true;
                EmAtaque = true;
                if(estaChao && AtaqueDuplo == false && AtaqueTriplo == false && AtaqueQuadruplo == false){
                    animador.SetTrigger(ataqueHash1);
                }else if(!estaChao){
                    animador.SetTrigger(ataquepuloHash);
                }
                if(AtaqueDuplo == true && AtaqueTriplo == false && AtaqueQuadruplo == false){
                    animador.SetTrigger(ataqueHash2);
                }
                if(AtaqueTriplo == true && AtaqueDuplo == false && AtaqueQuadruplo == false){
                    animador.SetTrigger(ataqueHash3);
                }
                if(AtaqueQuadruplo == true && AtaqueDuplo == false && AtaqueTriplo == false){
                    animador.SetTrigger(ataqueHash4);
                }
            }

            if(Ataque == true && estaChao){
                horizontalInput = 0;
            }

            estaChao = Physics2D.OverlapCircle(peDoPersonagem.position, 0.5f, Chao);

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
        EmAtaque = false;
    }

    void AcabaAtaqueDuplo(){
        AtaqueDuplo = false;
        Ataque = false;
        EmAtaque = false;
    }

    void AcabaAtaqueTriplo(){
        AtaqueTriplo = false;
        Ataque = false;
        EmAtaque = false;
    }

    void AcabaAtaqueQuadruplo(){
        AtaqueQuadruplo = false;
        Ataque = false;
        EmAtaque = false;
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
    vidas--;
        if (vidas <= 0) {
            Debug.Log("Game Over");
            Destroy(gameObject);
            BlockInput = true;
            velocidadeInicial = 0;
            GetComponent<Collider2D>().enabled = false;
            SalvarProgresso();
        } else {
            SalvarProgresso();
            Reviver();
        }
        HUDControl.HControl.AtualizarVidas();
    }

    public void Reviver(){
        transform.position = GameManager.Instance.GetCheckpointPosition();
        HUDControl.HControl.RestauraVida();
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

    public void SalvarProgresso() {
        DadosDoJogo dados = SistemadeSave.pegardados();
        List<bool> colecionaveisColetados = dados != null ? dados.colecionaveisColetados : new List<bool>();
        dados = new DadosDoJogo(fasesCompletadas, vidas, colecionaveisColetados);
        SistemadeSave.SalvarDados(dados);
    }

    private void CarregarProgresso() {
        DadosDoJogo dados = SistemadeSave.pegardados();
        if (dados != null) {
            vidas = dados.vidas;
            fasesCompletadas = dados.fasesCompletadas;
        }
    }
}
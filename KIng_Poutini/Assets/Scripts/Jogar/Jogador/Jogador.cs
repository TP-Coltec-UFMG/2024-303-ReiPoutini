using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Jogador : MonoBehaviour {
    public static Jogador player { get; private set; }

    [SerializeField] private float velocidadeInicial = 5f;
    [SerializeField] private float velocidadeCorrendo = 10f;
    [SerializeField] private float forca = 6f;
    [SerializeField] private Transform peDoPersonagem;
    [SerializeField] private LayerMask Chao;
    [SerializeField] private float incrementoVelocidade = 2.0f;

    private bool estaChao;
    public Animator animador;
    public static float horizontalInput;
    public Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject escudoTamahua;
    private BoxCollider2D jogadorCollider;
    [SerializeField] private bool Ataque;
    public bool AtaqueDuplo, AtaqueTriplo, AtaqueQuadruplo, LockeAtaque = false;
    public bool EmAtaque = false;
    public BoxCollider2D Detecta;
    public event Action OnPlayerJump;

    private int andandoHash = Animator.StringToHash("andando");
    private int saltandoHash = Animator.StringToHash("saltando");
    private int correndoHash = Animator.StringToHash("correndo");
    private int ataqueHash1 = Animator.StringToHash("ataque1");
    private int ataqueHash2 = Animator.StringToHash("ataque2");
    private int ataqueHash3 = Animator.StringToHash("ataque3");
    private int ataqueHash4 = Animator.StringToHash("ataque4");
    private int ataquepuloHash = Animator.StringToHash("AtaqueAr");
    private int detectarHash = Animator.StringToHash("Detecta");
    private int MorteHash = Animator.StringToHash("Morre");
    private int NasceHash = Animator.StringToHash("Nasce");

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
        if (Time.timeScale == 0f || BlockInput) return;

        if (!EmAtaque) {
            horizontalInput = Input.GetAxis("Horizontal");

            if (Input.GetKeyDown(KeyCode.UpArrow) && estaChao && !Ataque) {
                swapV = rb.velocity;
                swapV.y = forca;
                rb.velocity = swapV;
                OnPlayerJump?.Invoke();
            }
        }

        if (Input.GetKeyDown(KeyCode.A) && !LockeAtaque) {
            Ataque = true;
            EmAtaque = true;
            if (estaChao && !AtaqueDuplo && !AtaqueTriplo && !AtaqueQuadruplo) {
                animador.SetTrigger(ataqueHash1);
            } else if (!estaChao) {
                animador.SetBool(ataquepuloHash, true);
            } else if (AtaqueDuplo && !AtaqueTriplo && !AtaqueQuadruplo) {
                animador.SetTrigger(ataqueHash2);
            } else if (AtaqueTriplo && !AtaqueDuplo && !AtaqueQuadruplo) {
                animador.SetTrigger(ataqueHash3);
            } else if (AtaqueQuadruplo && !AtaqueDuplo && !AtaqueTriplo) {
                animador.SetTrigger(ataqueHash4);
            }
        }

        if (Ataque && estaChao) {
            horizontalInput = 0;
        }

        estaChao = Physics2D.OverlapCircle(peDoPersonagem.position, 0.8f, Chao);

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
            Detecta.offset = new Vector2(0.9f, -1.1f);
        } else if (horizontalInput < 0) {
            spriteRenderer.flipX = true;
            jogadorCollider.offset = new Vector2(0.25f, -0.03f);
            Detecta.offset = new Vector2(-0.9f, -1.1f);
        }

        if (animador.GetCurrentAnimatorStateInfo(0).IsName("GolpeAr") && animador.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f) {
            if (DetectaInimigo.DetectarInimigo) {
                animador.SetTrigger(detectarHash);
            }
        }
    }


    private void FixedUpdate() {
        rb.velocity = new Vector2(horizontalInput * velocidadeAtual, rb.velocity.y);
    }

    void AcabaAtaqueSimples() {
        Ataque = false;
        EmAtaque = false;
    }

    void AcabaAtaqueAereo() {
        animador.SetBool(ataquepuloHash, false);
        EmAtaque = false;
        Ataque = false;
    }

    void AcabaAtaqueDuplo() {
        AtaqueDuplo = false;
        Ataque = false;
        EmAtaque = false;
    }

    void AcabaAtaqueTriplo() {
        AtaqueTriplo = false;
        Ataque = false;
        EmAtaque = false;
    }

    void AcabaAtaqueQuadruplo() {
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
        StartCoroutine(HandleMorte());
    }

    private IEnumerator HandleMorte() {
        BlockInput = true;
        animador.SetTrigger(MorteHash);

        yield return new WaitUntil(() => animador.GetCurrentAnimatorStateInfo(0).IsName("Morte") && animador.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);


        vidas--;
        SalvarProgresso();
        HUDControl.HControl.AtualizarVidas();

        if (vidas <= 0) {
            velocidadeInicial = 0;
            GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject);
            BlockInput = false;
        } else {
            Reviver();
        }

        BlockInput = false;
    }



    public void Reviver() {
        StartCoroutine(HandleReviver());

        Debug.Log("check");
        transform.position = GameManager.Instance.GetCheckpointPosition();
        HUDControl.HControl.RestauraVida();

        BlockInput = false;
    }

    private IEnumerator HandleReviver() {
        BlockInput = true;
        animador.SetTrigger(NasceHash);

        yield return new WaitUntil(() => animador.GetCurrentAnimatorStateInfo(0).IsName("Nasce") && animador.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
    }

    public void AtivarInvencibilidade(float duracao) {
        StartCoroutine(Invencibilidade(duracao));
    }

    private IEnumerator Invencibilidade(float duracao) {
        invencivel = true;
        gameObject.layer = LayerMask.NameToLayer("Invencivel");
        escudoTamahua.SetActive(true);
        spriteRenderer.color = new Color(1f, 0, 0, 1f);
        if(horizontalInput > 0){
            Vector3 novaPosicao = transform.position;
            novaPosicao.x = -11.9f;
            escudoTamahua.transform.position = novaPosicao;
        }else if (horizontalInput < 0) {
            Vector3 novaPosicao = transform.position;
            novaPosicao.x = -11.6f;
            escudoTamahua.transform.position = novaPosicao;
        }
        yield return new WaitForSeconds(duracao);
        invencivel = false;
        gameObject.layer = camadaOriginal;
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        escudoTamahua.SetActive(false);
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
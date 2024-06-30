using System.Collections;
using UnityEngine;

public class CobraInimigo : MonoBehaviour {
    public Animator anim;
    public SpriteRenderer sprites;

    [SerializeField] private int vida = 1;
    [SerializeField] private float velocidade = 5.0f;

    public Transform pontoA;
    public Transform pontoB;

    private Transform destinoAtual;
    public BoxCollider2D Ataque;
    public BoxCollider2D Detectar;
    public GameObject ItemPrefab;  //prefab do item a ser dropado

    private bool atacando = false;
    private bool jogadorNaFrente = false;
    private bool inimigoMorto = false;

    [SerializeField] private float tempoInvencibilidadeJogador = 2.0f;

    void Start() {
        anim = GetComponent<Animator>();
        sprites = GetComponent<SpriteRenderer>();

        destinoAtual = pontoB;
        transform.position = pontoA.position;
        AtualizarDirecaoEColisor();
    }

    void Update() {
        if (vida <= 0 && !inimigoMorto) {
            InimigoMorto();
        }
    }

    private void FixedUpdate() {
        if (!atacando && !inimigoMorto) {
            Movimento();
        }
    }

    void AtualizarDirecaoEColisor() {
        if (destinoAtual == pontoA) {
            sprites.flipX = true;
            Ataque.offset = new Vector2(0.50f, -0.06f);
            Detectar.offset = new Vector2(-4.5f, 0.8f);
        } else {
            sprites.flipX = false;
            Ataque.offset = new Vector2(2.09f, -0.06f);
            Detectar.offset = new Vector2(-2.5f, 0.8f);
        }
    }

    void Movimento() {
        transform.position = Vector2.MoveTowards(transform.position, destinoAtual.position, velocidade * Time.deltaTime);

        if (DetectaJogador.DetectarJogador) {
            if (!atacando) {
                StartCoroutine(Atacando());
                anim.SetBool("Andando", true);
            }
        }

        if (Vector2.Distance(transform.position, destinoAtual.position) < 0.1f) {
            destinoAtual = (destinoAtual == pontoA) ? pontoB : pontoA;
            AtualizarDirecaoEColisor();
        }

        anim.SetBool("Andando", velocidade != 0 && !atacando);
    }

    private void InimigoMorto() {
        inimigoMorto = true;
        vida = 0;
        anim.SetTrigger("Morreu");
        velocidade = 0;
        Destroy(GetComponent<BoxCollider2D>());
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(Detectar);
        Destroy(Ataque);

        DropItem();

        Destroy(gameObject, 2f);
    }

    private void DropItem() { 
        int random = Random.Range(0, 10); 
        if (random >= 5) { 
            Instantiate(ItemPrefab, new Vector2 (gameObject.transform.position.x,(gameObject.transform.position.y + 0.9f)), transform.rotation); 
        } 
    } 

    IEnumerator Atacando() {
        atacando = true;
        anim.SetBool("Ataque", true);
        velocidade = 0;

        yield return new WaitForSeconds(0.85f);

        anim.SetBool("Ataque", false);
        yield return new WaitForSeconds(tempoInvencibilidadeJogador);

        anim.SetBool("Ataque", false);

        if (jogadorNaFrente) {
            StartCoroutine(Atacando());
        } else {
            atacando = false;
            velocidade = 5.0f;
        }

        DetectaJogador.DetectarJogador = false;
    }

    private void OnTriggerEnter2D(Collider2D colisao) {
        if (colisao.gameObject.tag == "Soco") {
            vida--;
            if(vida > 0){
                anim.SetTrigger("sofrendo");
            }
        }

        if (vida < 1) {
            StopCoroutine("Atacando");
            InimigoMorto();
        }
        if (colisao.gameObject.tag == "Jogador") {
            jogadorNaFrente = true;
        }
    }

    private void OnTriggerExit2D(Collider2D colisao) {
        if (colisao.gameObject.tag == "Jogador") {
            jogadorNaFrente = false;
        }
    }
}
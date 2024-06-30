using System.Collections;
using UnityEngine;

public class CoelhoInimigo : MonoBehaviour {
    public Animator anim;
    public SpriteRenderer sprites;

    [SerializeField] private int vida = 1;
    [SerializeField] private float velocidade = 5.0f;

    public Transform pontoA;
    public Transform pontoB;

    private Transform destinoAtual;
    public BoxCollider2D Ataque;
    public GameObject ItemPrefab;  //prefab do item a ser dropado

    private bool inimigoMorto = false;

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
        if (!inimigoMorto) {
            Movimento();
        }
    }

    void AtualizarDirecaoEColisor() {
        if (destinoAtual == pontoA) {
            sprites.flipX = true;
            Ataque.offset = new Vector2(-0.1f, -0.15f);
        } else {
            sprites.flipX = false;
            Ataque.offset = new Vector2(0.9f, -0.15f);
        }
    }

    void Movimento() {
        transform.position = Vector2.MoveTowards(transform.position, destinoAtual.position, velocidade * Time.deltaTime);

        if (Vector2.Distance(transform.position, destinoAtual.position) < 0.1f) {
            destinoAtual = (destinoAtual == pontoA) ? pontoB : pontoA;
            AtualizarDirecaoEColisor();
        }

        anim.SetBool("Andando", velocidade != 0);
    }

    private void InimigoMorto() {
        inimigoMorto = true;
        vida = 0;
        anim.SetTrigger("Morreu");
        velocidade = 0;
        Destroy(GetComponent<BoxCollider2D>());
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(Ataque);

        DropItem();

        Destroy(gameObject, 2f);
    }

    private void DropItem() { 
        int random = Random.Range(0, 10); 
        if (random >= 5) { 
            Instantiate(ItemPrefab, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 0.9f), transform.rotation); 
        } 
    } 

    private void OnTriggerEnter2D(Collider2D colisao) {
        if (colisao.gameObject.tag == "Soco") {
            vida--;
            if (vida > 0) {
                StartCoroutine(SofrendoCoroutine());
            } else {
                InimigoMorto();
            }
        }
    }

    private IEnumerator SofrendoCoroutine() {
        anim.SetTrigger("sofrendo");
        velocidade = 0;
        yield return null;
    }

    public void VoltarAMovimentar() {
        if (vida > 0) {
            velocidade = 5.0f;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDControl : MonoBehaviour {
    public static HUDControl HControl { get; private set; }
    private Jogador jogador;

    // HP do Personagem
    [SerializeField] private int vida;
    [SerializeField] private int vidaMaxima;
    public Image[] frutapao;
    public Sprite cheio;
    public Color vazio;

    // Vida total
    [SerializeField] private TextMeshProUGUI vidasText; 
    public Image personagemImagem;


    // Itens colecionaveis
    public GameObject Colecionavel;
    public bool ColOn;

    private void Awake() {
        if (HControl == null) {
            HControl = this;
        } else {
            Destroy(gameObject);
        }

        jogador = FindObjectOfType<Jogador>();
        AtualizarVidas();
    }

    public void AtivarColecionavel() {
        Colecionavel.SetActive(true);
        ColOn = true;
    }

    public void PerderVida() {
        vida--;
        AtualizarVida();
        if (vida <= 0) {
            vida = 0;
            Morto();
        }
    }

    public void AtualizarVidas() {
        vidasText.text = "X" + jogador.vidas;
    }

    public void AumentarVidas(){
        jogador.vidas++;
        AtualizarVidas();
    }

    private void Morto() {
        jogador.Morte();
    }

    public void aumentaVida() {
        vida++;
        AtualizarVida();
    }

    public void AtualizarVida() {
        if (vida > vidaMaxima) {
            vida = vidaMaxima;
        }

        for (int i = 0; i < frutapao.Length; i++) {
            if (i < vida) {
                frutapao[i].sprite = cheio;
                frutapao[i].color = Color.white;
                frutapao[i].enabled = true;
            } else {
                frutapao[i].color = vazio;
                frutapao[i].enabled = i < vidaMaxima;
            }
        }
    }

    public void RestauraVida() {
        vida = vidaMaxima;
        AtualizarVida();
    }

    public int Vida() {
        return vida;
    }

    public void Dano() {
        StartCoroutine(DamageHUD());
    }

    private IEnumerator DamageHUD() {
        yield return new WaitForSeconds(0.1f);
    }
}

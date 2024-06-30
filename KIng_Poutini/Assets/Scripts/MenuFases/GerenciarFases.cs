using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GerenciarFases : MonoBehaviour {
    [SerializeField] private Button[] botaoFase;
    [SerializeField] private List<Sprite> spritesIncompletos;
    [SerializeField] private List<Sprite> spritesCompletos;
    [SerializeField] private GameObject personagem;
    [SerializeField] private float velocidadeMovimento = 2f;
    [SerializeField] private MenuIntermediario[] menusIntermediarios;

    private Animator animador;
    private SpriteRenderer spriteRenderer;
    public int faseAtual = 0;
    private bool isMoving = false;
    private int fasesCompletadas = 0;
    private int vidas = 6;
    private int andandoHash = Animator.StringToHash("Andando");
    private bool menuIntermediarioAtivo = false;
    private bool inputLocked = false;

    void Start() {
        CarregarProgresso();
        AtualizarBotoesFase();

        faseAtual = Mathf.Clamp(fasesCompletadas - 1, 0, botaoFase.Length - 1);

        if (botaoFase.Length > 0) {
            personagem.transform.position = botaoFase[faseAtual].transform.position;
        } else {
            Debug.LogError("O array botaoFase não contém elementos!");
        }

        foreach (var menu in menusIntermediarios) {
            menu.Ocultar();
        }
    }

    void Awake() {
        animador = personagem.GetComponent<Animator>();
        spriteRenderer = personagem.GetComponent<SpriteRenderer>();
    }

    void Update() {
        if (!menuIntermediarioAtivo && !inputLocked) {
            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                MoverParaFase(faseAtual + 1);
            } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                MoverParaFase(faseAtual - 1);
            } else if (Input.GetKeyDown(KeyCode.Return)) {
                    AbrirMenuIntermediario(faseAtual);
            }
        }
    }

    void MoverParaFase(int novaFase) {
        if (novaFase >= 0 && novaFase < botaoFase.Length && botaoFase[novaFase].interactable && !isMoving) {
            faseAtual = novaFase;
            StartCoroutine(MoverPersonagem(botaoFase[faseAtual].transform.position));
        }
    }

    IEnumerator MoverPersonagem(Vector3 novaPosicao) {
        isMoving = true;
        animador.SetBool(andandoHash, true);

        Vector3 direcao = novaPosicao - personagem.transform.position;
        if (direcao.x < 0) {
            spriteRenderer.flipX = true;
        } else if (direcao.x > 0) {
            spriteRenderer.flipX = false;
        }

        while (Vector3.Distance(personagem.transform.position, novaPosicao) > 0.1f) {
            personagem.transform.position = Vector3.MoveTowards(personagem.transform.position, novaPosicao, velocidadeMovimento * Time.deltaTime);
            yield return null;
        }

        personagem.transform.position = novaPosicao;
        isMoving = false;
        animador.SetBool(andandoHash, false);
    }

    public void AbrirMenuIntermediario(int numFase) {
        if (numFase > faseAtual + 1) {
            return;
        }
        
        faseAtual = Mathf.Max(numFase - 1, faseAtual);
        menusIntermediarios[faseAtual].ConfigurarMenu("Fase " + (numFase + 1), spritesCompletos[numFase], () => CarregarFase(numFase + 1), FecharMenuIntermediario);
        menusIntermediarios[faseAtual].Mostrar();
        DesativarSelecaoFases();
        menuIntermediarioAtivo = true;
    }

    public void FecharMenuIntermediario() {
        foreach (var menu in menusIntermediarios) {
            menu.Ocultar();
        }
        AtivarSelecaoFases();
        menuIntermediarioAtivo = false;
        inputLocked = true;
        StartCoroutine(UnlockInput());
    }

    public void CarregarFase(int numFase) {
        if (numFase > faseAtual + 1) {
            return;
        }

        faseAtual = Mathf.Max(numFase - 1, faseAtual);
        SalvarFase(faseAtual + 1);
        SceneManager.LoadScene("Fase" + numFase);
    }

    public void AtualizarBotoesFase() {
        for (int i = 0; i < botaoFase.Length; i++) {
            bool isCompleto = i < fasesCompletadas;
            botaoFase[i].interactable = isCompleto;
            botaoFase[i].GetComponent<Image>().sprite = isCompleto ? spritesCompletos[i] : spritesIncompletos[i];
        }
    }

    public void SalvarFase(int faseAtual) {
        PlayerPrefs.SetInt("FaseAtual", faseAtual);
        PlayerPrefs.Save();
    }

    private void CarregarProgresso() {
        DadosDoJogo dados = SistemadeSave.pegardados();
        if (dados != null) {
            fasesCompletadas = dados.fasesCompletadas;
            vidas = dados.vidas;
        } else {
            fasesCompletadas = 2;
            vidas = 6;
        }
        faseAtual = fasesCompletadas - 1;
    }

    public void SalvarProgresso() {
        try {
            DadosDoJogo dados = new DadosDoJogo(fasesCompletadas, vidas);
            SistemadeSave.SalvarDados(dados);
            PlayerPrefs.SetInt("FasesCompletadas", fasesCompletadas);
            PlayerPrefs.SetInt("Vidas", vidas);
            PlayerPrefs.Save();
            Debug.Log("Progresso salvo com sucesso");
        } catch (System.Exception e) {
            Debug.LogError("Erro ao salvar o progresso: " + e.Message);
        }
    }

    private void DesativarSelecaoFases() {
        foreach (var botao in botaoFase) {
            botao.interactable = false;
        }
    }

    private void AtivarSelecaoFases() {
        AtualizarBotoesFase();
    }

    private IEnumerator UnlockInput() {
        yield return new WaitForSeconds(0.2f);
        inputLocked = false;
    }
}

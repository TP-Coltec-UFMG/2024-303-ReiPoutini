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
    [SerializeField] private List<GameObject> colecionaveis;
    [SerializeField] private List<GameObject> Ilhas;
    [SerializeField] private GameObject Ilha4;

    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private GameObject painel_carregando;

    private Animator ilha4;
    private Animator animador;
    private SpriteRenderer spriteRenderer;
    public int faseAtual = 0;
    private bool isMoving = false;
    private int fasesCompletadas = 0;
    private int vidas = 6;
    private int andandoHash = Animator.StringToHash("Andando");
    private bool menuIntermediarioAtivo = false;
    private bool inputLocked = false;
    private List<bool> colecionaveisColetados = new List<bool>();

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
        ilha4 = Ilha4.GetComponent<Animator>();
    }

    void Update() {
        if (!menuIntermediarioAtivo && !inputLocked && !isMoving) {
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
        if (numFase < 0 || numFase >= colecionaveisColetados.Count) {
            Debug.LogError("Índice numFase fora dos limites: " + numFase);
            return;
        }

        if (numFase > faseAtual + 1) {
            return;
        }

        faseAtual = Mathf.Max(numFase - 1, faseAtual);
 
        if (numFase >= spritesCompletos.Count || numFase >= colecionaveis.Count) {
            Debug.LogError("Índice numFase fora dos limites para sprites: " + numFase);
            return;
        }

        bool colecionavelPego = colecionaveisColetados[numFase + 1];
        GameObject colecionavelObj = colecionaveis[numFase];
        GameObject Ilha = Ilhas[numFase];
        menusIntermediarios[faseAtual].ConfigurarMenu("Fase " + (numFase + 1), Ilha, colecionavelPego, colecionavelObj, () => CarregarFase(numFase + 1), FecharMenuIntermediario);
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
        if (numFase < 1 || numFase > botaoFase.Length) {
            Debug.LogError("indice numFase fora dos limites para carregamento: " + numFase);
            return;
        }

        if (numFase > faseAtual + 1) {
            return;
        }

        faseAtual = Mathf.Max(numFase - 1, faseAtual);
        SalvarFase(faseAtual + 1);
        painel_carregando.SetActive(true);
        sceneLoader.LoadScene("Fase" + numFase);
    }

    public void AtualizarBotoesFase() {
        for (int i = 0; i < botaoFase.Length; i++) {
            bool isCompleto = i < fasesCompletadas;
            botaoFase[i].interactable = isCompleto;
            botaoFase[i].GetComponent<Image>().sprite = isCompleto ? spritesCompletos[i] : spritesIncompletos[i];
        }
        
        if (botaoFase[3].GetComponent<Image>().sprite == spritesCompletos[3]) {
            ilha4.SetTrigger("Desbloqueado");
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
            colecionaveisColetados = dados.colecionaveisColetados;
           
            if (colecionaveisColetados.Count < botaoFase.Length) {
                colecionaveisColetados.AddRange(new bool[botaoFase.Length - colecionaveisColetados.Count]);
            }
        } else {
            fasesCompletadas = 2;
            vidas = 6;
            colecionaveisColetados = new List<bool>(new bool[botaoFase.Length]);
        }
        faseAtual = fasesCompletadas - 1;
    }

    public void SalvarProgresso() {
        try {
            DadosDoJogo dados = new DadosDoJogo(fasesCompletadas, vidas, colecionaveisColetados);
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GerenciarFases : MonoBehaviour {

    [SerializeField] private Button[] botaoFase;
    [SerializeField] private Sprite incompleto;
    [SerializeField] private Sprite completo;
    [SerializeField] private GameObject personagem;
    [SerializeField] private float velocidadeMovimento = 2f;

    private Animator animador;
    private SpriteRenderer spriteRenderer;
    public int faseAtual = 0;
    private bool isMoving = false;
    private int fasesCompletadas = 0;
    
    private int andandoHash = Animator.StringToHash("Andando");

    void Start() {
        CarregarProgresso();
        AtualizarBotoesFase();
        faseAtual = fasesCompletadas - 1;
        personagem.transform.position = botaoFase[faseAtual].transform.position;
    }

    void Awake(){
        animador = personagem.GetComponent<Animator>();
        spriteRenderer = personagem.GetComponent<SpriteRenderer>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            MoverParaFase(faseAtual + 1);
        } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            MoverParaFase(faseAtual - 1);
        } else if (Input.GetKeyDown(KeyCode.Return)) {
            SelecionaFase(faseAtual + 1);
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

    public void SelecionaFase(int numFase) {
        SalvarFase(numFase);
        SceneManager.LoadScene("Fase" + numFase);
    }

    public void AtualizarBotoesFase() {
        for (int i = 0; i < botaoFase.Length; i++) {
            bool isCompleto = i + 1 <= fasesCompletadas;
            botaoFase[i].interactable = isCompleto;
            botaoFase[i].GetComponent<Image>().sprite = isCompleto ? completo : incompleto;
        }
    }

    public void SalvarFase(int faseAtual){
        PlayerPrefs.SetInt("FaseAtual", faseAtual);
        PlayerPrefs.Save();
    }

    private void CarregarProgresso() {
        DadosDoJogo dados = SistemadeSave.pegardados();
        if (dados != null) {
            fasesCompletadas = dados.fasesCompletadas;
        } else {
            fasesCompletadas = 1;
        }
        faseAtual = fasesCompletadas - 1;
    }

    public void SalvarProgresso() {
        try {
            DadosDoJogo dados = new DadosDoJogo(fasesCompletadas);
            SistemadeSave.SalvarDados(dados);
            PlayerPrefs.SetInt("FasesCompletadas", fasesCompletadas);
            PlayerPrefs.Save();
            Debug.Log("Progresso salvo com sucesso");
        } catch (IOException e) {
            Debug.LogError("Erro ao salvar o progresso: " + e.Message);
        } catch (System.Runtime.Serialization.SerializationException e) {
            Debug.LogError("Erro de serializacao ao salvar o progresso: " + e.Message);
        }
    }
}

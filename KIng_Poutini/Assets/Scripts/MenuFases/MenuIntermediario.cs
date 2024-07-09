
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class MenuIntermediario : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI nomeFaseText;
    [SerializeField] private GameObject ilhaImage;
    [SerializeField] private GameObject colecionavelImage;
    [SerializeField] private GameObject Selecao;
    public Button botaoJogar;
    public Button botaoVoltar;

    private int botaoAtual = 0;
    private Button[] botoes;

    private Animator colecionavelAnimator;
    private Animator IlhaAnimation;
    private int AtivarRotacao = Animator.StringToHash("Ativarotacao");

    private void Awake() {
        colecionavelAnimator = colecionavelImage.GetComponent<Animator>();
        IlhaAnimation = ilhaImage.GetComponent<Animator>();
    }

    public void ConfigurarMenu(string nomeFase, GameObject spriteIlha, bool colecionavelPego, GameObject colecionavelObj, UnityAction jogarCallback, UnityAction voltarCallback) {
        nomeFaseText.text = nomeFase;
        spriteIlha.SetActive(true);
        IlhaAnimation = spriteIlha.GetComponent<Animator>();
       
        if (colecionavelPego) {
            colecionavelObj.SetActive(true);
            colecionavelAnimator = colecionavelObj.GetComponent<Animator>();
            if (colecionavelAnimator != null) {
                colecionavelAnimator.SetTrigger(AtivarRotacao);
            }
            if (colecionavelAnimator == null){
            }
        } else {
            if (colecionavelAnimator != null) {
            }
            colecionavelObj.SetActive(false);
        }

        botaoJogar.onClick.RemoveAllListeners();
        botaoVoltar.onClick.RemoveAllListeners();

        botaoJogar.onClick.AddListener(jogarCallback);
        botaoVoltar.onClick.AddListener(voltarCallback);
       
        botoes = new Button[] { botaoJogar, botaoVoltar };
        SelectButton(botaoAtual);
    }

    private void Update() {
        if (gameObject.activeSelf) {
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                botaoAtual = (botaoAtual + 1) % botoes.Length;
                SelectButton(botaoAtual);
            } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
                botaoAtual = (botaoAtual - 1 + botoes.Length) % botoes.Length;
                SelectButton(botaoAtual);
            }
        }
    }

    private void SelectButton(int index) {
        for (int i = 0; i < botoes.Length; i++) {
            ColorBlock colors = botoes[i].colors;
            if (i == index) {
                botoes[i].Select();
                colors.normalColor = colors.highlightedColor;
            } else {
                colors.normalColor = Color.white;
            }
            botoes[i].colors = colors;
        }
    }

    public void Mostrar() {
        gameObject.SetActive(true);
        Selecao.SetActive(false);
    }

    public void Ocultar() {
        gameObject.SetActive(false);
        Selecao.SetActive(true);
    }
}
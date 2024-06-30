using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class MenuIntermediario : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI nomeFaseText;
    [SerializeField] private Image ilhaImage;
    [SerializeField] private Image colecionavelImage;
    [SerializeField] private GameObject Selecao;
    public Button botaoJogar;
    public Button botaoVoltar;

    private int botaoAtual = 0;
    private Button[] botoes;

    private Animator colecionavelAnimator;
    private static readonly int AtivarRotacao = Animator.StringToHash("Ativarotacao");

    private void Awake() {
        colecionavelAnimator = colecionavelImage.GetComponent<Animator>();
    }

    public void ConfigurarMenu(string nomeFase, Sprite spriteIlha, bool colecionavelPego, Sprite spriteColecionavel, UnityAction jogarCallback, UnityAction voltarCallback) {
        nomeFaseText.text = nomeFase;
        ilhaImage.sprite = spriteIlha;
        
        if (colecionavelPego) {
            colecionavelImage.sprite = spriteColecionavel;
            colecionavelAnimator.SetTrigger(AtivarRotacao);
        } else {
            colecionavelImage.sprite = null;
            colecionavelAnimator.ResetTrigger(AtivarRotacao);
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

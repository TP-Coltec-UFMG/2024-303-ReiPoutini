using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuJogando : MonoBehaviour {

    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject painel_opcoes;
    [SerializeField] private List<Selectable> Botoes_pause;
    [SerializeField] private List<Selectable> Botoes_opcoes;
    [SerializeField] private GameObject Vidas;

    private bool taPausado = false;
    private int BotaoAtual = 0;
    private List<Selectable> Botoes;

    void Awake() {
        Botoes = Botoes_pause;
        SelectButton(BotaoAtual);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            if (taPausado) {
                Continuar();
            } else {
                Pausar();
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && taPausado) {
            BotaoAtual = (BotaoAtual + 1) % Botoes.Count;
            SelectButton(BotaoAtual);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && taPausado) {
            BotaoAtual = (BotaoAtual - 1 + Botoes.Count) % Botoes.Count;
            SelectButton(BotaoAtual);
        }

        if (painel_opcoes.activeSelf && taPausado) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Voltar();
            }
        }
    }

    private void SelectButton(int index) {
        Botoes[index].Select();
    }

    public void Continuar() {
        pauseMenuUI.SetActive(false);
        painel_opcoes.SetActive(false);
        Vidas.SetActive(true);
        Time.timeScale = 1f;
        taPausado = false;
    }

    public void Opcoes() {
        pauseMenuUI.SetActive(false);
        painel_opcoes.SetActive(true);
        Vidas.SetActive(false);
        BotaoAtual = 0;
        Botoes = Botoes_opcoes;
        SelectButton(BotaoAtual);
    }

    private void Pausar() {
        pauseMenuUI.SetActive(true);
        Vidas.SetActive(false);
        Time.timeScale = 0f;
        taPausado = true;
        Botoes = Botoes_pause;
        SelectButton(BotaoAtual);
    }

    public void Voltar() {
        painel_opcoes.SetActive(false);
        pauseMenuUI.SetActive(true);
        BotaoAtual = 0;
        Botoes = Botoes_pause;
        SelectButton(BotaoAtual);
    }

    public void Sair() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuFases");
    }
}
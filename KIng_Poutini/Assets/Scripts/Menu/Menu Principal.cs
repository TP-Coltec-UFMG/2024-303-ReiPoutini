using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using System.Linq;

public class MenuPrincipal : MonoBehaviour {
    
    [SerializeField] private string Jogar_jogo;
    [SerializeField] private GameObject painel_menu;
    [SerializeField] private GameObject painel_opcoes;
    [SerializeField] private GameObject painel_jogar;
    [SerializeField] private TextMeshProUGUI progressoText;
    [SerializeField] private List<Selectable> Botoes_menu;
    [SerializeField] private List<Selectable> Botoes_opcoes;
    [SerializeField] private List<Selectable> Botoes_jogar;
    
    [SerializeField] private int totalDeFases = 16;
    private int vidasIniciais = 6;

    private int BotaoAtual = 0;
    private List<Selectable> Botoes;
    private int fasesCompletadas = 1;
    private int vidas = 6;
    

    private void Awake() {
        Botoes = Botoes_menu;
        SelectButton(BotaoAtual);
        CarregarProgresso();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.DownArrow)){
            BotaoAtual = (BotaoAtual + 1) % Botoes.Count;
            SelectButton(BotaoAtual);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)){
            BotaoAtual = (BotaoAtual - 1 + Botoes.Count) % Botoes.Count;
            SelectButton(BotaoAtual);
        }

        if (painel_opcoes.activeSelf){
            if (Input.GetKeyDown(KeyCode.Escape)){
                Voltar();
            }
        }

        if (painel_jogar.activeSelf){
            if (Input.GetKeyDown(KeyCode.Escape)){
                Voltar();
            }
        }
    }

    private void SelectButton(int index) {
        Botoes[index].Select();
    }

    public void Jogar() {
        painel_menu.SetActive(false);
        painel_jogar.SetActive(true);
        painel_opcoes.SetActive(false);
        BotaoAtual = 0;
        Botoes = Botoes_jogar;
        SelectButton(BotaoAtual);
        CarregarProgresso();
    }

    public void Opcoes() {
        painel_menu.SetActive(false);
        painel_opcoes.SetActive(true);
        painel_jogar.SetActive(false);
        BotaoAtual = 0;
        Botoes = Botoes_opcoes;
        SelectButton(BotaoAtual);
    }

    public void Voltar() {
        if (painel_opcoes.activeSelf){
            painel_opcoes.SetActive(false);
            painel_menu.SetActive(true);
            painel_jogar.SetActive(false);
        } else if (painel_jogar.activeSelf){
            painel_jogar.SetActive(false);
            painel_menu.SetActive(true);
            painel_opcoes.SetActive(false);
        }
        BotaoAtual = 0;
        Botoes = Botoes_menu;
        SelectButton(BotaoAtual);
    }

    public void Sair() {
        Debug.Log("Sair do jogo");
        Application.Quit();
    }

    public void NovoJogo() {
        LimparDados();

        fasesCompletadas = 1;
        vidas = vidasIniciais;
        int numeroDeColecionaveis = 16;
        List<bool> colecionaveis = new List<bool>(new bool[numeroDeColecionaveis]);

        DadosDoJogo dados = new DadosDoJogo(fasesCompletadas, vidasIniciais, colecionaveis);
        Debug.Log("Fases completadas: " + dados.fasesCompletadas);
        SistemadeSave.SalvarDados(dados);

        SceneManager.LoadScene("Fase1");
    }


    public void CarregarJogo() {
        DadosDoJogo dados = SistemadeSave.pegardados();
        if (dados != null) {
            PlayerPrefs.SetInt("FasesCompletadas", dados.fasesCompletadas);
            Debug.Log("Jogo carregado com progresso: " + dados.fasesCompletadas + " fases, " + dados.vidas + " vidas.");
            SceneManager.LoadScene("MenuFases");
        } else {
            Debug.Log("Nenhum jogo salvo encontrado");
        }
    }

    private void CarregarProgresso() {
        DadosDoJogo dados = SistemadeSave.pegardados();
        if (dados != null) {
            fasesCompletadas = dados.fasesCompletadas;
            vidas = dados.vidas;
            progressoText.text = "Progresso: " + CalcularProgresso() + "%";
        } else {
            progressoText.text = "Nenhum jogo salvo";
            fasesCompletadas = 0;
        }
    }

    private int CalcularProgresso() {
        DadosDoJogo dados = SistemadeSave.pegardados();
        if (dados != null) {
            int totalColecionaveis = 12; // Ajuste conforme necessário
            int coletados = dados.colecionaveisColetados.Count(c => c); // Conta o número de verdadeiros
            return (int)((float)(fasesCompletadas + coletados) / (totalDeFases + totalColecionaveis) * 100);
        } else {
            return 0;
        }
    }

    private void LimparDados() {
        string path = Application.persistentDataPath + "/save.dat";
        if (File.Exists(path)) {
            try {
                File.Delete(path); // Deleta o arquivo de dados salvo
                PlayerPrefs.SetInt("FasesCompletadas", 0);
                PlayerPrefs.Save(); // Salva as mudanças no PlayerPrefs
                PlayerPrefs.SetInt("FaseAtual", 1);
                PlayerPrefs.Save(); // Salvar as mudanças feitas nos PlayerPrefs
                Debug.Log("Dados limpos com sucesso");
            } catch (IOException e) {
                Debug.Log("Erro ao limpar dados");
            }
        }
    }

}

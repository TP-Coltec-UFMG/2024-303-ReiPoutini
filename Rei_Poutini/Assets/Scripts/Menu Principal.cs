using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincial : MonoBehaviour{

    [SerializeField] private string Jogar_jogo;
    [SerializeField] private GameObject painel_menu;
    [SerializeField] private GameObject painel_opcoes;


    public void Jogar(){
        SceneManager.LoadScene("Jogo");
    }

    public void Opcoes(){
        painel_menu.SetActive(false);
        painel_opcoes.SetActive(true);
    }
    public void Voltar(){
        painel_menu.SetActive(true);
        painel_opcoes.SetActive(false);
    }

    public void sair(){
        Debug.Log("Sair do jogo");
        Application.Quit();
    }
    
}

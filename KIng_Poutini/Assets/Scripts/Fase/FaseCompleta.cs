using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FaseCompleta : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Parede")) {
            int faseAtual = PlayerPrefs.GetInt("FaseAtual", 1);
            
            DadosDoJogo dados = SistemadeSave.pegardados();
            int fasesCompletadas = dados != null ? dados.fasesCompletadas : 0;
            int vidas = dados != null ? dados.vidas : 6; 
            List<bool> colecionaveisColetados = dados != null ? dados.colecionaveisColetados : new List<bool>();

            Debug.Log("Fases completadas antes do incremento: " + fasesCompletadas);
            if (faseAtual >= fasesCompletadas) {
                fasesCompletadas = faseAtual + 1;
                dados = new DadosDoJogo(fasesCompletadas, vidas, colecionaveisColetados);
                SistemadeSave.SalvarDados(dados);

                Debug.Log("Fase atual: " + faseAtual);
                Debug.Log("Fases completadas após incremento: " + fasesCompletadas); 
            }
            
            Debug.Log("Missão Concluída!");
            SceneManager.LoadScene("MenuFases");
        }
    }
}

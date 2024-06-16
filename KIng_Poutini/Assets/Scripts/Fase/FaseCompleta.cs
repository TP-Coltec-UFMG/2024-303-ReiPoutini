using UnityEngine;
using UnityEngine.SceneManagement;

public class FaseCompleta : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Parede") {
            DadosDoJogo dados = SistemadeSave.pegardados();
            int fasesCompletadas = dados != null ? dados.fasesCompletadas : 1;
            
            int faseAtual = PlayerPrefs.GetInt("FaseAtual", 0);

            if (faseAtual >= fasesCompletadas) {
                fasesCompletadas = faseAtual + 1;
                dados = new DadosDoJogo(fasesCompletadas);
                SistemadeSave.SalvarDados(dados);
            }
            Debug.Log("Missão Concluída!");
            SceneManager.LoadScene("MenuFases");
        }
    }
}
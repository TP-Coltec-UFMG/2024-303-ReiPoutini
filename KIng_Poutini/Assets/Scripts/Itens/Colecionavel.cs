using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colecionavel : MonoBehaviour {
    HUDControl HControl;

    void Start() {
        HControl = HUDControl.HControl;
        if (HControl == null) {
            Debug.LogError("HUDControl não encontrado!");
        }

        VerificarSeColecionavelJaFoiPegos();
    }

    private void OnTriggerEnter2D(Collider2D colisao) {
        if (colisao.gameObject.CompareTag("Jogador")) {
            AdicionarColecionavelAoSave(gameObject.name);
            Destroy(gameObject);
        }
    }

    private void VerificarSeColecionavelJaFoiPegos() {
        DadosDoJogo dados = SistemadeSave.pegardados();
        if (dados != null) {
            int index = ObterIndiceColecionavel();
            if (index >= 0 && index < dados.colecionaveisColetados.Count) {
                if (dados.colecionaveisColetados[index]) {
                    gameObject.SetActive(false);
                }
            }
        }
    }

    private int ObterIndiceColecionavel() {
        int index;
        if (int.TryParse(gameObject.name.Replace("ColecionavelFase", ""), out index)) {
            return index;
        } else {
            Debug.LogError("Nome do colecionável inválido: " + gameObject.name);
            return -1;
        }
    }

    private void AdicionarColecionavelAoSave(string colecionavelNome) {
        DadosDoJogo dados = SistemadeSave.pegardados();
        int index = ObterIndiceColecionavel();
        if (dados != null) {
            if (index >= 0 && index < dados.colecionaveisColetados.Count && !dados.colecionaveisColetados[index]) {
                dados.colecionaveisColetados[index] = true;
                SistemadeSave.SalvarDados(dados);
            }
        } else {
            List<bool> colecionaveis = new List<bool>(new bool[12]);
            if (index >= 0 && index < colecionaveis.Count) {
                colecionaveis[index] = true;
            }
            dados = new DadosDoJogo(0, 0, colecionaveis);
            SistemadeSave.SalvarDados(dados);
        }
    }
}

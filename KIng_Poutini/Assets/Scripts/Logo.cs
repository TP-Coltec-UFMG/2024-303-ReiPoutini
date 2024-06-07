using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private float tempoNaTela;
    [SerializeField] private float tempoFade;
    [SerializeField] private GameObject[] objetosAtivadosNoFim;

    private SpriteRenderer SpriteR;

    private float cronometro;

    private int estagioAtual;
    //1 = Fade in
    //2 = Mostra a logo parada
    //3 = Fade out

    void Awake()
    {
        this.SpriteR = this.GetComponent<SpriteRenderer>();
        this.estagioAtual = 1;
        this.cronometro = 0;

    }


    private void avancarEstagio(){
        estagioAtual = estagioAtual + 1;
        if(estagioAtual == 4){
            this.finalizar();
        }
    }
    

    // Metodo chamado quando acaba o tempo da logo na tela
    private void finalizar(){
        foreach(GameObject objeto in objetosAtivadosNoFim){
            objeto.SetActive(true);
        }
        Object.Destroy(this);
    }

    void Update()
    {
        if(estagioAtual == 2){
            this.cronometro = this.cronometro + (Time.deltaTime / tempoNaTela);
        } else{
            this.cronometro = this.cronometro + (Time.deltaTime / tempoFade);
        }

        if(estagioAtual == 1){
            this.SpriteR.color = new Color(1,1,1,this.cronometro) ;
        } else
        if(estagioAtual == 3){
            this.SpriteR.color = new Color(1,1,1,(1 - this.cronometro));
        }

        if(cronometro >= 1){ 
            this.cronometro = 0;
            avancarEstagio();
        }
    }
}

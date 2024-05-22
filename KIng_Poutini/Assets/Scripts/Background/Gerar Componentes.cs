using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerarComponentes : MonoBehaviour
{
    [SerializeField]private GameObject componente;
    [SerializeField]private float IntervaloDeGeracao;
    private float cronometro;


    void InstanciarComponente(){
        GameObject.Instantiate(this.componente, this.transform.position, Quaternion.identity);
    }

    void Awake(){
        InstanciarComponente();
        this.cronometro = this.IntervaloDeGeracao;
    }

    void Update(){
        this.cronometro = this.cronometro - Time.deltaTime;
        if(this.cronometro <= 0){
            InstanciarComponente();
            this.cronometro = this.IntervaloDeGeracao;
        }
    }
}

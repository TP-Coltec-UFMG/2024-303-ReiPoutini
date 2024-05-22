using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover_Constantemente : MonoBehaviour
{
    [SerializeField] private float velocidade;
    [SerializeField] private int variaçãoDeAlturaPx;
    private float distanciaMovida;
    void Awake()
    {
        //Variar a posição no eixo y de acordo com o valor fornecido
        this.transform.Translate(new Vector3(0, Random.Range(variaçãoDeAlturaPx * -1, variaçãoDeAlturaPx + 1) * 0.04166667f, 0));
        
        //Mover no eixo z um valor aleatorio (para os sprites não se sobreporem)
        this.transform.Translate(new Vector3(0, 0, Random.Range(-0.01f, 0.01f)));
    }

    void Update()
    {
        this.transform.Translate(Vector3.left * velocidade * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D outro){
        GameObject.Destroy(this.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRandom : MonoBehaviour{
    public GameObject item;
    public int RandomItem;

    void Start(){
        DropItem();
    }

    void DropItem(){
        RandomItem = Random.Range(0,10);
        
        //Quanto menor o número, maior a chance de vir o item 
        if(RandomItem >= 3){
            Instantiate(item, gameObject.transform.position,transform.rotation);
        }


    }
}
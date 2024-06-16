using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class SpriteAleatorio : MonoBehaviour{
    
    [SerializeField] private Sprite[] Sprites;

    void Awake(){
        this.GetComponent<SpriteRenderer>().sprite = Sprites[Random.Range(0, Sprites.Length)];
    } 
}
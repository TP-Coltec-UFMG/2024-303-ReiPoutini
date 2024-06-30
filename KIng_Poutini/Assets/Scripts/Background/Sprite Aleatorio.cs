using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class SpriteAleatorio : MonoBehaviour{
    
    [SerializeField] private Sprite[] Sprites;
    [SerializeField] private bool Randomizar_FlipX;
    [SerializeField] private bool Randomizar_FlipY;
    private SpriteRenderer SpriteR;
    
    private bool IntToBool(int value){if(value==1){return true;}else{return false;}}

    void Awake(){
        this.SpriteR = this.GetComponent<SpriteRenderer>();
        
        this.SpriteR.sprite = Sprites[Random.Range(0, Sprites.Length)];

        if(this.Randomizar_FlipX == true){this.SpriteR.flipX = IntToBool(Random.Range(0, 2));}
        if(this.Randomizar_FlipY == true){this.SpriteR.flipY = IntToBool(Random.Range(0, 2));}
    } 
}
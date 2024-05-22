using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class SpriteAleatorio : MonoBehaviour
{
    [SerializeField] private string pastaComSprites;
    [SerializeField] private int numeroSprites;
    private SpriteRenderer SpriteR;

    void Awake() {
        this.SpriteR = this.GetComponent<SpriteRenderer>();

        int spriteEscolhido = Random.Range(1, numeroSprites + 1);
        Debug.Log($"{pastaComSprites} {spriteEscolhido}");
        SpriteR.sprite = Resources.Load<Sprite>($"{pastaComSprites} {spriteEscolhido}");
    }
}
//
//"Background/Imediato/Background Imediato 4"
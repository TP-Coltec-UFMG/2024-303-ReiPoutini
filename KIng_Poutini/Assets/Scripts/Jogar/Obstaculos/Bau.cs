using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bau : MonoBehaviour{
    public SpriteRenderer sprites;
    public GameObject ItemPrefab;
    public List<Sprite> BauSprites;
    public BoxCollider2D Bc;
    public Transform posicao;

    [SerializeField] private int vida = 2;

    void Start(){
        sprites = GetComponent<SpriteRenderer>();
        Bc = GetComponent<BoxCollider2D>();
    }
    
    public void TomaDano() {
        if (vida > 0) {
            vida--;
            TrocaSprite();
        }
        if (vida <= 0){
            Destroy(Bc);
            DropItem();
        }
    }

    private void DropItem() { 
        if (vida <= 0) {
            Transform dropPosition = posicao;
            GameObject item = Instantiate(ItemPrefab, dropPosition.position, Quaternion.identity);
        }
    } 
    
    private void TrocaSprite() {
        if (vida >= 0 && vida < BauSprites.Count) {
            sprites.sprite = BauSprites[vida];
        }
    }

    private void OnTriggerEnter2D(Collider2D colisao){
        if (colisao.gameObject.tag == "Soco") {
            TomaDano();
        }
    }
}

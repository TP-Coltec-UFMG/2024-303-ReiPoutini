using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arvore : MonoBehaviour {
    public Animator anima;
    public SpriteRenderer sprites;
    public GameObject ItemPrefab;
    public GameObject FolhasCaindo;
    public List<Sprite> treeSprites;

    public Transform[] fruitDropPositions;

    [SerializeField] private int vida = 3;

    void Start() {
        anima = GetComponent<Animator>();
        sprites = GetComponent<SpriteRenderer>();
        FolhasCaindo.SetActive(false);
    }

    public void TomaDano() {
        if (vida > 0) {
            vida--;
            DropItem();
            NovoSprite();
            AtivarFolhasCaindo();
        }
        if (vida <= 0) {
            Debug.Log("A árvore não pode mais dropar frutas.");
            AtivarFolhasCaindo();
        }
    }

    private void DropItem() { 
        if (vida > 0 && vida < fruitDropPositions.Length) {
            Transform dropPosition = fruitDropPositions[vida];
            GameObject item = Instantiate(ItemPrefab, dropPosition.position, Quaternion.identity);
        }
    } 

    private void NovoSprite() {
        if (vida >= 0 && vida < treeSprites.Count) {
            sprites.sprite = treeSprites[vida];
        }
    }

    private void AtivarFolhasCaindo() {
        FolhasCaindo.SetActive(true);
        float animationDuration = GetFolhasCaindoAnimationDuration();
        StartCoroutine(DesativarFolhasCaindo(animationDuration));
    }

    private float GetFolhasCaindoAnimationDuration() {
        AnimatorClipInfo[] clipInfo = FolhasCaindo.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0);
        return clipInfo.Length > 0 ? clipInfo[0].clip.length : 0f;
    }

    private IEnumerator DesativarFolhasCaindo(float delay) {
        yield return new WaitForSeconds(delay);
        FolhasCaindo.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D colisao) {
        if (colisao.gameObject.tag == "Soco") {
            TomaDano();
        }
    }
}

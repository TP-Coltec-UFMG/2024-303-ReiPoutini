using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Plataforma : MonoBehaviour {
    private TilemapCollider2D platformCollider;
    private Jogador jogador;
    [SerializeField] private float TempoDesativado = 5.0f;

    private void Start() {
        platformCollider = GetComponent<TilemapCollider2D>();
    }

    private void Update() {
        if (jogador == null) {
            jogador = FindObjectOfType<Jogador>();
            if (jogador != null) {
                jogador.OnPlayerJump += DesativarCollider;
            }
        }
    }

    private void OnDestroy() {
        if (jogador != null) {
            jogador.OnPlayerJump -= DesativarCollider;
        }
    }

    private void DesativarCollider() {
        platformCollider.enabled = false;
        Debug.Log("Colisor desativado");
        Invoke("ReativarCollider", TempoDesativado);
    }

    private void ReativarCollider() {
        platformCollider.enabled = true;
        Debug.Log("Colisor reativado");
    }
}

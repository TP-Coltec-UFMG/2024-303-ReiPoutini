using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toca : MonoBehaviour {
    public GameObject Inimigo;
    public int InimigoMax = 3;
    public float SpawnTempo = 5f;
    public Transform SpawnPoint; // Referência ao ponto de spawn

    private List<GameObject> ContInimigo = new List<GameObject>();

    void Start() {
        StartCoroutine(SpawnInimigosComDelay());
    }

    void Update() {
        for (int i = ContInimigo.Count - 1; i >= 0; i--) {
            if (ContInimigo[i] == null) {
                ContInimigo.RemoveAt(i);
                StartCoroutine(SpawnComDelay());
            }
        }
    }

    private void Spawn() {
        GameObject NovoInimigo = Instantiate(Inimigo, SpawnPoint.position, Quaternion.identity);
        ContInimigo.Add(NovoInimigo);
    }

    private IEnumerator SpawnComDelay() {
        yield return new WaitForSeconds(SpawnTempo);
        Spawn();
    }

    private IEnumerator SpawnInimigosComDelay() {
        for (int i = 0; i < InimigoMax; i++) {
            Spawn();
            yield return new WaitForSeconds(SpawnTempo);
        }
    }
}

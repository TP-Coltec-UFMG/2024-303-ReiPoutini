using UnityEngine;
using UnityEngine.SceneManagement;

public class  ConcluirTutorial : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {

        if (other.gameObject.tag == "Parede") {
            SceneManager.LoadScene("MenuFases");
        }

    }
}

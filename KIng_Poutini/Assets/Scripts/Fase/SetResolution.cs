using UnityEngine;

public class SetResolution : MonoBehaviour {
    void Start() {
        // Tentar definir a resolucao para 1800x1440, mas ajustar se necessario
        int targetWidth = 1800;
        int targetHeight = 1440;

        // Verificar se a tela suporta a resolucao desejada
        if (Screen.currentResolution.width >= targetWidth && Screen.currentResolution.height >= targetHeight) {
            Screen.SetResolution(targetWidth, targetHeight, false);
        } else {
            // Ajustar para a resolucao maxima suportada
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, false);
            Debug.LogWarning("A resolucao desejada nao e suportada pelo monitor atual. Ajustando para a resolucao maxima suportada.");
        }
    }
}

using UnityEngine;

public class SetResolution : MonoBehaviour {
    void Start() {
        // Tentar definir a resolução para 1800x1440, mas ajustar se necessário
        int targetWidth = 1800;
        int targetHeight = 1440;

        // Verificar se a tela suporta a resolução desejada
        if (Screen.currentResolution.width >= targetWidth && Screen.currentResolution.height >= targetHeight) {
            Screen.SetResolution(targetWidth, targetHeight, false);
        } else {
            // Ajustar para a resolução máxima suportada
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, false);
            Debug.LogWarning("A resolução desejada não é suportada pelo monitor atual. Ajustando para a resolução máxima suportada.");
        }
    }
}

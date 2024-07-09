using UnityEngine;
using UnityEngine.UI;
using BrewedInk.CRT;

public class ConfigMenu : MonoBehaviour{
    public static ConfigMenu Instance;

    public Toggle crtToggle;
    public Slider volumeSlider;

    private void Awake(){
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
            AplicarConfiguracoes();
        }
        else{
            Destroy(gameObject);
        }
    }

    private void OnEnable(){
        AplicarConfiguracoes();
    }

    public void AplicarConfiguracoes(){
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1);
        AudioListener.volume = volumeSlider.value;

        bool crtEnabled = PlayerPrefs.GetInt("CRTCamera", 0) == 1;
        crtToggle.isOn = crtEnabled;
        Camera.main.GetComponent<CRTCameraBehaviour>().enabled = crtEnabled;
    }

    public void AtualizarVolume(){
        float volume = volumeSlider.value;
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
    }

    public void AtualizarCRTCamera(){
        bool crtEnabled = crtToggle.isOn;
        Camera.main.GetComponent<CRTCameraBehaviour>().enabled = crtEnabled;
        PlayerPrefs.SetInt("CRTCamera", crtEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }
}
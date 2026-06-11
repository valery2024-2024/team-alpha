using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("Панель налаштувань")]
    public GameObject settingsPanel;

    [Header("Слайдер гучності")]
    public Slider volumeSlider;

    void Start()
    {
        settingsPanel.SetActive(false);

        float volume = PlayerPrefs.GetFloat("Volume", 1f);
        AudioListener.volume = volume;

        volumeSlider.value = volume;
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void ChangeVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
    }
}
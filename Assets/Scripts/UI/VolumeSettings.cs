using UnityEngine;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    public Slider volumeSlider;

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);

        volumeSlider.value = savedVolume;
        AudioListener.volume = savedVolume;

        volumeSlider.onValueChanged.AddListener(ChangeVolume);
    }

    public void ChangeVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
    }
}
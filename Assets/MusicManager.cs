using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource musicSource;

    private const string MUSIC_KEY = "MUSIC_ON";
    private const string VOLUME_KEY = "MUSIC_VOLUME";

    private bool musicOn = true;

    [Range(0f, 1f)]
    public float volume = 1f;

    void Start()
    {
        // Завантаження стану музики
        musicOn = PlayerPrefs.GetInt(MUSIC_KEY, 1) == 1;

        // Завантаження гучності
        volume = PlayerPrefs.GetFloat(VOLUME_KEY, 1f);

        ApplyState();
    }

    public void ToggleMusic()
    {
        musicOn = !musicOn;

        PlayerPrefs.SetInt(MUSIC_KEY, musicOn ? 1 : 0);
        PlayerPrefs.Save();

        ApplyState();
    }

    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);

        PlayerPrefs.SetFloat(VOLUME_KEY, volume);
        PlayerPrefs.Save();

        ApplyState();
    }

    void ApplyState()
    {
        if (musicSource == null)
            return;

        musicSource.mute = !musicOn;
        musicSource.volume = volume;
    }
}
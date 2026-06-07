using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    [Header("Звук кнопки")]
    public AudioClip clickSound;

    private Button button;
    private AudioSource audioSource;

    void Start()
    {
        button = GetComponent<Button>();

        audioSource = FindFirstObjectByType<AudioSource>();

        if (button != null)
        {
            button.onClick.AddListener(PlayClickSound);
        }
    }

    void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
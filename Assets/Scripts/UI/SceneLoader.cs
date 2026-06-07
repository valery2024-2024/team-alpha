using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadSceneByName(string sceneName)
    {
        Debug.Log($"[SceneManager] Loading scene {sceneName}...");
        SceneManager.LoadScene(sceneName);
    }
}

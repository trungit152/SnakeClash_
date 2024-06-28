using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeUI : MonoBehaviour
{
    public string sceneName1;
    public string sceneName2;

    private static SnakeUI instance = null;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != sceneName1 && scene.name != sceneName2)
        {
            Destroy(gameObject);
        }
    }
}
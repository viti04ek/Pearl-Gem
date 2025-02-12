using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public const string GameSceneName = "Game";
    public const string MenuSceneName = "Menu";
    public const string CollectionSceneName = "Collection";
    
    private void Awake()
    {
        
        if (Services.DataManager == null)
        {
            Services.Register(this);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void ReLoadScene()
    {
        LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public enum MyScenes
{
    InitScene,
    MainGame
}
public class SceneLoader : MonoBehaviour
{
    #region Singleton
    public static SceneLoader Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    [SerializeField] private MyScenes startScene = MyScenes.MainGame;

    private void Start()
    {
        LoadScene(startScene);
    }
    public void LoadScene(MyScenes scene, LoadSceneMode _loadSceneMode = LoadSceneMode.Single)
    {
        SceneManager.LoadScene((int)scene, _loadSceneMode);
    }
    public void UnloadScene(MyScenes scene)
    {
        if (SceneManager.GetSceneByName(scene.ToString()).isLoaded)
        {
            SceneManager.UnloadSceneAsync((int)scene);
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class SeedEmptyBehavior : MonoBehaviour
{
    
    void Awake()
    {
        DontDestroyOnLoad(gameObject); // 保证不销毁
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded; // 注册事件
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "WellScene")
        {
            gameObject.SetActive(false);
        }
        else if (scene.name == "FarmScene")
        {
            gameObject.SetActive(true);
        }
    }

    void OnDestroy()
    {
        // 避免事件重复绑定
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

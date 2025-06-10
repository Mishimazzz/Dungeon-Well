using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    private Button switchSceneRightButton;
    private Button switchSceneLeftButton;
    public SceneManager sceneManager;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (sceneManager == null)
            sceneManager = FindObjectOfType<SceneManager>();
        BindSwitchSceneRightButton();
        BindSwitchSceneLeftButton();
    }

    public void BindSwitchSceneRightButton()
    {
        GameObject timeObject = GameObject.Find("Time");
        if (timeObject == null) return;

        Transform btnTransform = timeObject.transform.Find("SwitchSceneRight");
        if (btnTransform == null) return;

        switchSceneRightButton = btnTransform.GetComponent<Button>();
        if (switchSceneRightButton == null) return;

        switchSceneRightButton.onClick.RemoveAllListeners();
        switchSceneRightButton.onClick.AddListener(SceneSwitchRightMethod);
    }

    public void BindSwitchSceneLeftButton()
    {
        GameObject timeObject = GameObject.Find("Time");
        if (timeObject == null) return;


        Transform btnTransform = timeObject.transform.Find("SwitchSceneLeft");
        if (btnTransform == null) return;

        switchSceneLeftButton = btnTransform.GetComponent<Button>();
        if (switchSceneLeftButton == null) return;

        switchSceneLeftButton.onClick.RemoveAllListeners();
        switchSceneLeftButton.onClick.AddListener(SceneSwitchLeftMethod);
    }

    private void SceneSwitchRightMethod()
    {
        if (sceneManager != null) sceneManager.LoadScene();
        else Debug.LogError("SceneManager missing");
    }

    private void SceneSwitchLeftMethod()
    {
        if (sceneManager != null)
            sceneManager.LoadScene();
        else
            Debug.LogError("SceneManager missing");
    }
}
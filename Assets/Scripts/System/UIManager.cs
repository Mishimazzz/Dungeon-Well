using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    private Button switchSceneRightButton;
    private Button switchSceneLeftButton;
    private Button exploreButton;
    private Button startButton;
    private Button timeUpButton;

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
        BindExploreButton();
        BindStartButton();
        BindTimeUpButton();
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

    public void BindExploreButton()
    {
        GameObject timeObject = GameObject.Find("Time");
        if (timeObject == null)
        {
            Debug.LogError("BindExploreButton: 找不到 Time 对象");
            return;
        }

        Transform btnTransform = timeObject.transform.Find("Explore Button");
        if (btnTransform == null)
        {
            Debug.LogError("BindExploreButton: 在 Time 下找不到 ExploreButton");
            return;
        }

        exploreButton = btnTransform.GetComponent<Button>();
        if (exploreButton == null)
        {
            Debug.LogError("BindExploreButton: ExploreButton 没有 Button 组件");
            return;
        }

        exploreButton.onClick.RemoveAllListeners();
        exploreButton.onClick.AddListener(ExploreButtonMethod);
        // Debug.Log("BindExploreButton: 绑定成功");
    }

    public void BindStartButton()
    {
        GameObject timeObject = GameObject.Find("Time");
        if (timeObject == null)
        {
            Debug.LogError("BindStartButton: 找不到 Time 对象");
            return;
        }

        Transform timePanelTransform = timeObject.transform.Find("TimePanel");
        if (timePanelTransform == null)
        {
            Debug.LogError("BindStartButton: 在 Time 下找不到 TimePanel");
            return;
        }

        Transform btnTransform = timePanelTransform.Find("StartButton");
        if (btnTransform == null)
        {
            Debug.LogError("BindStartButton: 在 TimePanel 下找不到 StartButton");
            return;
        }

        startButton = btnTransform.GetComponent<Button>();
        if (startButton == null)
        {
            Debug.LogError("BindStartButton: StartButton 没有 Button 组件");
            return;
        }

        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(StartButtonMethod);
    }

    public void BindTimeUpButton()
    {
        GameObject timeObject = GameObject.Find("Time");
        if (timeObject == null)
        {
            Debug.LogError("BindTimeUpButton: 找不到 Time 对象");
            return;
        }

        Transform timeUpPanelTransform = timeObject.transform.Find("TimeUpPanel");
        if (timeUpPanelTransform == null)
        {
            Debug.LogError("BindTimeUpButton: 在 Time 下找不到 TimeUpPanel");
            return;
        }

        Transform btnTransform = timeUpPanelTransform.Find("Button");
        if (btnTransform == null)
        {
            Debug.LogError("BindTimeUpButton: 在 TimeUpPanel 下找不到 Button");
            return;
        }

        timeUpButton = btnTransform.GetComponent<Button>();
        if (timeUpButton == null)
        {
            Debug.LogError("BindTimeUpButton: Button 没有 Button 组件");
            return;
        }

        timeUpButton.onClick.RemoveAllListeners();
        timeUpButton.onClick.AddListener(TimeUpButtonMethod);
        // Debug.Log("BindTimeUpButton: 绑定成功");
    }

    private void TimeUpButtonMethod()
    {
        var buttonManage = FindObjectOfType<ButtonManage>();
        if (buttonManage != null)
        {
            buttonManage.TimeUpButtom();
        }
        else
        {
            Debug.LogError("ButtonManage missing");
        }
    }

    private void StartButtonMethod()
    {
        var buttonManage = FindObjectOfType<ButtonManage>();
        if (buttonManage != null)
        {
            buttonManage.StartButton();
        }
        else
        {
            Debug.LogError("ButtonManage missing");
        }
    }

    private void ExploreButtonMethod()
    {
        var buttonManage = FindObjectOfType<ButtonManage>();
        if (buttonManage != null)
        {
            // Debug.Log("you reach here2");
            buttonManage.ExploreButton();
        }
        else
            Debug.LogError("ButtonManage missing");
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
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class TimerSaveBarSkinSwitcher : MonoBehaviour
{
    public Sprite wellBoardSprite;
    public Sprite wellPanel;
    public Sprite farmBoardSprite;
    public Sprite farmPanel;

    public GameObject farmSkin;
    public GameObject wellSkin;

    string lastScene = "";

    void Update()
    {
        string scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (scene != lastScene)
        {
            lastScene = scene;
            SwitchSkin(scene == "WellScene");
        }
    }

    void SwitchSkin(bool isWell)
    {
        Sprite boardSkin = isWell ? wellBoardSprite : farmBoardSprite;
        Sprite panelSkin = isWell ? wellPanel : farmPanel;

        // 切换 FarmSkin / WellSkin
        farmSkin.SetActive(!isWell);
        wellSkin.SetActive(isWell);

        foreach (Transform child in transform)
        {
            // panel
            if (child.name == "panel")
            {
                var img = child.GetComponent<Image>();
                if (img != null) img.sprite = panelSkin;
                continue;
            }

            // board
            var bg = child.Find("Background")?.GetComponent<Image>();
            if (bg != null) bg.sprite = boardSkin;
        }
    }
}

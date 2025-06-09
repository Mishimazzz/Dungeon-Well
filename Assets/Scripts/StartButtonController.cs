using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartButtonController : MonoBehaviour
{
    public GameObject timePanel;
    public GameObject timeCountDownPanel;

    public TextMeshProUGUI exploreButtonText;
    public TimeCountDownController timeCountDownController;
    public void OnButtonClick()
    {
        Debug.Log(" Debug: You click the start button");
        timePanel.SetActive(false);
        timeCountDownPanel.SetActive(true);
        exploreButtonText.text = "Cancel";
        timeCountDownController.isCounting = false;
    }
}

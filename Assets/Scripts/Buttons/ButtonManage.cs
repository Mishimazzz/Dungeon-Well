using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ButtonManage : MonoBehaviour
{
    public GameObject timePanel;
    public GameObject timeStopPanel;
    public GameObject timeUpPanel;
    public Button exploreButton;
    public TextMeshProUGUI exploreButtonText;
    public GameObject timeCountDownPanel;

    public TimeCountDownController timeCountDownController;
    public void StartButton()
    {
        Debug.Log(" Debug: You click the start button");
        timePanel.SetActive(false);
        timeCountDownPanel.SetActive(true);
        exploreButtonText.text = "Cancel";
        timeCountDownController.isCounting = false;
    }
    public void ExploreButton()
    {
        string text = exploreButtonText.text;
        Debug.Log(" Debug: You click the Explore button");

        if (text.Equals("Explore")) timePanel.SetActive(true);
        else
        {
            timeStopPanel.SetActive(true);
        }
    }

    public void TimeUpButtom()
    {
        timeUpPanel.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ExploreButton : MonoBehaviour
{
    public GameObject timePanel;
    public GameObject timeStopPanel;
    public Button exploreButton;
    public TextMeshProUGUI exploreButtonText;
    public void OnButtonClick()
    {
        string text = exploreButtonText.text;
        Debug.Log(" Debug: You click the Explore button");

        if (text.Equals("Explore")) timePanel.SetActive(true);
        else
        {
            timeStopPanel.SetActive(true);
        }
    }
}

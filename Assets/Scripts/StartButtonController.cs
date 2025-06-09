using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButtonController : MonoBehaviour
{
    public GameObject timePanel;
    public void OnButtonClick()
    {
        Debug.Log(" Debug: You click the start button");
        timePanel.SetActive(false);
    }
}

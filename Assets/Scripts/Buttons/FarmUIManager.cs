using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmUIManager : MonoBehaviour
{
    public GameObject SeedPanel;
    public void ToggleSeedBox()
    {
        bool isActive = SeedPanel.activeSelf;
        SeedPanel.SetActive(!isActive);

        if (isActive) // 如果是刚打开
        {
            SeedBoxManager.seedBoxManager.RefreshSeedBoxUI();
        }
    }
}

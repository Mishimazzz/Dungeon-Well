using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType { Well, Farm }
public class SceneManager : MonoBehaviour
{
    public void LoadSceneByName(SceneType type)
    {
        switch (type)
        {
            case SceneType.Well:
                UnityEngine.SceneManagement.SceneManager.LoadScene("WellScene");
                break;
            case SceneType.Farm:
                UnityEngine.SceneManagement.SceneManager.LoadScene("FarmScene");
                break;
        }
    }

    //switch scenes
    public void LoadWell() => LoadSceneByName(SceneType.Well);
    public void LoadFarm() => LoadSceneByName(SceneType.Farm);

    public void LoadScene()
    {
        string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        Debug.Log(currentSceneName);
        if (currentSceneName.Equals("WellScene")) LoadFarm();
        else if (currentSceneName.Equals("FarmScene")) LoadWell();
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}

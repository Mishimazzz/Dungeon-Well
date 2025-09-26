using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIButtonManager : MonoBehaviour
{
  //退出游戏
  public void QuitGame()
  {
    Application.Quit();
    //unity play 退出
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#endif
  }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    //保证所有的种子都不会随着切换场景而销毁
    void Awake()
    {
        DontDestroyOnLoad(gameObject); // 保留这个对象和它的所有子对象
    }
}

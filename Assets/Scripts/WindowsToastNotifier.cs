using System.Diagnostics;
using UnityEngine;

public class WindowsToastNotifier
{
    public static void ShowToast(string title, string message)
    {
        string psScriptPath = Application.dataPath + "Assets/Scripts/toast.ps1";
        ProcessStartInfo psi = new ProcessStartInfo()
        {
            FileName = "powershell.exe",
            Arguments = $"-ExecutionPolicy Bypass -File \"{psScriptPath}\" -title \"探索完成\" -message \"探索时间结束！可以领奖励了。\"",
            UseShellExecute = false,
            CreateNoWindow = true
        };
        Process.Start(psi);

    }
}

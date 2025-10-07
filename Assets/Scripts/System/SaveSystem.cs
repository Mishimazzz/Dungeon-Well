using UnityEngine;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;
    string savePath;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        savePath = Path.Combine(Application.persistentDataPath, "save.json");
    }

    public void SaveGame(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        #if UNITY_EDITOR
        Debug.Log("[Save] " + savePath);
        #endif
    }

    public SaveData LoadGame()
    {
        if (!File.Exists(savePath)) return new SaveData();
        var json = File.ReadAllText(savePath);
        return JsonUtility.FromJson<SaveData>(json) ?? new SaveData();
    }
}

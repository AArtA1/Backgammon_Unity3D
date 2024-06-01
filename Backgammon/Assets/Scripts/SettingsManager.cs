using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DiceSettings
{
    public Vector3 ForceAmount;
    public List<Vector3> RollTorques;
}

[System.Serializable]
public class GameSettings
{
    public DiceSettings DiceSettings;
}

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    public GameSettings Settings;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadSettings()
    {
        TextAsset json = Resources.Load<TextAsset>("settings.json");
        Settings = JsonUtility.FromJson<GameSettings>(json.text);
    }
}
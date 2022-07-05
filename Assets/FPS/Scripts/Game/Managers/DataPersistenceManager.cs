using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Unity.FPS.Game
{
    public class DataPersistenceManager : MonoBehaviour
    {
        public const string SAVE_SETTINGS_FILENAME = "saveSettings.json";

        public static DataPersistenceManager Instance;

        public SettingsSaveData defaultSettings;
        public SettingsSaveData currentSettings;
        public int playerScore;

        private string settingsPath;

        public void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            settingsPath = Application.persistentDataPath + SAVE_SETTINGS_FILENAME;
            LoadSettings();
        }

        [System.Serializable]
        public class SettingsSaveData
        {
            [Range(0.05f, 3f)]
            public float lookSensitivity;
            [Range(0.0f, 1f)]
            public float musicVolume;
            public bool weaponAutoSwitchOnPickup;
            public bool enableShadows;
            public bool isInvincible;
            public bool showFramerate;
            public string playerLastUsedNickname;
        }

        public void SaveSettings(SettingsSaveData newSettings)
        {
            currentSettings = newSettings;
            string json = JsonUtility.ToJson(newSettings);
            File.WriteAllText(settingsPath, json);
        }

        public void LoadSettings()
        {
            if (File.Exists(settingsPath))
            {
                string json = File.ReadAllText(settingsPath);
                SettingsSaveData loadedSettings = JsonUtility.FromJson<SettingsSaveData>(json);
                currentSettings = loadedSettings;
            }
            else
            {
                currentSettings = defaultSettings;
            }
        }

        public void DeleteSettings()
        {
            string path = Application.persistentDataPath + SAVE_SETTINGS_FILENAME;
            if (File.Exists(settingsPath))
            {
                File.Delete(settingsPath);
            }
        }
    }
}

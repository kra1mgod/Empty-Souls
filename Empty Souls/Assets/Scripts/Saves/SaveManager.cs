using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    public string saveFileName = "savegame.json";
    public string backupFileName = "savegame_backup.json";
    public int currentSaveVersion = 2;
    public string currentGameVersion = "1.0.0";

    public GameSaveRoot LoadedSave { get; private set; }
    public PlayerProfile ActiveProfile { get; private set; }

    // --- Путь к "Мои документы" для сейвов ---
    private string SaveFolderPath
    {
        get
        {
#if UNITY_STANDALONE_WIN
            string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string folder = Path.Combine(documents, "EmptySouls", "Saves");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            return folder;
#else
            return Application.persistentDataPath;
#endif
        }
    }
    private string SavePath => Path.Combine(SaveFolderPath, saveFileName);
    private string BackupPath => Path.Combine(SaveFolderPath, backupFileName);

    private bool loaded = false;
    public bool IsLoaded => loaded;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[SaveManager] Awake, will load save...");
            // Важно: асинхронно загружаем сейв при старте
            _ = LoadAsync();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async Task LoadAsync()
    {
        try
        {
            Debug.Log("[SaveManager] Loading save from: " + SavePath);
            LoadedSave = await LoadGameSaveRootAsync();
            if (LoadedSave != null && LoadedSave.profiles != null && LoadedSave.profiles.Count > 0)
            {
                ActiveProfile = LoadedSave.profiles[0];
                Debug.Log("[SaveManager] Loaded profile: " + ActiveProfile.profileName);
            }
            else
            {
                ActiveProfile = null;
                Debug.LogWarning("[SaveManager] No profiles found in save.");
            }
            loaded = true;
        }
        catch (Exception ex)
        {
            Debug.LogError("[SaveManager] Failed to load save: " + ex);
            LoadedSave = null;
            ActiveProfile = null;
            loaded = true;
        }
    }

    public async Task SaveAsync()
    {
        if (LoadedSave == null)
        {
            LoadedSave = new GameSaveRoot();
            LoadedSave.profiles = new List<PlayerProfile>();
        }
        LoadedSave.saveVersion = currentSaveVersion;
        LoadedSave.gameVersion = currentGameVersion;
        LoadedSave.lastSaveTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

        // Резервная копия
        if (File.Exists(SavePath))
            File.Copy(SavePath, BackupPath, true);

        string json = JsonUtility.ToJson(LoadedSave, true);

        await Task.Run(() => File.WriteAllText(SavePath, json));
        Debug.Log("[SaveManager] Saved!");
    }

    private async Task<GameSaveRoot> LoadGameSaveRootAsync()
    {
        string json = null;
        if (File.Exists(SavePath))
            json = await Task.Run(() => File.ReadAllText(SavePath));
        else if (File.Exists(BackupPath))
            json = await Task.Run(() => File.ReadAllText(BackupPath));
        else
            return null;

        var root = JsonUtility.FromJson<GameSaveRoot>(json);

        // Валидация
        if (root == null || root.profiles == null)
            throw new Exception("Save corrupted or invalid");

        // Миграция по версии
        if (root.saveVersion < currentSaveVersion)
        {
            Debug.Log("[SaveManager] Migrating save...");
            // TODO: Migration logic
            root.saveVersion = currentSaveVersion;
        }
        return root;
    }

    public void SetActiveProfile(string profileId)
    {
        if (LoadedSave == null || LoadedSave.profiles == null) return;
        ActiveProfile = LoadedSave.profiles.Find(p => p.profileId == profileId);
        if (ActiveProfile != null)
            Debug.Log("[SaveManager] ActiveProfile set: " + ActiveProfile.profileName);
    }

    public void CreateProfile(string profileName)
    {
        if (LoadedSave == null) LoadedSave = new GameSaveRoot();
        if (LoadedSave.profiles == null) LoadedSave.profiles = new List<PlayerProfile>();
        var profile = new PlayerProfile
        {
            profileId = Guid.NewGuid().ToString(),
            profileName = profileName,
            createdAt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            lastPlayedAt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
        };
        LoadedSave.profiles.Add(profile);
        ActiveProfile = profile;
        Debug.Log("[SaveManager] Profile created: " + profileName);
    }

    public void DeleteProfile(string profileId)
    {
        if (LoadedSave == null) return;
        LoadedSave.profiles.RemoveAll(p => p.profileId == profileId);
        if (ActiveProfile != null && ActiveProfile.profileId == profileId)
            ActiveProfile = null;
        Debug.Log("[SaveManager] Profile deleted: " + profileId);
    }
}
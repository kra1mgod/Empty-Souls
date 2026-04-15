using System;
using System.Collections.Generic;

[Serializable]
public class PlayerProfile
{
    public string profileId;
    public string profileName;
    public string createdAt;
    public string lastPlayedAt;
    public GameStatistics gameStatistics = new GameStatistics();
    public Resources resources = new Resources();
    public Progression progression = new Progression();
    public CurrentSelection currentSelection = new CurrentSelection();
    public UserSettings userSettings = new UserSettings();
}

[Serializable]
public class GameStatistics
{
    public int bestLevel;
    public int bestSurvivalTimeSec;
    public int totalPlayTimeSec;
    public int totalDeaths;
    public int evolutionCount;
}

[Serializable]
public class Resources
{
    public int totalSoulFragments;
    public int totalLumzvar;
    public List<string> achievements = new List<string>();
}

[Serializable]
public class Progression
{
    public List<string> unlockedAbilities = new List<string>();
    public List<string> unlockedCharacters = new List<string>();
    public Dictionary<string, WeaponProgress> weaponProgress = new Dictionary<string, WeaponProgress>();
}

[Serializable]
public class WeaponProgress
{
    public int level;
    public bool evolved;
    public int experiencePoints;
}

[Serializable]
public class CurrentSelection
{
    public string selectedCharacter;
    public string selectedFragment;
    public string selectedWeapon;
}

[Serializable]
public class UserSettings
{
    public AudioSettings audioSettings = new AudioSettings();
    public GameplaySettings gameplaySettings = new GameplaySettings();
}

[Serializable]
public class AudioSettings
{
    public float musicVolume = 1f;
    public float sfxVolume = 1f;
    public float ambientVolume = 1f;
}

[Serializable]
public class GameplaySettings
{
    public bool autoSaveEnabled = true;
    public string difficultyLevel = "Normal";
}
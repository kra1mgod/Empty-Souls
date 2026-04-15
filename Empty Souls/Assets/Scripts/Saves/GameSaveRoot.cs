using System;
using System.Collections.Generic;

[Serializable]
public class GameSaveRoot
{
    public List<PlayerProfile> profiles = new List<PlayerProfile>();
    public string gameVersion;
    public int saveVersion;
    public string lastSaveTime;
}
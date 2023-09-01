using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class SaveData
{
    public string playerName;
    private readonly string defaultPlayerName = "Player";

    public float musicVolume;
    public float sfxVolume;
    public float envSFXVolume;
    public float dangerSFXVolume;

    public SaveData()
    {
        playerName = defaultPlayerName;
        musicVolume = 0.5f;
        sfxVolume = 1f;
        dangerSFXVolume = 1f;
        envSFXVolume = 0.15f;
    }
}

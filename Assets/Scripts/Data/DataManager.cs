using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private SaveData _saveData;
    private JsonSaver _jsonSaver;

    public float MusicVolume 
    {
        get {return _saveData.musicVolume;}
        set {_saveData.musicVolume = value;}
    }

    public float SFXVolume
    {
        get{return _saveData.sfxVolume;}
        set{_saveData.sfxVolume = value;}
    }

    public float EnvSFXVolume
    {
        get{return _saveData.envSFXVolume;}
        set{_saveData.envSFXVolume = value;}
    }

    public float DangerSFXVolume
    {
        get{return _saveData.dangerSFXVolume;}
        set{_saveData.dangerSFXVolume = value;}
    }

    private void Awake()
    {
        _saveData = new SaveData();
        _jsonSaver = new JsonSaver();
    }

    public void Save()
    {
        _jsonSaver.Save(_saveData);
    }

    public void Load()
    {
        _jsonSaver.Load(_saveData);
    }
}

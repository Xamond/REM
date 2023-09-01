using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonSaver
{
    private static readonly string _filename = "saveData1.sav";

    public static string GetSaveFileName()
    {
        return Application.persistentDataPath + "/" + _filename;
    }

    public void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data);
        string saveFileName = GetSaveFileName();

        FileStream filestream = new FileStream(saveFileName, FileMode.Create);

        using(StreamWriter writer = new StreamWriter(filestream))
        {
            writer.Write(json);
        }
    }

    public bool Load(SaveData data)
    {
        string loadFileName = GetSaveFileName();
        if(File.Exists(loadFileName))
        {
            using (StreamReader reader = new StreamReader(loadFileName))
            {
                string json = reader.ReadToEnd();
                JsonUtility.FromJsonOverwrite(json, data);
            }
            return true;
        }
        return false;
    }

    public void Delete()
    {
        File.Delete(GetSaveFileName());
    }

}

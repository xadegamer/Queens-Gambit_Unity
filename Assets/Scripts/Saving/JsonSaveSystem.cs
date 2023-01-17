using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class JsonSaveSystem
{
    public static readonly string SAVE_FOLDER = Application.persistentDataPath + "/";
    public static string SAVE_EXTENSION = ".txt";


    public static void Save<T>(string fileName, T t)
    {
        string json = JsonUtility.ToJson(t);
        File.WriteAllText(string.Concat(SAVE_FOLDER, fileName, SAVE_EXTENSION), json);
    }

    public static T Load<T>(string fileName)
    {
        if (File.Exists(SAVE_FOLDER + fileName + SAVE_EXTENSION))
        {
            string saveString = File.ReadAllText(string.Concat(SAVE_FOLDER, fileName, SAVE_EXTENSION));

            if (saveString != null)
            {
                T t = JsonUtility.FromJson<T>(saveString);
                return t;
            }
            return default;
        }
        else return default;
    }
}

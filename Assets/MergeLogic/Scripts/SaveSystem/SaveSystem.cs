using UnityEngine;
using System.IO;

public class SaveSystem<T>
{
    public static readonly string SAVE_FOLDER = Application.persistentDataPath + "/Saves/";
    private string _fileName;

    public SaveSystem(string fileName)
    {
        Debug.Log(SAVE_FOLDER + _fileName + ".txt");
        _fileName = fileName;
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
    }
    public void Save(T value)
    {        
        string json = JsonUtility.ToJson(value, true);
        File.WriteAllText(SAVE_FOLDER + _fileName + ".txt", json);
    }
    public bool TryLoad(out T value)
    {
        value = default(T);
        if (File.Exists(SAVE_FOLDER + _fileName + ".txt"))
        {
            string saveString = File.ReadAllText(SAVE_FOLDER + _fileName + ".txt");
            value = JsonUtility.FromJson<T>(saveString);
            return true;
        }
        else return false;
    }
}
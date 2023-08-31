using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    public static void SaveData()
    {        
        BinaryFormatter formatter = new BinaryFormatter();

        PlayerData data = new PlayerData();

        string path = Application.persistentDataPath + "/Data.yeah";

        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);

        stream.Close();

    } 
    
    public static void LoadData()
    {
        string path = Application.persistentDataPath + "/Data.yeah";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;

            stream.Close();

            data.Load();
        }
        else
        {
            Debug.LogWarning("No Save Data found in : " + path);
        }
    }
}

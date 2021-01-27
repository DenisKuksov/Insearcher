using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager
{

    public static void Save(ProgressData progressData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/data.dat";
        FileStream stream = new FileStream(path, FileMode.Create);

        ProgressData data = progressData;

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static ProgressData Load()
    {
        string path = Application.persistentDataPath + "/data.dat";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            var data = formatter.Deserialize(stream) as ProgressData;
            stream.Close();

            return data;
        }
        else
        {
            return null;
        }
    }
}

using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer(Player player, LevelSystem levelSystem, StageManager stageManager, QuizManager quizManager, ItemAndGoldSystem goldSystem)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Quizzosing.mp4";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player, levelSystem, stageManager, quizManager, goldSystem);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/Quizzosing.mp4";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("Save file not found");
            return null;
        }

    }
}

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
#nullable enable

namespace FactoryGame.SaveGame
{
    /// <summary>
    /// SaveFileManager keeps track of the save files stored in <c>Application.persistentDataPath</c>.
    /// </summary>
    public static class SaveFileManager
    {
        public static GameData? Load(string saveName)
        {
            string filePath = Path.Combine(Application.persistentDataPath, saveName + ".dat");

            if (File.Exists(filePath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(filePath, FileMode.Open);

                GameData gameData = (GameData)bf.Deserialize(file);
                file.Close();

                Debug.Log("Loaded game: " + filePath);
                return gameData;
            }
            else
            {
                Debug.LogWarning("File doesn't exist: " + filePath);
                return null;
            }
        }

        public static void Save(GameData save, string saveName)
        {
            BinaryFormatter bf = new BinaryFormatter();
            string savelocation = Path.Combine(Application.persistentDataPath, saveName + ".dat");
            FileStream file = File.Create(savelocation);

            bf.Serialize(file, save);
            file.Close();
            Debug.Log("Saved game: " + savelocation);
        }

        public static void Delete(string saveName)
		{
            string path = Path.Combine(Application.persistentDataPath, saveName + ".dat");

            if (!File.Exists(path))
                return;

            File.Delete(path);
		}

        public static IEnumerable<string> GetSaves()
        {
            return Directory.EnumerateFiles(Application.persistentDataPath, "*.dat")
                .Select(fullPath => Path.GetFileNameWithoutExtension(fullPath));
        }
    }
}

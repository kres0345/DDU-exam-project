using FactoryGame.Placements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FactoryGame.SaveGame
{
    public class SaveGameController : MonoBehaviour
    {
        public static SaveGameController Instance { get; private set; }
        
        public string SaveName { get; private set; }

        public GameData GameData { get; private set; }

        [SerializeField]
        private string _loadedDebugSaveLoad;

        bool gameHasBeenLoaded = false;

        void Awake()
        {
            _loadedDebugSaveLoad = "Not loaded";
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        void OnLevelWasLoaded()
        {
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;
            if (sceneIndex == 0)
            {
                if (gameHasBeenLoaded)
                {
                    Destroy(this.gameObject);
                }
            }
            else if (sceneIndex == 1)
            {
                gameHasBeenLoaded = true;
                LoadCurrentSaveIntoWorld();
            }
        }

        public void SaveCurrentGameSync() => SaveCurrentGameSync(SaveName);

        public void SaveCurrentGameSync(string saveName)
        {
            SaveName = saveName;

            if (GameData is null)
            {
                Debug.LogWarning("No game data associated");
                return;
            }

            // Perform serialization
            SerializeAllPlacements();

            // Actual saving
            SaveFileManager.Save(GameData, SaveName);
        }

        public void StartSave(GameData gameData, string saveName)
        {
            GameData = gameData;
            SaveName = saveName;

            SceneManager.LoadScene(1);
        }

        private void LoadCurrentSaveIntoWorld()
        {
            DeserializeAllPlacements();
            DeserializeAllInventories();

            _loadedDebugSaveLoad = "Loaded: " + SaveName;
        }

        private void DeserializeAllInventories()
        {

        }

        private void DeserializeAllPlacements()
        {
            if (GameData is null)
            {
                Debug.Log("No game data associated");
                return;
            }

            // reset statics
            PlacementController.ResetStatics();

            Dictionary<string, Object> prefabCache = new Dictionary<string, Object>();

            foreach (DictionaryEntry item in GameData.Placements)
            {
                // Get position
                string[] positionCoordinates = ((string)item.Key).Split(',');
                Vector2Int position = new Vector2Int(int.Parse(positionCoordinates[0]), int.Parse(positionCoordinates[1]));

                // Separate prefab name and data
                string[] serializedData = ((string)item.Value).Split(new char[] { ';' }, 2);
                string prefabName = serializedData[0];

                // Get prefab by name
                if (!prefabCache.TryGetValue(prefabName, out Object prefab))
                {
                    prefab = Resources.Load($"Placements/{prefabName}", typeof(GameObject));
                    prefabCache.Add(prefabName, prefab);
                }

                GameObject instantiatedPlacement = (GameObject)Instantiate(prefab);
                Placement placement = instantiatedPlacement.GetComponent<Placement>();
                placement.SetSerializedData(serializedData[1]);
                PlacementController.PlaceBuilding(placement, position);
                placement.UpdateSprite(false);
            }
        }
        
        // Regner med at vi har diverse game save ting her.
        private void SerializeAllPlacements()
        {
            if (GameData is null)
            {
                Debug.LogWarning("No game data specified for save game controller");
                return;
            }

            GameData.Placements.Clear();
            HashSet<Placement> serializedPlacements = new HashSet<Placement>();

            foreach (var item in PlacementController.GetPlacements())
            {
                if (serializedPlacements.Contains(item.Value))
                {
                    continue;
                }
                
                serializedPlacements.Add(item.Value);

                // Prefab navnet findes ved at fjerne ekstra delen af navnet.
                string prefabName = item.Value.GetPrefabName();
                // Gem også den data den har.
                string serializedData = item.Value.GetSerializedData();

                var position = item.Value.GridPosition;

                GameData.Placements.Add($"{position.x},{position.y}", $"{prefabName};{serializedData}");
            }
        }
    }
}
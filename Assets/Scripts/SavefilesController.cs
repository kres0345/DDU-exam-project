using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FactoryGame.SaveGame;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine.Events;

public class SavefilesController : MonoBehaviour
{
    public GameObject GameSavePrefab;
    public GameObject ConfirmBox;
    public string selectedSavefile;

    private string newSaveGameName;

    void Start()
    {
        UpdateSaveList();
    }

    public void UpdateSaveList()
    {
        var saves = SaveFileManager.GetSaves();

        //Finds the children
        GameObject[] childrenToKill = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            childrenToKill[i] = transform.GetChild(i).gameObject;
        }

        //Abandons the children
        transform.DetachChildren();

        //Comit warcrimes against the children
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(childrenToKill[i]);
        }

        foreach (var filename in saves)
        {
            GameObject gameSave = Instantiate(GameSavePrefab);
            gameSave.transform.SetParent(transform);
            gameSave.transform.localScale = new Vector3(1, 1, 1);
            gameSave.GetComponent<SaveGameButton>().SetFilename(filename);
        }
        
        //Debug.Log(Application.persistentDataPath);
    }

    public void UpdateNewSaveName(TMP_Text NewSaveName)
    {
        newSaveGameName = NewSaveName.text;
        //Debug.Log(NewSaveName.text);
    }

    public void NewSaveFile()
    {
        //Tjekker om navnet er tom
        if (newSaveGameName.Length <= 1)
            return;

        SaveFileManager.Save(new GameData(), newSaveGameName);
        UpdateSaveList();
    }
    
    public void SetSelectedSavefile(string savefile)
    {
        selectedSavefile = savefile;
        Debug.Log($"'{savefile}' have been selected");
    }

    public void QuickSave()
    {
        SaveGameController.Instance.SaveCurrentGameSync();
    }

    public void SaveToSelectedGame()
    {
        SaveGameController.Instance.SaveCurrentGameSync(selectedSavefile);
    }
}
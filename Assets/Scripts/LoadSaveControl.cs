using FactoryGame.SaveGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSaveControl : MonoBehaviour
{
	public SavefilesController savefilesController;

	public void LoadGame()
	{
		//Debug.Log($"�bner: {savefilesController?.selectedSavefile}.");

		if (savefilesController.selectedSavefile == null)
			return;

		if (savefilesController.selectedSavefile.Length > 0)
		{
			string saveFile = savefilesController.selectedSavefile;
			GameData gameData = SaveFileManager.Load(saveFile);
			SaveGameController.Instance.StartSave(gameData, savefilesController.selectedSavefile);
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FactoryGame.SaveGame;
using UnityEngine.UI;
using TMPro;

public class SaveGameButton : MonoBehaviour
{
	private string SaveName;

	void Start()
	{
		Button btn = gameObject.GetComponent<Button>();
		SavefilesController controller = gameObject.GetComponentInParent<SavefilesController>();
		btn.onClick.AddListener(() => controller.SetSelectedSavefile(SaveName));
	}

	public void SetFilename(string saveName)
	{
		//Stores the values
		SaveName = saveName;

		//Manipulates the prefab to look correct
		gameObject.GetComponentInChildren<TMP_Text>().text = SaveName;
	}
}
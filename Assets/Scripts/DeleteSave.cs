using FactoryGame.SaveGame;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeleteSave : MonoBehaviour
{
	public SavefilesController Controller;
	public TMP_Text ConfirmText;
	public GameObject Panel;

	private string lastDeleted;

	public void TryDeleteSelected()
	{
		Panel.SetActive(true);
		ConfirmText.text = $"Delete '{Controller.selectedSavefile}'?";
	}

	public void Canceled()
	{
		Panel.SetActive(false);
	}

    public void DeleteSelectedSavefile ()
	{
		SaveFileManager.Delete(Controller.selectedSavefile);

		Controller.UpdateSaveList();
		Debug.Log($"Deleted {Controller.selectedSavefile}");
		Controller.selectedSavefile = null;
		Panel.SetActive(false);
	}
}
using UnityEngine;
using UnityEngine.UI;

namespace FactoryGame.UI
{
    public class MenuButton : MonoBehaviour
	{
		public MenuController.Menues ThisMenu;
		public bool MoveToVisible;

		private Button thisButton;
		private MenuController menuController;

		public void Start()
		{
			thisButton = gameObject.GetComponent<Button>();
			menuController = transform.GetComponentInParent<Transform>().GetComponentInParent<Transform>().GetComponentInParent<MenuController>();

			thisButton.onClick.AddListener(MoveMenu);
		}

		public void MoveMenu()
		{
			menuController.Move(ThisMenu, MoveToVisible);
		}
	}
}
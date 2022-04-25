using UnityEngine;
using UnityEngine.InputSystem;

namespace FactoryGame.UI
{
    public class MenuHider : MonoBehaviour
    {
        public PlayerInput PlayerInput;
        public MenuController.Menues[] menues;
        public MenuController menuController;
        public GameObject menu;

        private InputAction Action;

        private void Awake()
        {
            Action = PlayerInput.actions["MenuOpenClose"];
        }

        private void Update()
        {
            if (Action.triggered)
            {
                ChangeVisibility();
            }
        }

        void ChangeVisibility()
        {
            Debug.Log("changed visibility");
            for (int i = 0; i < transform.childCount; i++)
            {
                string name = transform.GetChild(i).gameObject.name;
                if (transform.GetChild(i).gameObject.activeSelf && name != "Hotbar" && name != "BuildModeIcon" && name != "CursorSlot")
                {
                    Debug.Log("Deactivate");
                    transform.GetChild(i).gameObject.SetActive(false);
                    return;
                }
            }

            menu.SetActive(!menu.activeSelf);
            for (int i = 0; i < menues.Length; i++)
            {
                menuController.Move(menues[i], false);
            }
        }
    }
}
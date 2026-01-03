using UnityEngine;
using UnityEngine.InputSystem;

namespace MapBuilder
{
    // This class is responsible for adding the buttons to the menu.
    public class PieceSelectorMenu : MonoBehaviour
    {
        private bool _menuOpen = false;
        GameObject canvasObject;
        GameObject content;
        [SerializeField] GameObject pieceSelectionButtonPrefab;

        void Start()
        {
            MapEditorInputManager.Instance.menuAction.started += OnMenu;
            canvasObject = transform.Find("Canvas").gameObject;
            content = canvasObject.transform.Find("Scroll View").Find("Viewport").Find("Content").gameObject;

            AddAllItemsToMenu();
        }

        // Add a single button to the menu with a given name
        public void AddEntryToMenu(string name)
        {
            GameObject newEntry = Instantiate(pieceSelectionButtonPrefab);
            newEntry.GetComponent<PieceSelectorButton>().prefabName = name;
            newEntry.GetComponent<PieceSelectorButton>().previewScript = GetComponent<EditorPiecePreview>();
            GameObject nameObject = newEntry.transform.Find("Name").gameObject;
            nameObject.GetComponent<TMPro.TMP_Text>().text = name;
            newEntry.transform.SetParent(content.transform);
            newEntry.transform.localScale = Vector2.one;
        }

        private void AddAllItemsToMenu()
        {
            PieceSelectorMenu menuScript = GetComponent<PieceSelectorMenu>();

            foreach (string name in MapEditor.Instance.keys)
            {
                menuScript.AddEntryToMenu(name);
            }
        }

        // This function is called when the menu key is pressed. It hides and shows the menu as well as locking and unlocking the cursor.
        private void OnMenu(InputAction.CallbackContext context)
        {
            _menuOpen = !_menuOpen;

            if (_menuOpen)
            {
                MapEditorInputManager.Instance.UnlockCursor();
                canvasObject.GetComponent<Canvas>().enabled = true;
            }
            else if (!_menuOpen)
            {
                MapEditorInputManager.Instance.LockCursor();
                canvasObject.GetComponent<Canvas>().enabled = false;
            }
        }
    }
}

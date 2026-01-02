using UnityEngine;
using UnityEngine.InputSystem;

namespace MapBuilder
{
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

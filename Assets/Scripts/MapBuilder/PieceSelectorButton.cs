using UnityEngine;

namespace MapBuilder
{
    // Sets the prefab name in the preview script when the button is pressed
    public class PieceSelectorButton : MonoBehaviour
    {
        public string prefabName;
        public EditorPiecePreview previewScript;

        public void OnPressed()
        {
            previewScript.prefabName = prefabName;
        }
    }
}

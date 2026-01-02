using UnityEngine;

namespace MapBuilder
{
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

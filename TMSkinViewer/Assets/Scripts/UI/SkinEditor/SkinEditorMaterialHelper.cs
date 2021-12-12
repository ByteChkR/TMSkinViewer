using UnityEngine;

namespace UI.SkinEditorMainWindow
{

    public class SkinEditorMaterialHelper : MonoBehaviour
    {

        public void Toggle()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }

    }

}

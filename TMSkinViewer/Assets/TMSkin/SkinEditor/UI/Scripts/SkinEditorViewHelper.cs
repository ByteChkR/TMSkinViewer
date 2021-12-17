using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.SkinEditorMainWindow
{

    public class SkinEditorViewHelper : MonoBehaviour, IPointerClickHandler
    {

        [SerializeField]
        private SkinEditorWindow m_EditorWindow;

        public void OnPointerClick( PointerEventData eventData )
        {
            m_EditorWindow.ViewerInstance.NextCamera();
        }

    }

}

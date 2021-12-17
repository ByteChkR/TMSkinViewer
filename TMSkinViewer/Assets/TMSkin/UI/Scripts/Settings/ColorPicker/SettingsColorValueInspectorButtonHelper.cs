using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Settings.ColorPicker
{

    public class SettingsColorValueInspectorButtonHelper : MonoBehaviour,IPointerClickHandler
    {

        [SerializeField]
        private SettingsColorValueInspector m_Inspector;
        public void OnPointerClick( PointerEventData eventData )
        {
            m_Inspector.OpenWindow();
        }

    }

}

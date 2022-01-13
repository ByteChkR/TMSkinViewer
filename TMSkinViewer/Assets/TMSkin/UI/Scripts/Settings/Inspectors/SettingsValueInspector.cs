using UnityEngine;
using UnityEngine.UI;

namespace UI.Settings
{

    public abstract class SettingsValueInspector : MonoBehaviour
    {

        [SerializeField]
        protected Text m_PropertyName;

        public virtual void SetProperty( SettingsPropertyWrapper prop )
        {
            m_PropertyName.text = prop.Name;

            if ( !prop.CanWrite )
            {
                foreach ( Selectable selectable in GetComponentsInChildren < Selectable >() )
                {
                    selectable.interactable = false;
                }
            }
        }

    }

}

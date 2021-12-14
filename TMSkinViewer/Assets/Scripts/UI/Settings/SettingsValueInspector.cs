using UnityEngine;
using UnityEngine.UI;

namespace UI.Settings
{

    public abstract class SettingsValueInspector : MonoBehaviour
    {

        [SerializeField]
        private Text m_PropertyName;
        public virtual void SetProperty( SettingsPropertyWrapper prop )
        {
            m_PropertyName.text = prop.Name;
        }

    }

}
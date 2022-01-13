using UnityEngine;
using UnityEngine.UI;

namespace UI.Settings
{

    public class SettingsBooleanInspector : SettingsValueInspector
    {

        [SerializeField]
        private Toggle m_Toggle;

        private SettingsPropertyWrapper m_Wrapper;

        public override void SetProperty( SettingsPropertyWrapper prop )
        {
            base.SetProperty( prop );

            m_Wrapper = prop;
            m_Toggle.SetIsOnWithoutNotify( ( bool )prop.Value );
            m_Toggle.interactable = prop.CanWrite;
        }

        public void OnToggleValueChanged( bool value )
        {
            m_Wrapper.Value = value;
        }

    }

}

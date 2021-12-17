using UnityEngine;
using UnityEngine.UI;

namespace UI.Settings
{

    public class ButtonInspector : SettingsValueInspector
    {

        [SerializeField]
        private Button m_Button;
        [SerializeField]
        private Text m_ButtonText;

        public Button Button => m_Button;

        public Text ButtonText => m_ButtonText;

        public override void SetProperty( SettingsPropertyWrapper prop )
        {
            base.SetProperty( prop );
        }

    }

}
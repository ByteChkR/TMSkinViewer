using UnityEngine;
using UnityEngine.UI;

namespace UI.SkinEditorMainWindow
{

    public class SkinEditorMainWindowItem : MonoBehaviour
    {

        [SerializeField]
        private Button m_Button;

        [SerializeField]
        private Button m_ExportButton;

        [SerializeField]
        private Image m_Icon;

        [SerializeField]
        private Text m_Text;

        public Button Button => m_Button;

        public Button ExportButton => m_ExportButton;

        public Image Icon => m_Icon;

        public Text Text => m_Text;

    }

}

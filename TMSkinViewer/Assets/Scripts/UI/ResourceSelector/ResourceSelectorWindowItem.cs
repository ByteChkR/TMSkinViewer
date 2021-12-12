using UnityEngine;
using UnityEngine.UI;

namespace UI.ResourceSelector
{

    public class ResourceSelectorWindowItem : MonoBehaviour
    {

        [SerializeField]
        private Button m_Button;

        [SerializeField]
        private Image m_Icon;

        [SerializeField]
        private Text m_Text;

        public Button Button => m_Button;

        public Image Icon => m_Icon;

        public Text Text => m_Text;

    }

}

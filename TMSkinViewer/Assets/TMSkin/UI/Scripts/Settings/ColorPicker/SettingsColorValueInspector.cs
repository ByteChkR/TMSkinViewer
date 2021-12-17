using System.Linq;

using UnityEngine;
using UnityEngine.UI;

namespace UI.Settings.ColorPicker
{

    public class SettingsColorValueInspector : SettingsValueInspector
    {

        [SerializeField]
        private GameObject m_WindowPrefab;

        private GameObject m_WindowInstance;
        [SerializeField]
        private Image m_ColorImage;

        private SettingsPropertyWrapper m_Wrapper;

        private Transform m_WindowParent;

        public override void SetProperty( SettingsPropertyWrapper prop )
        {
            base.SetProperty( prop );
            m_Wrapper = prop;
            Color color = (Color)prop.Value;
            m_ColorImage.color = color;
            Window window = GetComponentInParent < Window >();
            m_WindowParent = window.transform.parent;
        }

        public void OpenWindow()
        {
            if ( m_WindowInstance == null )
            {
                m_WindowInstance = Instantiate( m_WindowPrefab, m_WindowParent );
                ColorPickerWindow window = m_WindowInstance.GetComponent < ColorPickerWindow >();
                window.Initialize((Color)m_Wrapper.Value);
                window.OnColorChanged += c =>
                                        {
                                            m_Wrapper.Value = c;
                                            m_ColorImage.color = c;
                                        };
            }
        }

    }

}

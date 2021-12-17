using System.Linq;

using UnityEngine;
using UnityEngine.UI;

namespace UI.SkinEditorMainWindow
{

    public class SkinEditorCreatorDialog : MonoBehaviour
    {

        [SerializeField]
        private Window m_Window;

        [SerializeField]
        private InputField m_Field;

        [SerializeField]
        private Toggle m_UseTemplateToggle;

        [SerializeField]
        private Dropdown m_TemplateDropdown;

        private void Awake()
        {
            m_TemplateDropdown.options.AddRange(
                                                SkinDatabase.LoadedSkins.Select(
                                                                                x => new Dropdown.OptionData( x.SkinName )
                                                                               )
                                               );
        }

        public void OnToggleFromTemplate( bool isOn )
        {
            m_TemplateDropdown.interactable = isOn;
        }

        public void Create()
        {
            string name = m_Field.text;

            if ( m_UseTemplateToggle.isOn )
            {
                CarSkin template = SkinDatabase.LoadedSkins.First(
                                                                  x => x.SkinName ==
                                                                       m_TemplateDropdown.options[m_TemplateDropdown.value].
                                                                           text
                                                                 );

                SkinDatabase.CreateSkin( name, template );
            }
            else
            {
                SkinDatabase.CreateSkin( name );
            }

            m_Window.Close();
        }

    }

}

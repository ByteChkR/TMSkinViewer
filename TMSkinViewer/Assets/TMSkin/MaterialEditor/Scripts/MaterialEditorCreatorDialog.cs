using System.Linq;

using UnityEngine;
using UnityEngine.UI;

namespace UI.SkinEditorMainWindow
{

    public class MaterialEditorCreatorDialog : MonoBehaviour
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
                                                MaterialDatabase.Materials.Select(
                                                     x => new Dropdown.OptionData( x.MaterialName )
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
                CarMaterial template = MaterialDatabase.Materials.First(
                                                                        x => x.MaterialName ==
                                                                             m_TemplateDropdown.
                                                                                 options[m_TemplateDropdown.value].
                                                                                 text
                                                                       );

                MaterialDatabase.CreateMaterial( name, template );
            }
            else
            {
                MaterialDatabase.CreateMaterial( name );
            }

            m_Window.Close();
        }

    }

}

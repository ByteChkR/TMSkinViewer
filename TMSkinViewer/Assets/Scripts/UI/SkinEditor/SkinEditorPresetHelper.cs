using UnityEngine;
using UnityEngine.UI;

namespace UI.SkinEditorMainWindow
{

    public class SkinEditorPresetHelper : MonoBehaviour
    {

        [SerializeField]
        private SkinEditorWindow m_EditorWindow;

        [SerializeField]
        private MapPreset[] m_MapPresets;
        [SerializeField]
        private SurfacePreset[] m_SurfacePresets;

        [SerializeField]
        private CarPreset[] m_CarPresets;
        
        [SerializeField]
        private Dropdown m_MapPresetDropdown;
        
        [SerializeField]
        private Dropdown m_SurfacePresetDropdown;
        [SerializeField]
        private Dropdown m_CarPresetDropdown;

        private void Awake()
        {
            foreach ( MapPreset mapPreset in m_MapPresets )
            {
                m_MapPresetDropdown.options.Add( new Dropdown.OptionData( mapPreset.name ) );
            }
            
            foreach ( CarPreset carPreset in m_CarPresets )
            {
                m_CarPresetDropdown.options.Add( new Dropdown.OptionData( carPreset.name ) );
            }
            
            foreach ( SurfacePreset surfacePreset in m_SurfacePresets )
            {
                m_SurfacePresetDropdown.options.Add( new Dropdown.OptionData( surfacePreset.name ) );
            }
        }

        public void SetMapPreset( int i )
        {
            m_EditorWindow.ViewerInstance.MapController.SetMapPreset(m_MapPresets[i]);
        }
        public void SetSurfacePreset( int i )
        {
            m_EditorWindow.ViewerInstance.MapController.SetSurfacePreset(m_SurfacePresets[i]);
        }
        
        public void SetCarPreset( int i )
        {
            m_EditorWindow.ViewerInstance.MapController.SetCarPreset(m_CarPresets[i]);
        }
        
        public void Toggle()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }

    }

}

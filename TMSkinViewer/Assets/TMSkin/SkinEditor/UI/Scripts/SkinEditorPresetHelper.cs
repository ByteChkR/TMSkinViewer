using UnityEngine;
using UnityEngine.UI;

namespace UI.SkinEditorMainWindow
{

    public class SkinEditorPresetHelper : MonoBehaviour
    {

        [SerializeField]
        private SkinEditorWindow m_EditorWindow;

        [SerializeField]
        private Dropdown m_MapPresetDropdown;

        [SerializeField]
        private Dropdown m_SurfacePresetDropdown;

        [SerializeField]
        private Dropdown m_CarPresetDropdown;

        private void Awake()
        {
            foreach ( MapPreset mapPreset in SkinEditorPresets.Instance.MapPresets )
            {
                m_MapPresetDropdown.options.Add( new Dropdown.OptionData( mapPreset.name ) );
            }

            foreach ( CarPreset carPreset in SkinEditorPresets.Instance.CarPresets )
            {
                m_CarPresetDropdown.options.Add( new Dropdown.OptionData( carPreset.name ) );
            }

            foreach ( SurfacePreset surfacePreset in SkinEditorPresets.Instance.SurfacePresets )
            {
                m_SurfacePresetDropdown.options.Add( new Dropdown.OptionData( surfacePreset.name ) );
            }
        }

        public void SetMapPreset( int i )
        {
            m_EditorWindow.ViewerInstance.MapController.SetMapPreset( SkinEditorPresets.Instance.MapPresets[i] );
        }

        public void SetSurfacePreset( int i )
        {
            m_EditorWindow.ViewerInstance.MapController.SetSurfacePreset(
                                                                         SkinEditorPresets.Instance.SurfacePresets[i]
                                                                        );
        }

        public void SetCarPreset( int i )
        {
            m_EditorWindow.ViewerInstance.MapController.SetCarPreset( SkinEditorPresets.Instance.CarPresets[i] );
        }

        public void Toggle()
        {
            gameObject.SetActive( !gameObject.activeSelf );
        }

    }

}

using UnityEngine;

namespace UI.SkinEditorMainWindow
{

    [SettingsCategory( "Presets" )]
    public class SkinEditorPresets : MonoBehaviour
    {

        [SerializeField]
        private MapPreset[] m_MapPresets;

        [SerializeField]
        private SurfacePreset[] m_SurfacePresets;

        [SerializeField]
        private CarPreset[] m_CarPresets;

        public static SkinEditorPresets Instance { get; private set; }

        [SettingsHeader( "Presets" )]
        [SettingsProperty]
        public MapPreset[] MapPresets
        {
            get => m_MapPresets;
            set => m_MapPresets = value;
        }

        [SettingsProperty]
        public SurfacePreset[] SurfacePresets
        {
            get => m_SurfacePresets;
            set => m_SurfacePresets = value;
        }

        [SettingsProperty]
        public CarPreset[] CarPresets
        {
            get => m_CarPresets;
            set => m_CarPresets = value;
        }

        private void Awake()
        {
            Instance = this;
            SettingsManager.AddSettingsObject( this );
        }

    }

}

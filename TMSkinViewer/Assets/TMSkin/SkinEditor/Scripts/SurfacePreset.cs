using System;

using UnityEngine;

[Serializable]
[CreateAssetMenu( fileName = "TrackSurfacePreset", menuName = "TMSkin/Presets/TrackSurface" )]
public class SurfacePreset : ScriptableObject
{

    [SerializeField]
    private TrackSurface m_TrackSurface;

    [SerializeField]
    private Texture2D m_SurfaceTexture;

    [SerializeField]
    [Range( 0, 1 )]
    private float m_SurfaceReflectivity;

    [SerializeField]
    [Range( 0, 1 )]
    private float m_SurfaceRefractionDistortion;

    [SerializeField]
    private bool m_ApplySurfaceColor;

    [SerializeField]
    private Color m_SurfaceColor;

    [SettingsProperty]
    public TrackSurface TrackSurface
    {
        get => m_TrackSurface;
        set => m_TrackSurface = value;
    }

    [SettingsProperty]
    public Texture2D SurfaceTexture
    {
        get => m_SurfaceTexture;
        set => m_SurfaceTexture = value;
    }

    [SettingsProperty]
    [SettingsRange( 0, 1 )]
    public float SurfaceReflectivity
    {
        get => m_SurfaceReflectivity;
        set => m_SurfaceReflectivity = value;
    }

    [SettingsProperty]
    [SettingsRange( 0, 1 )]
    public float SurfaceRefractionDistortion
    {
        get => m_SurfaceRefractionDistortion;
        set => m_SurfaceRefractionDistortion = value;
    }

    [SettingsProperty]
    public bool ApplySurfaceColor
    {
        get => m_ApplySurfaceColor;
        set => m_ApplySurfaceColor = value;
    }

    [SettingsProperty]
    public Color SurfaceColor
    {
        get => m_SurfaceColor;
        set => m_SurfaceColor = value;
    }

}

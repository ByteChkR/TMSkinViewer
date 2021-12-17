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

    public TrackSurface TrackSurface => m_TrackSurface;

    public Texture2D SurfaceTexture => m_SurfaceTexture;

    public float SurfaceReflectivity => m_SurfaceReflectivity;

    public float SurfaceRefractionDistortion => m_SurfaceRefractionDistortion;

    public bool ApplySurfaceColor => m_ApplySurfaceColor;

    public Color SurfaceColor => m_SurfaceColor;

}

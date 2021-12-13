using System;

using UnityEngine;

[Serializable]
[CreateAssetMenu( fileName = "TrackSurfacePreset", menuName = "TMSkin/Presets/TrackSurface" )]
public class SurfacePreset : ScriptableObject
{

    [SerializeField]
    private TrackSurface m_TrackSurface;

    [SerializeField]
    private Material m_SurfaceMaterial;
    [SerializeField]
    private bool m_ApplySurfaceColor;
    [SerializeField]
    private Color m_SurfaceColor;

    public TrackSurface TrackSurface => m_TrackSurface;

    public Material SurfaceMaterial => m_SurfaceMaterial;
    
    public bool ApplySurfaceColor => m_ApplySurfaceColor;
    
    public Color SurfaceColor => m_SurfaceColor;

}

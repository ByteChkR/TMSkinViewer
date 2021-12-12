using System;

using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "TrackSurfacePreset", menuName = "TMSkin/Presets/TrackSurface")]
public class SurfacePreset : ScriptableObject
{

    [SerializeField]
    private TrackSurface m_TrackSurface;
    public TrackSurface TrackSurface => m_TrackSurface;

    [SerializeField]
    private Color m_SurfaceColor;

    public Color SurfaceColor => m_SurfaceColor;

}
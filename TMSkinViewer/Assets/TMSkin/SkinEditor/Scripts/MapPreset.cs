using System;

using UnityEngine;

[Serializable]
[CreateAssetMenu( fileName = "Map", menuName = "TMSkin/Presets/Map" )]
public class MapPreset : ScriptableObject
{

    [SerializeField]
    private bool m_IsNight;

    [SerializeField]
    private Color m_LightColor;

    public Color LightColor => m_LightColor;

    public bool IsNight => m_IsNight;

}

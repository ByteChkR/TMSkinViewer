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

    
    [SettingsProperty]
    public string Name
    {
        get => name;
        set => name = value;
    }


    
    [SettingsProperty]
    public Color LightColor
    {
        get => m_LightColor;
        set => m_LightColor = value;
    }

    [SettingsProperty]
    public bool IsNight
    {
        get => m_IsNight;
        set => m_IsNight = value;
    }
    
    

}

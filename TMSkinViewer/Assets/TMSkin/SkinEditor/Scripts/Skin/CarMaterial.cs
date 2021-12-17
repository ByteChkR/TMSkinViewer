using System;

using UnityEngine;

[Serializable]
[CreateAssetMenu( fileName = "New Car Material", menuName = "TMSkin/Car Material" )]
public class CarMaterial : ScriptableObject
{

    [SerializeField]
    private string m_MaterialName;

    [SerializeField]
    private Texture2D m_Albedo;

    [SerializeField]
    private Texture2D m_Normal;

    [SerializeField]
    private Texture2D m_AmbientOcclusion;

    [SerializeField]
    private Texture2D m_Emissive;

    [SerializeField]
    private Texture2D m_DirtMask;

    [SerializeField]
    private Texture2D m_Roughness;

    [SettingsProperty]
    public string MaterialName
    {
        get => m_MaterialName;
        set => m_MaterialName = value;
    }

    [SettingsProperty]
    public Texture2D Albedo
    {
        get => m_Albedo;
        set => m_Albedo = value;
    }

    [SettingsProperty]
    public Texture2D Normal
    {
        get => m_Normal;
        set => m_Normal = value;
    }
    [SettingsProperty]
    public Texture2D AmbientOcclusion
    {
        get => m_AmbientOcclusion;
        set => m_AmbientOcclusion = value;
    }

    [SettingsProperty]
    public Texture2D Emissive
    {
        get => m_Emissive;
        set => m_Emissive = value;
    }

    [SettingsProperty]
    public Texture2D DirtMask
    {
        get => m_DirtMask;
        set => m_DirtMask = value;
    }

    [SettingsProperty]
    public Texture2D Roughness
    {
        get => m_Roughness;
        set => m_Roughness = value;
    }

}

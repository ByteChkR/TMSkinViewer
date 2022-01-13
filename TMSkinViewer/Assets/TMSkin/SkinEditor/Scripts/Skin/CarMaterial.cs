using System;

using UnityEngine;

[Serializable]
[CreateAssetMenu( fileName = "New Car Material", menuName = "TMSkin/Car Material" )]
public class CarMaterial : ScriptableObject
{

    [SerializeField]
    private string m_MaterialName;

    [SerializeField]
    private CarTexture m_Albedo;

    [SerializeField]
    private CarTexture m_Normal;

    [SerializeField]
    private CarTexture m_AmbientOcclusion;

    [SerializeField]
    private CarTexture m_Emissive;

    [SerializeField]
    private CarTexture m_DirtMask;

    [SerializeField]
    private CarTexture m_Roughness;

    [SettingsProperty]
    public string MaterialName
    {
        get => m_MaterialName;
        set => m_MaterialName = value;
    }

    [SettingsProperty]
    public CarTexture Albedo
    {
        get => m_Albedo;
        set => m_Albedo = value;
    }

    [SettingsProperty]
    public CarTexture Normal
    {
        get => m_Normal;
        set => m_Normal = value;
    }

    [SettingsProperty]
    public CarTexture AmbientOcclusion
    {
        get => m_AmbientOcclusion;
        set => m_AmbientOcclusion = value;
    }

    [SettingsProperty]
    public CarTexture Emissive
    {
        get => m_Emissive;
        set => m_Emissive = value;
    }

    [SettingsProperty]
    public CarTexture DirtMask
    {
        get => m_DirtMask;
        set => m_DirtMask = value;
    }

    [SettingsProperty]
    public CarTexture Roughness
    {
        get => m_Roughness;
        set => m_Roughness = value;
    }

}

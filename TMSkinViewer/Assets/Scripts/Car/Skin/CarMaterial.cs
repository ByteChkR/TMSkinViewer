using System;

using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Car Material", menuName = "TMSkin/Car Material")]
public class CarMaterial:ScriptableObject
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
    
    public string MaterialName
    {
        get => m_MaterialName;
        set => m_MaterialName = value;
    }

    public Texture2D Albedo
    {
        get => m_Albedo;
        set => m_Albedo = value;
    }
    
    public Texture2D Normal
    {
        get => m_Normal;
        set => m_Normal = value;
    }
    
    public Texture2D AmbientOcclusion
    {
        get => m_AmbientOcclusion;
        set => m_AmbientOcclusion = value;
    }
    
    public Texture2D Emissive
    {
        get => m_Emissive;
        set => m_Emissive = value;
    }
    
    public Texture2D DirtMask
    {
        get => m_DirtMask;
        set => m_DirtMask = value;
    }
    
    public Texture2D Roughness
    {
        get => m_Roughness;
        set => m_Roughness = value;
    }
    

}
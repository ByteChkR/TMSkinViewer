using System;

using UnityEngine;

[Serializable]
[CreateAssetMenu( fileName = "New Car Skin", menuName = "TMSkin/Car Skin" )]
public class CarSkin : ScriptableObject
{

    [SerializeField]
    private string m_SkinName;

    [SerializeField]
    private CarMaterial m_Skin;

    [SerializeField]
    private CarMaterial m_Glass;

    [SerializeField]
    private CarMaterial m_Wheel;

    [SerializeField]
    private CarMaterial m_Details;

    [SettingsProperty]
    public string SkinName
    {
        get => m_SkinName;
        set => m_SkinName = value;
    }

    [SettingsHeader( "Skin Materials" )]
    [SettingsProperty]
    public CarMaterial Skin
    {
        get => m_Skin;
        set => m_Skin = value;
    }

    [SettingsProperty]
    public CarMaterial Glass
    {
        get => m_Glass;
        set => m_Glass = value;
    }

    [SettingsProperty]
    public CarMaterial Wheel
    {
        get => m_Wheel;
        set => m_Wheel = value;
    }

    [SettingsProperty]
    public CarMaterial Details
    {
        get => m_Details;
        set => m_Details = value;
    }

}

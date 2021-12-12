using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Car Skin", menuName = "TMSkin/Car Skin")]
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

    public string SkinName
    {
        get => m_SkinName;
        set => m_SkinName = value;
    }

    public CarMaterial Skin
    {
        get => m_Skin;
        set => m_Skin = value;
    }
    
    public CarMaterial Glass
    {
        get => m_Glass;
        set => m_Glass = value;
    }
    
    public CarMaterial Wheel
    {
        get => m_Wheel;
        set => m_Wheel = value;
    }
    
    public CarMaterial Details
    {
        get => m_Details;
        set => m_Details = value;
    }

}

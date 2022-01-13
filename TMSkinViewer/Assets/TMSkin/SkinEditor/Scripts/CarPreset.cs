using System;

using UnityEngine;

[Serializable]
[CreateAssetMenu( fileName = "CarPreset", menuName = "TMSkin/Presets/Car" )]
public class CarPreset : ScriptableObject
{

    [SerializeField]
    private bool m_IsAccelerating;

    [Range( 0, 1 )]
    [SerializeField]
    private float m_Turn = 0.5f;

    [Range( 0, 1 )]
    [SerializeField]
    private float m_Wings;

    [Range( 0, 1 )]
    [SerializeField]
    private float m_ExhaustUp;

    [Range( 0, 1 )]
    [SerializeField]
    private float m_ExhaustDown;

    [Range( 0, 1 )]
    [SerializeField]
    private float m_WheelDown;

    [Range( 0, 1 )]
    [SerializeField]
    private float m_BrakeLights;

    [Range( 0, 1 )]
    [SerializeField]
    private float m_BrakeAnimation;

    [Range( 0, 1 )]
    [SerializeField]
    private float m_Energy;

    [Range( 0, 1 )]
    [SerializeField]
    private float m_Turbo;

    [SerializeField]
    private Color m_TurboColor;

    [Range( 0, 1 )]
    [SerializeField]
    private float m_ExhaustHeat;

    [Range( 0, 1 )]
    [SerializeField]
    private float m_Boost;

    [SettingsProperty]
    public string Name
    {
        get => name;
        set => name = value;
    }

    //Wrap all members into get/set properties
    [SettingsProperty]
    public bool IsAccelerating
    {
        get => m_IsAccelerating;
        set => m_IsAccelerating = value;
    }

    [SettingsProperty]
    [SettingsRange( 0, 1 )]
    public float Turn
    {
        get => m_Turn;
        set => m_Turn = value;
    }

    [SettingsProperty]
    [SettingsRange( 0, 1 )]
    public float Wings
    {
        get => m_Wings;
        set => m_Wings = value;
    }

    [SettingsProperty]
    [SettingsRange( 0, 1 )]
    public float ExhaustUp
    {
        get => m_ExhaustUp;
        set => m_ExhaustUp = value;
    }

    [SettingsProperty]
    [SettingsRange( 0, 1 )]
    public float ExhaustDown
    {
        get => m_ExhaustDown;
        set => m_ExhaustDown = value;
    }

    [SettingsProperty]
    [SettingsRange( 0, 1 )]
    public float WheelDown
    {
        get => m_WheelDown;
        set => m_WheelDown = value;
    }

    [SettingsProperty]
    [SettingsRange( 0, 1 )]
    public float BrakeLights
    {
        get => m_BrakeLights;
        set => m_BrakeLights = value;
    }

    [SettingsProperty]
    [SettingsRange( 0, 1 )]
    public float BrakeAnimation
    {
        get => m_BrakeAnimation;
        set => m_BrakeAnimation = value;
    }

    [SettingsProperty]
    [SettingsRange( 0, 1 )]
    public float Energy
    {
        get => m_Energy;
        set => m_Energy = value;
    }

    [SettingsProperty]
    [SettingsRange( 0, 1 )]
    public float Turbo
    {
        get => m_Turbo;
        set => m_Turbo = value;
    }

    [SettingsProperty]
    public Color TurboColor
    {
        get => m_TurboColor;
        set => m_TurboColor = value;
    }

    [SettingsProperty]
    [SettingsRange( 0, 1 )]
    public float ExhaustHeat
    {
        get => m_ExhaustHeat;
        set => m_ExhaustHeat = value;
    }

    [SettingsProperty]
    [SettingsRange( 0, 1 )]
    public float Boost
    {
        get => m_Boost;
        set => m_Boost = value;
    }

}

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

    //Wrap all members into get only properties
    public float BrakeLights => m_BrakeLights;
    
    public float BrakeAnimation => m_BrakeAnimation;

    public float Energy => m_Energy;

    public float Turbo => m_Turbo;

    public Color TurboColor => m_TurboColor;

    public float ExhaustHeat => m_ExhaustHeat;

    public float Boost => m_Boost;
    
    public float Turn => m_Turn;
    
    public float Wings => m_Wings;
    
    public float ExhaustUp => m_ExhaustUp;
    
    public float ExhaustDown => m_ExhaustDown;
    
    public float WheelDown => m_WheelDown;
    
    public bool IsAccelerating => m_IsAccelerating;

}

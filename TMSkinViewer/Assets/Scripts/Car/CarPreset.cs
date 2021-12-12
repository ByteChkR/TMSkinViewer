using System;

using UnityEngine;

[Serializable]
[CreateAssetMenu( fileName = "CarPreset", menuName = "TMSkin/Presets/Car" )]
public class CarPreset : ScriptableObject
{

    [Range( 0, 1 )]
    [SerializeField]
    private float m_BrakeLights;

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

    public float Energy => m_Energy;

    public float Turbo => m_Turbo;

    public Color TurboColor => m_TurboColor;

    public float ExhaustHeat => m_ExhaustHeat;

    public float Boost => m_Boost;

}

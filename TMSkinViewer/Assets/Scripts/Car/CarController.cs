﻿using System.Collections;

using UnityEngine;

public class CarController : MonoBehaviour
{

    [SerializeField]
    private Animator m_Animator;

    [SerializeField]
    private Renderer m_SkinRenderer;

    [SerializeField]
    private Renderer m_DetailRenderer;

    [SerializeField]
    private Renderer m_GlassRenderer;

    [SerializeField]
    private Renderer m_WheelRenderer;

    private Material[] CarMaterials =>
        new[]
        {
            m_SkinRenderer.material, m_DetailRenderer.material, m_GlassRenderer.material, m_WheelRenderer.material,
        };

    private IEnumerator SurfaceChangeAnimation( float start, float end, float time )
    {
        float t = 0;

        while ( t < time )
        {
            t += Time.deltaTime;
            float value = Mathf.Lerp( start, end, t / time );
            DirtLevel = value;

            yield return null;
        }
    }

    public void SetSurfacePreset( SurfacePreset preset )
    {
        if ( !preset.ApplySurfaceColor )
        {
            StartCoroutine( SurfaceChangeAnimation( DirtLevel, 0, 1 ) );
        }
        else
        {
            DirtColor = preset.SurfaceColor;
            StartCoroutine( SurfaceChangeAnimation( DirtLevel, 1, 1 ) );
        }
    }

    public void SetMapPreset( MapPreset preset )
    {
        LightsOn = preset.IsNight;

        if ( preset.IsNight )
        {
            SelfIlluminationNight = 1;
            BrakeLight = 1;
        }
        else
        {
            SelfIlluminationNight = 0;
            BrakeLight = 0;
        }
    }

    public void SetCarPreset( CarPreset preset )
    {
        BrakeLight = preset.BrakeLights;
        BrakeAnimation = preset.BrakeAnimation;
        IsAccelerating = preset.IsAccelerating;
        Turn = preset.Turn;
        Wings = preset.Wings;
        ExhaustUp = preset.ExhaustUp;
        ExhaustDown = preset.ExhaustDown;
        WheelDown = preset.WheelDown;
        Boost = preset.Boost;
        Energy = preset.Energy;
        Turbo = preset.Turbo;
        TurboColor = preset.TurboColor;
        ExhaustHeat = preset.ExhaustHeat;
    }

    private void SetAnimation( string name, float weight )
    {
        int layer = m_Animator.GetLayerIndex( name );
        m_Animator.SetLayerWeight( layer, weight );
        m_Animator.Play( name, layer, 0 );
    }

    #region Dirt Settings

    private float m_DirtLevel = 0.0f;
    private Color m_DirtColor;

    public float DirtLevel
    {
        get => m_DirtLevel;
        set
        {
            if ( value > 1.0f )
            {
                m_DirtLevel = 1.0f;
            }
            else if ( value < 0.0f )
            {
                m_DirtLevel = 0.0f;
            }
            else
            {
                m_DirtLevel = value;
            }

            SetDirtLevel( m_DirtLevel );
        }
    }

    public Color DirtColor
    {
        get => m_DirtColor;
        set
        {
            m_DirtColor = value;
            SetDirtColor( m_DirtColor );
        }
    }

    private void SetDirtLevel( float level )
    {
        foreach ( Material mat in CarMaterials )
        {
            mat.SetFloat( "_DirtIntensity", level );
        }
    }

    private void SetDirtColor( Color color )
    {
        foreach ( Material material in CarMaterials )
        {
            material.SetColor( "_DirtColor", color );
        }
    }

    #endregion

    #region Wing Settings

    private float m_Wings = 0.0f;

    public float Wings
    {
        get => m_Wings;
        set
        {
            m_Wings = Mathf.Clamp01( value );
            SetAnimation( "Wings", m_Wings );
        }
    }

    #endregion

    #region Exhaust Settings

    private float m_ExhaustUp = 0.0f;
    private float m_ExhaustDown = 0.0f;

    public float ExhaustUp
    {
        get => m_ExhaustUp;
        set
        {
            m_ExhaustUp = Mathf.Clamp01( value );
            SetAnimation( "ExhaustUp", m_ExhaustUp );
        }
    }

    public float ExhaustDown
    {
        get => m_ExhaustDown;
        set
        {
            m_ExhaustDown = Mathf.Clamp01( value );
            SetAnimation( "ExhaustDown", m_ExhaustDown );
        }
    }

    #endregion

    #region Turn Settings

    private float m_Turn = 0.5f;

    public float Turn
    {
        get => m_Turn;
        set
        {
            m_Turn = Mathf.Clamp01( value );

            if ( m_Turn < 0.5f )
            {
                SetAnimation( "TurnLeft", m_Turn * 2 );
                SetAnimation( "TurnRight", 0 );
            }
            else if ( m_Turn > 0.5f )
            {
                SetAnimation( "TurnRight", ( m_Turn - 0.5f ) * 2 );
                SetAnimation( "TurnLeft", 0 );
            }
        }
    }

    #endregion

    #region ReleaseAcceleration Settings

    private bool m_IsAccelerating = false;

    public bool IsAccelerating
    {
        get => m_IsAccelerating;
        set
        {
            m_IsAccelerating = value;
            SetAnimation( "ReleasePanels", m_IsAccelerating ? 0 : 1 );
        }
    }

    #endregion

    #region Wheel Settings
    
    private float m_WheelDown = 0.0f;
    
    public float WheelDown
    {
        get => m_WheelDown;
        set
        {
            m_WheelDown = Mathf.Clamp01( value );
            SetAnimation( "WheelsDown", m_WheelDown );
        }
    }

    #endregion

    #region Front Light Settings

    private bool m_LightsOn = false;

    [SerializeField]
    private Light[] m_Lights;

    public bool LightsOn
    {
        get => m_LightsOn;
        set
        {
            m_LightsOn = value;
            SetCarLights( m_LightsOn );
        }
    }

    private void SetCarLights( bool enable )
    {
        foreach ( Material material in CarMaterials )
        {
            material.SetFloat( "_FrontLights", enable ? 2 : 0 );
        }

        foreach ( Light light in m_Lights )
        {
            light.enabled = enable;
        }
    }

    #endregion

    #region Brake Light Settings

    private float m_BrakeAnimationStrength = 0.0f;
    private float m_BrakeLightIntensity = 0.0f;

    public float BrakeAnimation
    {
        get => m_BrakeAnimationStrength;
        set
        {
            m_BrakeAnimationStrength = Mathf.Clamp01( value );
            SetAnimation( "BrakePanels", m_BrakeAnimationStrength );
        }
    }

    public float BrakeLight
    {
        get => m_BrakeLightIntensity;
        set
        {
            if ( value > 2.0f )
            {
                m_BrakeLightIntensity = 2.0f;
            }
            else if ( value < 0.0f )
            {
                m_BrakeLightIntensity = 0.0f;
            }
            else
            {
                m_BrakeLightIntensity = value;
            }

            SetBrakeLight( m_BrakeLightIntensity );
        }
    }

    private void SetBrakeLight( float intensity )
    {
        foreach ( Material material in CarMaterials )
        {
            material.SetFloat( "_BrakeLights", intensity );
        }
    }

    #endregion

    #region Energy Settings

    private float m_Energy = 0.0f;

    public float Energy
    {
        get => m_Energy;
        set
        {
            if ( value > 1.0f )
            {
                m_Energy = 1.0f;
            }
            else if ( value < 0.0f )
            {
                m_Energy = 0.0f;
            }
            else
            {
                m_Energy = value;
            }

            SetEnergy( m_Energy );
        }
    }

    private void SetEnergy( float energy )
    {
        foreach ( Material material in CarMaterials )
        {
            material.SetFloat( "_Energy", energy );
        }
    }

    #endregion

    #region Self-Illumination Settings

    private float m_SelfIllumination = 0.0f;

    public float SelfIllumination
    {
        get => m_SelfIllumination;
        set
        {
            if ( value > 1.0f )
            {
                m_SelfIllumination = 1.0f;
            }
            else if ( value < 0.0f )
            {
                m_SelfIllumination = 0.0f;
            }
            else
            {
                m_SelfIllumination = value;
            }

            SetSelfIllumination( m_SelfIllumination );
        }
    }

    private void SetSelfIllumination( float illumination )
    {
        foreach ( Material material in CarMaterials )
        {
            material.SetFloat( "_SelfIllumination", illumination );
        }
    }

    #endregion

    #region Turbo Settings

    private float m_Turbo = 0.0f;
    private Color m_TurboColor;

    public float Turbo
    {
        get => m_Turbo;
        set
        {
            if ( value > 1.0f )
            {
                m_Turbo = 1.0f;
            }
            else if ( value < 0.0f )
            {
                m_Turbo = 0.0f;
            }
            else
            {
                m_Turbo = value;
            }

            SetTurbo( m_Turbo );
        }
    }

    public Color TurboColor
    {
        get => m_TurboColor;
        set
        {
            m_TurboColor = value;
            SetTurboColor( m_TurboColor );
        }
    }

    private void SetTurboColor( Color color )
    {
        foreach ( Material material in CarMaterials )
        {
            material.SetColor( "_TurboColor", color );
        }
    }

    private void SetTurbo( float turbo )
    {
        foreach ( Material material in CarMaterials )
        {
            material.SetFloat( "_TurboIntensity", turbo );
        }
    }

    #endregion

    #region Exhaust Heat Settings

    private float m_ExhaustHeat = 0.0f;

    public float ExhaustHeat
    {
        get => m_ExhaustHeat;
        set
        {
            if ( value > 1.0f )
            {
                m_ExhaustHeat = 1.0f;
            }
            else if ( value < 0.0f )
            {
                m_ExhaustHeat = 0.0f;
            }
            else
            {
                m_ExhaustHeat = value;
            }

            SetExhaustHeat( m_ExhaustHeat );
        }
    }

    private void SetExhaustHeat( float heat )
    {
        foreach ( Material material in CarMaterials )
        {
            material.SetFloat( "_ExhaustHeat", heat );
        }
    }

    #endregion

    #region Boost Settings

    private float m_Boost = 0.0f;

    public float Boost
    {
        get => m_Boost;
        set
        {
            if ( value > 1.0f )
            {
                m_Boost = 1.0f;
            }
            else if ( value < 0.0f )
            {
                m_Boost = 0.0f;
            }
            else
            {
                m_Boost = value;
            }

            SetBoost( m_Boost );
        }
    }

    private void SetBoost( float boost )
    {
        foreach ( Material material in CarMaterials )
        {
            material.SetFloat( "_Boost", boost );
        }
    }

    #endregion

    #region Self-Illumination-Night Settings

    private float m_SelfIlluminationNight = 0.0f;

    public float SelfIlluminationNight
    {
        get => m_SelfIlluminationNight;
        set
        {
            if ( value > 1.0f )
            {
                m_SelfIlluminationNight = 1.0f;
            }
            else if ( value < 0.0f )
            {
                m_SelfIlluminationNight = 0.0f;
            }
            else
            {
                m_SelfIlluminationNight = value;
            }

            SetSelfIlluminationNight( m_SelfIlluminationNight );
        }
    }

    private void SetSelfIlluminationNight( float illumination )
    {
        foreach ( Material material in CarMaterials )
        {
            material.SetFloat( "_SelfIlluminationNight", illumination );
        }
    }

    #endregion

}

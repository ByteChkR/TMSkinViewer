using System.Collections;
using System.IO;

using UnityEngine;

public class CarController : MonoBehaviour
{

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

    private void Update()
    {
        UpdateWheels();
    }

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
        }
        else
        {
            SelfIlluminationNight = 0;
        }
    }

    public void SetCarPreset( CarPreset preset )
    {
        BrakeLight = preset.BrakeLights;
        Boost = preset.Boost;
        Energy = preset.Energy;
        Turbo = preset.Turbo;
        TurboColor = preset.TurboColor;
        ExhaustHeat = preset.ExhaustHeat;
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
            material.SetFloat( "_FrontLights", enable ? 1 : 0 );
        }

        foreach ( Light light in m_Lights )
        {
            light.enabled = enable;
        }
    }

    #endregion

    #region Brake Light Settings

    private float m_BrakeLightIntensity = 0.0f;

    public float BrakeLight
    {
        get => m_BrakeLightIntensity;
        set
        {
            if ( value > 1.0f )
            {
                m_BrakeLightIntensity = 1.0f;
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

    #region Wheel Settings

    private float m_WheelAcceleration = 0.02f;

    public float WheelAcceleration
    {
        get => m_WheelAcceleration;
        set => m_WheelAcceleration = value;
    }

    private float m_WheelFriction = 0.99f;

    public float WheelFriction
    {
        get => m_WheelFriction;
        set => m_WheelFriction = value;
    }

    private float m_WheelSpeed = 0;
    private float m_WheelPosition = 0;

    private void UpdateWheels()
    {
        m_WheelSpeed += m_WheelAcceleration * Time.deltaTime;
        m_WheelSpeed *= m_WheelFriction;
        m_WheelPosition += m_WheelSpeed;

        Vector2 v = new Vector2( 0, m_WheelPosition );
        m_WheelRenderer.material.SetTextureOffset( "_MainTex", v );
        m_WheelRenderer.material.SetTextureOffset( "_AO", v );
        m_WheelRenderer.material.SetTextureOffset( "_Normals", v );
        m_WheelRenderer.material.SetTextureOffset( "_Emission", v );
        m_WheelRenderer.material.SetTextureOffset( "_DirtMask", v );
        m_WheelRenderer.material.SetTextureOffset( "_Roughness", v );
    }

    #endregion

}

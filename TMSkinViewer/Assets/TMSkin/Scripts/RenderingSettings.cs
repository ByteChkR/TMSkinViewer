using UnityEngine;

[SettingsCategory( "Rendering" )]
public class RenderingSettings : MonoBehaviour
{

    [SerializeField]
    private Light m_Light;

    [SettingsHeader( "Light Settings" )]
    [SettingsProperty]
    public Color SunColor
    {
        get => m_Light.color;
        set => m_Light.color = value;
    }

    [SettingsProperty]
    [SettingsRange( 0, 5 )]
    public float SunIntensity
    {
        get => m_Light.intensity;
        set => m_Light.intensity = value;
    }

    private void Awake()
    {
        SettingsManager.AddSettingsObject( this );
    }

    private void Start()
    {
        m_Light = GameObject.Find( "SunLight" ).GetComponent < Light >();
    }

}

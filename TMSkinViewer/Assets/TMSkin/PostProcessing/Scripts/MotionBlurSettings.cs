using System.Linq;

using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[SettingsCategory("Rendering/Motion Blur")]
public class MotionBlurSettings : MonoBehaviour
{

    [SerializeField]
    private PostProcessProfile m_Profile;

    [SettingsProperty( "Enable Motion Blur" )]
    public bool EnableMotionBlur
    {
        get
        {
            PostProcessEffectSettings es = m_Profile.settings.FirstOrDefault( x => x is MotionBlur );

            if ( es != null )
            {
                return es.enabled;
            }

            return false;
        }
        set
        {
            PostProcessEffectSettings es = m_Profile.settings.FirstOrDefault( x => x is MotionBlur );

            if ( es != null )
            {
                es.enabled.value = value;
            }
        }
    }
    [SettingsProperty( "Shutter Angle" )]
    [SettingsRange(0, 360)]
    public float ShutterAngle
    {
        get
        {
            
            MotionBlur es = m_Profile.settings.FirstOrDefault( x => x is MotionBlur ) as MotionBlur;

            if ( es == null )
            {
                return 0;
            }

            return es.shutterAngle;
        }
        set
        {
            MotionBlur es = m_Profile.settings.FirstOrDefault( x => x is MotionBlur ) as MotionBlur;
            if ( es == null )
            {
                return;
            }
            es.shutterAngle.value = value;
        }
    }
    
    [SettingsProperty( "Sample Count" )]
    [SettingsRange(4, 32)]
    public int SampleCount
    {
        get
        {
            
            MotionBlur es = m_Profile.settings.FirstOrDefault( x => x is MotionBlur ) as MotionBlur;

            if ( es == null )
            {
                return 0;
            }

            return es.sampleCount;
        }
        set
        {
            MotionBlur es = m_Profile.settings.FirstOrDefault( x => x is MotionBlur ) as MotionBlur;
            if ( es == null )
            {
                return;
            }
            es.sampleCount.value = value;
        }
    }


    private void Awake()
    {
        SettingsManager.AddSettingsObject(this);
    }

}
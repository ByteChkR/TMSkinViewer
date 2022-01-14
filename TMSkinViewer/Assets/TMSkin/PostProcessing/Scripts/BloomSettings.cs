

using System.Linq;

using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[SettingsCategory("Rendering/Bloom")]
public class BloomSettings :MonoBehaviour
{

    [SerializeField]
    private PostProcessProfile m_Profile;

    [SettingsProperty("Enable Bloom")]
    public bool EnableBloom
    {
        get
        {
            PostProcessEffectSettings es = m_Profile.settings.FirstOrDefault( x => x is Bloom );
            if(es!=null)
            {
                return es.enabled;
            }

            return false;
        }
        set
        {
            PostProcessEffectSettings es = m_Profile.settings.FirstOrDefault( x => x is Bloom );

            if ( es != null )
            {
                es.enabled.value = value;
            }
        }
    }
    
    [SettingsProperty("Bloom Fast Mode")]
    public bool BloomFastMode
    {
        get
        {
            Bloom es = m_Profile.settings.FirstOrDefault( x => x is Bloom ) as Bloom;
            if(es!=null)
            {
                return es.fastMode;
            }

            return false;
        }
        set
        {
            Bloom es = m_Profile.settings.FirstOrDefault( x => x is Bloom ) as Bloom;

            if ( es != null )
            {
                es.fastMode.value = value;
            }
        }
    }

    [SettingsProperty( "Bloom Intensity" )]
    public float BloomIntensity
    {
        get
        {
            
            Bloom es = m_Profile.settings.FirstOrDefault( x => x is Bloom ) as Bloom;

            if ( es == null )
            {
                return 0;
            }

            return es.intensity;
        }
        set
        {
            Bloom es = m_Profile.settings.FirstOrDefault( x => x is Bloom ) as Bloom;
            if ( es == null )
            {
                return;
            }
            es.intensity.value = value;
        }
    }
    
    [SettingsProperty( "Bloom Threshold" )]
    public float BloomThreshold
    {
        get
        {
            
            Bloom es = m_Profile.settings.FirstOrDefault( x => x is Bloom ) as Bloom;

            if ( es == null )
            {
                return 0;
            }

            return es.threshold;
        }
        set
        {
            Bloom es = m_Profile.settings.FirstOrDefault( x => x is Bloom ) as Bloom;
            if ( es == null )
            {
                return;
            }
            es.threshold.value = value;
        }
    }
    
    [SettingsProperty( "Bloom Clamp" )]
    public float BloomClamp
    {
        get
        {
            
            Bloom es = m_Profile.settings.FirstOrDefault( x => x is Bloom ) as Bloom;

            if ( es == null )
            {
                return 0;
            }

            return es.clamp;
        }
        set
        {
            Bloom es = m_Profile.settings.FirstOrDefault( x => x is Bloom ) as Bloom;
            if ( es == null )
            {
                return;
            }
            es.clamp.value = value;
        }
    }
    
    [SettingsProperty( "Bloom Soft Knee" )]
    [SettingsRange(0, 1)]
    public float BloomSoftKnee
    {
        get
        {
            
            Bloom es = m_Profile.settings.FirstOrDefault( x => x is Bloom ) as Bloom;

            if ( es == null )
            {
                return 0;
            }

            return es.softKnee;
        }
        set
        {
            Bloom es = m_Profile.settings.FirstOrDefault( x => x is Bloom ) as Bloom;
            if ( es == null )
            {
                return;
            }
            es.softKnee.value = value;
        }
    }
    
    [SettingsProperty( "Bloom Diffusion" )]
    [SettingsRange(0, 10)]
    public float BloomDiffusion
    {
        get
        {
            
            Bloom es = m_Profile.settings.FirstOrDefault( x => x is Bloom ) as Bloom;

            if ( es == null )
            {
                return 0;
            }

            return es.diffusion;
        }
        set
        {
            Bloom es = m_Profile.settings.FirstOrDefault( x => x is Bloom ) as Bloom;
            if ( es == null )
            {
                return;
            }
            es.diffusion.value = value;
        }
    }
    
    [SettingsProperty( "Bloom Anamorphic Ratio" )]
    [SettingsRange(-1, 1)]
    public float BloomAnamorphicRatio
    {
        get
        {
            
            Bloom es = m_Profile.settings.FirstOrDefault( x => x is Bloom ) as Bloom;

            if ( es == null )
            {
                return 0;
            }

            return es.anamorphicRatio;
        }
        set
        {
            Bloom es = m_Profile.settings.FirstOrDefault( x => x is Bloom ) as Bloom;
            if ( es == null )
            {
                return;
            }
            es.anamorphicRatio.value = value;
        }
    }
    
    [SettingsProperty( "Bloom Color" )]
    public Color BloomColor
    {
        get
        {
            
            Bloom es = m_Profile.settings.FirstOrDefault( x => x is Bloom ) as Bloom;

            if ( es == null )
            {
                return Color.white;
            }

            return es.color;
        }
        set
        {
            Bloom es = m_Profile.settings.FirstOrDefault( x => x is Bloom ) as Bloom;
            if ( es == null )
            {
                return;
            }
            es.color.value = value;
        }
    }



    private void Awake()
    {
        SettingsManager.AddSettingsObject(this);
    }

}



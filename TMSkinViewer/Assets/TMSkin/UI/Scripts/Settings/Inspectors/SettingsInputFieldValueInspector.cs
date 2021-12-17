using JetBrains.Annotations;

using UnityEngine;
using UnityEngine.UI;

namespace UI.Settings
{

    public class SettingsInputFieldValueInspector : SettingsValueInspector
    {

        [SerializeField]
        private InputField m_Field;
        [SerializeField]
        private InputField m_SliderField;
        [SerializeField]
        private GameObject m_FieldContainer;
        [SerializeField]
        private GameObject m_SliderContainer;

        [SerializeField]
        private Slider m_Slider;
        
        public override void SetProperty( SettingsPropertyWrapper prop )
        {
            base.SetProperty( prop );

            SettingsRangeAttribute range = prop.GetCustomAttribute<SettingsRangeAttribute>();

            if ( range != null && (prop.Type == typeof(int) || prop.Type == typeof(float)) )
            {
                m_SliderContainer.SetActive(true);
                m_FieldContainer.SetActive(false);
                m_Slider.minValue = range.Min;
                m_Slider.maxValue = range.Max;
                

                m_Slider.onValueChanged.AddListener(
                                                    v =>
                                                    {
                                                        if ( prop.Type == typeof( int ) )
                                                        {
                                                            int vi = ( int )v;
                                                            prop.Value = vi;
                                                            m_SliderField.SetTextWithoutNotify( vi.ToString());
                                                        }
                                                        else
                                                        {
                                                            prop.Value = v;
                                                            m_SliderField.SetTextWithoutNotify( v.ToString());
                                                        }
                                                    }
                                                   );
                
                m_SliderField.onEndEdit.AddListener(
                                                    v =>
                                                    {
                                                        if ( prop.Type == typeof( int ) )
                                                        {
                                                            int vi = int.Parse(v);
                                                            prop.Value = vi;
                                                            m_Slider.SetValueWithoutNotify( vi);
                                                        }
                                                        else
                                                        {
                                                            float vf = float.Parse(v);
                                                            prop.Value = vf;
                                                            m_Slider.SetValueWithoutNotify( vf);
                                                        }
                                                    }
                                                   );

                if ( prop.Type == typeof( int ) )
                {
                    int vi = ( int )prop.Value;
                    m_Slider.wholeNumbers = true;
                    m_Slider.value = vi;
                }
                else
                {
                    float vf = ( float )prop.Value;
                    m_Slider.wholeNumbers = false;
                    m_Slider.value = vf;
                }
            }
            else
            {
                m_SliderContainer.SetActive(false);
                m_FieldContainer.SetActive(true);
                m_Field.text = prop.Value?.ToString() ?? "";

                m_Field.onEndEdit.AddListener(
                                              v =>
                                              {
                                                  if ( prop.Type == typeof( string ) )
                                                  {
                                                      prop.Value = v;
                                                  }
                                                  else if ( prop.Type == typeof( int ) )
                                                  {
                                                      prop.Value = int.Parse( v );
                                                  }
                                                  else if ( prop.Type == typeof( float ) )
                                                  {
                                                      prop.Value = float.Parse( v );
                                                  }
                                              }
                                             );
            }
            
        }

    }



}

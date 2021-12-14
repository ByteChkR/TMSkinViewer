using UnityEngine;
using UnityEngine.UI;

namespace UI.Settings
{

    public class SettingsInputFieldValueInspector : SettingsValueInspector
    {

        [SerializeField]
        private InputField m_Field;
        public override void SetProperty( SettingsPropertyWrapper prop )
        {
            base.SetProperty( prop );

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
                                                  prop.Value = int.Parse(v);
                                              }
                                              else if ( prop.Type == typeof( float ) )
                                              {
                                                  prop.Value = float.Parse(v);
                                              }
                                          });
        }
        

    }

}

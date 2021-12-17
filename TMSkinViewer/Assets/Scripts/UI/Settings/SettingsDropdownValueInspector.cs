using System;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

namespace UI.Settings
{

    public class SettingsDropdownValueInspector : SettingsValueInspector
    {

        [SerializeField]
        private Dropdown m_Field;

        public override void SetProperty( SettingsPropertyWrapper prop )
        {
            base.SetProperty( prop );

            string[] values = Enum.GetNames( prop.Type );

            m_Field.options.Clear();
            m_Field.options.AddRange( values.Select( x => new Dropdown.OptionData( x ) ) );

            string v = prop.Value.ToString();

            for ( int i = 0; i < values.Length; i++ )
            {
                if ( v == values[i] )
                {
                    m_Field.value = i;
                }
            }

            m_Field.onValueChanged.AddListener(
                                               v => { prop.Value = Enum.Parse( prop.Type, m_Field.options[v].text ); }
                                              );
        }

    }

}

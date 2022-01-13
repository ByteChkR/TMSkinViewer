using System.Collections.Generic;

using UnityEngine;

namespace Themes
{

    public class ThemeManager : MonoBehaviour
    {

        private static ThemeManager s_Instance;

        [SerializeField]
        private ThemeSettings m_Settings;

        private readonly List < ThemeElement > m_Elements = new List < ThemeElement >();

        private void Awake()
        {
            s_Instance = this;
#if UNITY_EDITOR
            m_Settings = Instantiate( m_Settings );
#endif
            m_Settings.OnSettingsChanged += ApplyToAllElements;
        }

        private void Start()
        {
            SettingsManager.AddSettingsObject( m_Settings );
        }

        private static void ApplyToAllElements()
        {
            for ( int i = s_Instance.m_Elements.Count - 1; i >= 0; i-- )
            {
                ThemeElement elem = s_Instance.m_Elements[i];

                if ( elem == null )
                {
                    s_Instance.m_Elements.RemoveAt( i );
                }
                else
                {
                    Apply( elem );
                }
            }
        }

        public static void Apply( ThemeElement elem )
        {
            if ( !s_Instance.m_Elements.Contains( elem ) )
            {
                s_Instance.m_Elements.Add( elem );
            }

            foreach ( string selector in elem.ThemeSelectors )
            {
                foreach ( ThemeSelectorTarget target in s_Instance.m_Settings.Targets )
                {
                    if ( target.Name == selector )
                    {
                        target.ApplyTheme( elem.gameObject );

                        return;
                    }
                }
            }
        }

    }

}

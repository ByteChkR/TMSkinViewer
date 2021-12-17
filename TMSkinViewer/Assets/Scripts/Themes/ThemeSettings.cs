using System;

using UnityEngine;

namespace Themes
{

    [CreateAssetMenu( fileName = "New Theme", menuName = "TMSkin/Theme" )]
    [SettingsCategory( "Theme" )]
    public class ThemeSettings : ScriptableObject
    {

        [SettingsProperty( "Targets" )]
        public ThemeSelectorTarget[] Targets;

        private void OnValidate()
        {
            OnSettingsChanged?.Invoke();
        }

        public event Action OnSettingsChanged;

    }

}

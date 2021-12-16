using System;

using UnityEngine;

namespace Themes
{

    [CreateAssetMenu( fileName = "New Theme", menuName = "TMSkin/Theme" )]
    [SettingsCategory("Theme")]
    public class ThemeSettings : ScriptableObject
    {

        public event Action OnSettingsChanged;

        private void OnValidate()
        {
            OnSettingsChanged?.Invoke();
        }

        [SettingsProperty("Targets")]
        public ThemeSelectorTarget[] Targets;

    }

}
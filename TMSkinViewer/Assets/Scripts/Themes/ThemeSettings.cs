using System;

using UnityEngine;

namespace Themes
{

    [CreateAssetMenu( fileName = "New Theme", menuName = "TMSkin/Theme" )]
    public class ThemeSettings : ScriptableObject
    {

        public event Action OnSettingsChanged;

        private void OnValidate()
        {
            OnSettingsChanged?.Invoke();
        }

        public ThemeSelectorTarget[] Targets;

    }

}
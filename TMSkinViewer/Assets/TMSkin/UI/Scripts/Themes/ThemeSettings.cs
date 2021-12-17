using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Themes
{

    [CreateAssetMenu( fileName = "New Theme", menuName = "TMSkin/Theme" )]
    [SettingsCategory( "Theme" )]
    public class ThemeSettings : ScriptableObject, ISettingsObject
    {

        [SerializeField]
        private List<ThemeSelectorTarget> m_Targets;
        

        [SettingsProperty]
        public ThemeSelectorTarget[] Targets
        {
            get => m_Targets.ToArray();
            set => m_Targets = value.ToList();
        }

        private void OnValidate()
        {
            OnSettingsChanged?.Invoke();
        }

        public event Action OnSettingsChanged;

        void ISettingsObject.OnSettingsChanged()
        {
            OnSettingsChanged?.Invoke();
        }

        public void OnObjectLoaded()
        {
        }

    }

}

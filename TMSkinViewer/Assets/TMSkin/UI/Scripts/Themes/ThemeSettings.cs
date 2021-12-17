using System;
using System.Linq;

using UnityEngine;

namespace Themes
{

    [CreateAssetMenu( fileName = "New Theme", menuName = "TMSkin/Theme" )]
    [SettingsCategory( "Theme" )]
    public class ThemeSettings : ScriptableObject, ISettingsObject
    {

        public ThemeSelectorTarget[] Targets;


        private void OnValidate()
        {
            OnSettingsChanged?.Invoke();
        }

        public event Action OnSettingsChanged;

        void ISettingsObject.OnSettingsChanged()
        {
            OnSettingsChanged?.Invoke();
        }

        void ISettingsObject.OnObjectLoaded()
        {
            for ( int i = 0; i < Targets.Length; i++ )
            {
                SettingsManager.AddSettingsObject( Targets[i], $"Theme/{Targets[i].Name}" );
                Targets[i].OnSettingsChanged += OnValidate;
            }
        }

    }

}

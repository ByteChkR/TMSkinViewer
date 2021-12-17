using System;

using UnityEngine;
using UnityEngine.UI;

namespace Themes
{

    [Serializable]
    public class ThemeSelectorTarget: ISettingsObject
    {
        [SerializeField]
        private string m_Name;
        [SerializeField]
        private Color m_TextColor;
        [SerializeField]
        private Color m_BackgroundColor;
        public ColorBlock SelectableColors;
        
        public Action OnSettingsChanged;

        
        [SettingsProperty]
        public string Name
        {
            get => m_Name;
            set => m_Name = value;
        }
        
        [SettingsProperty]
        public Color TextColor
        {
            get => m_TextColor;
            set => m_TextColor = value;
        }
        
        [SettingsProperty]
        public Color BackgroundColor
        {
            get => m_BackgroundColor;
            set => m_BackgroundColor = value;
        }

        [SettingsProperty]
        public Color NormalColor {get=>SelectableColors.normalColor; set => SelectableColors.normalColor = value;}

        [SettingsProperty]
        public Color HighlightedColor
        {
            get => SelectableColors.highlightedColor;
            set => SelectableColors.highlightedColor = value;
        }
        
        [SettingsProperty]
        public Color PressedColor
        {
            get => SelectableColors.pressedColor;
            set => SelectableColors.pressedColor = value;
        }
        
        [SettingsProperty]
        public Color DisabledColor
        {
            get => SelectableColors.disabledColor;
            set => SelectableColors.disabledColor = value;
        }
        
        [SettingsProperty]
        public float ColorMultiplier
        {
            get => SelectableColors.colorMultiplier;
            set => SelectableColors.colorMultiplier = value;
        }
        
        [SettingsProperty]
        public float FadeDuration
        {
            get => SelectableColors.fadeDuration;
            set => SelectableColors.fadeDuration = value;
        }

        
        
        public void ApplyTheme( GameObject obj )
        {
            Text t = obj.GetComponent < Text >();

            Selectable s = obj.GetComponent < Selectable >();

            Image i = obj.GetComponent < Image >();

            if ( t != null )
            {
                t.color = TextColor;
            }
            else if ( s != null )
            {
                s.colors = SelectableColors;
            }
            else if ( i != null )
            {
                i.color = BackgroundColor;
            }
        }

        void ISettingsObject.OnSettingsChanged()
        {
            OnSettingsChanged?.Invoke();
        }

        public void OnObjectLoaded()
        {
        }

    }

}

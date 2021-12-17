using System;

using UnityEngine;
using UnityEngine.UI;

namespace Themes
{

    [Serializable]
    public class ThemeSelectorTarget
    {

        public string Name;
        public Color TextColor;
        public Color BackgroundColor;
        public ColorBlock SelectableColors;

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

    }

}

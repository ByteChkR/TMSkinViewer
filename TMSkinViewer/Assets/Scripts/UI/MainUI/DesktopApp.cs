using System;

using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    [Serializable]
    public class DesktopApp
    {

        public string Name;
        public Sprite Icon;
        public UnityEvent OnClick;

    }

}

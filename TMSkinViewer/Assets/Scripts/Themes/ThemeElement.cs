using System.Collections.Generic;

using UnityEngine;

namespace Themes
{

    public class ThemeElement : MonoBehaviour
    {

        [SerializeField]
        private string[] m_ThemeSelectors = new[] { "Default" };

        public IEnumerable < string > ThemeSelectors => m_ThemeSelectors;

        private void Start()
        {
            ThemeManager.Apply( this );
        }

    }

}

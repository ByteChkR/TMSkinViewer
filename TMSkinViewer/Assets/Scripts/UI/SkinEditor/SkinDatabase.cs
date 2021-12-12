using System;
using System.Collections.Generic;

using UnityEngine;

namespace UI.SkinEditorMainWindow
{

    public class SkinDatabase : MonoBehaviour
    {
        
        public static event Action OnSkinDatabaseChanged;
        private static SkinDatabase s_Instance;
        
        private void Awake()
        {
            s_Instance = this;
        }

        [SerializeField]
        private List < CarSkin > m_Skins = new List < CarSkin >();
        [SerializeField]
        private CarSkin m_DefaultSkin;
        public static IEnumerable < CarSkin > Skins => s_Instance.m_Skins;

        public static CarSkin CreateSkin( string name ) => CreateSkin( name, s_Instance.m_DefaultSkin );
        public static CarSkin CreateSkin(string name, CarSkin template)
        {
            CarSkin skin = Instantiate( template );
            s_Instance.m_Skins.Add( skin );
            skin.SkinName = name;
            OnSkinDatabaseChanged?.Invoke();
            return skin;
        }

    }

}
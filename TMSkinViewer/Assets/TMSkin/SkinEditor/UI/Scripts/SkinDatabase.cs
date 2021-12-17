using System;
using System.Collections.Generic;

using UnityEngine;

namespace UI.SkinEditorMainWindow
{

    [SettingsCategory("Car Skin Settings")]
    public class SkinDatabase : MonoBehaviour
    {

        private static SkinDatabase s_Instance;

        [SerializeField]
        private List < CarSkin > m_Skins = new List < CarSkin >();

        [SerializeField]
        private CarSkin m_DefaultSkin;
        
        [SettingsProperty]
        [SettingsHeader("Skin Settings")]
        public CarSkin DefaultSkin
        {
            get => m_DefaultSkin;
            set => m_DefaultSkin = value;
        }

        public static IEnumerable < CarSkin > LoadedSkins => s_Instance.m_Skins;

        [SettingsProperty]
        public CarSkin[] Skins
        {
            get => m_Skins.ToArray();
            set => m_Skins = new List<CarSkin>(value);
        }

        public CarSkin Default => s_Instance.DefaultSkin;


        private void Awake()
        {
            s_Instance = this;
            SettingsManager.AddSettingsObject(this);
        }

        public static event Action OnSkinDatabaseChanged;

        public static CarSkin CreateSkin( string name )
        {
            return CreateSkin( name, s_Instance.m_DefaultSkin );
        }

        public static CarSkin CreateSkin( string name, CarSkin template, bool createNewMaterials = false )
        {
            CarSkin skin = Instantiate( template );

            if ( createNewMaterials )
            {
                skin.Details = MaterialDatabase.CreateMaterial( $"{name}_Details", skin.Details );
                skin.Skin = MaterialDatabase.CreateMaterial( $"{name}_Skin", skin.Skin );
                skin.Glass = MaterialDatabase.CreateMaterial( $"{name}_Glass", skin.Glass );
                skin.Wheel = MaterialDatabase.CreateMaterial( $"{name}_Wheels", skin.Wheel );
            }

            s_Instance.m_Skins.Add( skin );
            skin.SkinName = name;
            OnSkinDatabaseChanged?.Invoke();

            return skin;
        }

    }

}

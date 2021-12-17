using System;
using System.Collections.Generic;

using UnityEngine;

namespace UI.SkinEditorMainWindow
{

    
    [SettingsCategory("Car Skin Settings")]
    public class MaterialDatabase : MonoBehaviour
    {

        private static MaterialDatabase s_Instance;

        [SerializeField]
        private List < CarMaterial > m_Materials = new List < CarMaterial >();

        [SerializeField]
        private CarMaterial m_DefaultMaterial;

        [SettingsProperty]
        [SettingsHeader("Material Settings")]
        public CarMaterial DefaultMaterial
        {
            get => m_DefaultMaterial;
            set => m_DefaultMaterial = value;
        }

        [SettingsProperty]
        public CarMaterial[] Materials
        {
            get => m_Materials.ToArray();
            set => m_Materials = new List < CarMaterial >(value);
        }
        public static IEnumerable < CarMaterial > LoadedMaterials => s_Instance.m_Materials;

        private void Awake()
        {
            s_Instance = this;
            SettingsManager.AddSettingsObject(this);
        }

        public static event Action OnMaterialDatabaseChanged;

        public static CarMaterial CreateMaterial( string name )
        {
            return CreateMaterial( name, s_Instance.m_DefaultMaterial );
        }

        public static CarMaterial CreateMaterial( string name, CarMaterial template )
        {
            CarMaterial material = Instantiate( template );
            material.MaterialName = name;
            s_Instance.m_Materials.Add( material );
            OnMaterialDatabaseChanged?.Invoke();

            return material;
        }

    }

}

using System;
using System.Collections.Generic;

using UnityEngine;

namespace UI.SkinEditorMainWindow
{

    public class MaterialDatabase : MonoBehaviour
    {

        public static event Action OnMaterialDatabaseChanged;
        private static MaterialDatabase s_Instance;

        private void Awake()
        {
            s_Instance = this;
        }

        [SerializeField]
        private List < CarMaterial > m_Materials = new List < CarMaterial >();
        [SerializeField]
        private CarMaterial m_DefaultMaterial;

        public static IEnumerable < CarMaterial > Materials => s_Instance.m_Materials;

        public static CarMaterial CreateMaterial( string name ) => CreateMaterial( name, s_Instance.m_DefaultMaterial );
        public static CarMaterial CreateMaterial(string name, CarMaterial template)
        {
            CarMaterial material = Instantiate(template);
            material.MaterialName = name;
            s_Instance.m_Materials.Add(material);
            OnMaterialDatabaseChanged?.Invoke();
            return material;
        }

    }

}
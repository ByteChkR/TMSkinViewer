using System;
using System.Linq;

using UnityEngine;

namespace UI.Settings
{

    public class SettingsInspector : MonoBehaviour
    {

        [SerializeField]
        private SettingsValueInspectorEntry[] m_Inspectors;

        [SerializeField]
        private Transform m_InspectorParent;

        private SettingsValueInspector CreateInspector( SettingsValueInspectorType type )
        {
            SettingsValueInspectorEntry e = m_Inspectors.FirstOrDefault( x => x.Type == type );

            if ( e == null )
                return null;
            return Instantiate( e.InspectorPrefab, m_InspectorParent ).
                GetComponent < SettingsValueInspector >();
        }

        private GameObject GetPrefab( SettingsValueInspectorType type )
        {
            SettingsValueInspectorEntry e = m_Inspectors.FirstOrDefault( x => x.Type == type );

            if ( e == null )
                return null;

            return e.InspectorPrefab;
        }
        public GameObject GetPrefab( Type t )
        {
            if ( t == typeof( string ) )
            {
                return  GetPrefab(SettingsValueInspectorType.String);
            }
            else if ( t == typeof( int ) )
            {
                return GetPrefab(SettingsValueInspectorType.Integer);
            }
            else if ( t == typeof( float ) )
            {
                return  GetPrefab(SettingsValueInspectorType.Decimal);
            }
            else if ( t.IsEnum )
            {
                return  GetPrefab(SettingsValueInspectorType.Enum);
            }
            else if ( t == typeof( Color ) )
            {
                return GetPrefab(SettingsValueInspectorType.Color);
            }
            else if ( t == typeof( Vector2 ) )
            {
                return GetPrefab(SettingsValueInspectorType.Vector2);
            }
            else if ( t == typeof( Vector3 ) )
            {
                return GetPrefab( SettingsValueInspectorType.Vector3 );
            }
            else if ( t.IsArray )
            {
                return GetPrefab( SettingsValueInspectorType.Array );
            }

            return null;
        }

        private void CreateInspectorValue( SettingsPropertyWrapper property )
        {
            SettingsValueInspector inspector = null;
            if ( property.Type == typeof( string ) )
            {
                inspector =  CreateInspector(SettingsValueInspectorType.String);
            }
            else if ( property.Type == typeof( int ) )
            {
                inspector = CreateInspector(SettingsValueInspectorType.Integer);
            }
            else if ( property.Type == typeof( float ) )
            {
                inspector =  CreateInspector(SettingsValueInspectorType.Decimal);
            }
            else if ( property.Type.IsEnum )
            {
                inspector =  CreateInspector(SettingsValueInspectorType.Enum);
            }
            else if ( property.Type == typeof( Color ) )
            {
                inspector =  CreateInspector(SettingsValueInspectorType.Color);
            }
            else if ( property.Type == typeof( Vector2 ) )
            {
                inspector = CreateInspector(SettingsValueInspectorType.Vector2);
            }
            else if ( property.Type == typeof( Vector3 ) )
            {
                inspector = CreateInspector( SettingsValueInspectorType.Vector3 );
            }
            else if ( property.Type.IsArray )
            {
                inspector = CreateInspector( SettingsValueInspectorType.Array );
            }

            if ( inspector != null )
            {
                inspector.SetProperty(property);
            }
            else
                Debug.Log($"No Inspector found for type {property.Type}"  );
        }
        
        public void SetCategory( SettingsCategory category )
        {
            foreach ( Transform inspector in m_InspectorParent )
            {
                Destroy(inspector.gameObject);
            }
            
            foreach ( SettingsObjectWrapper objectWrapper in category.Objects )
            {
                foreach ( SettingsPropertyWrapper property in objectWrapper.Properties )
                {
                    CreateInspectorValue(property);
                }
            }
        }

    }

}
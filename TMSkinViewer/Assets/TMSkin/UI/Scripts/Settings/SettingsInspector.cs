using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;
using UnityEngine.UI;

namespace UI.Settings
{

    public class SettingsInspector : MonoBehaviour
    {

        private static readonly List < SettingsValueInspectorEntry > m_CustomInspectors =
            new List < SettingsValueInspectorEntry >();

        private static CustomInspectorEntry s_FallbackInspector;

        [SerializeField]
        private List < SettingsValueInspectorEntry > m_Inspectors;

        [SerializeField]
        private Transform m_InspectorParent;

        [SerializeField]
        private GameObject m_InspectorButtonPrefab;

        [SerializeField]
        private GameObject m_InspectorHeaderPrefab;

        private void Awake()
        {
            m_Inspectors.AddRange( m_CustomInspectors );
        }

        public static void AddFallbackInspector( GameObject obj, Action < GameObject, object > onCreate )
        {
            s_FallbackInspector = new CustomInspectorEntry( typeof( object ), obj, onCreate, true );
        }

        public static void AddCustomInspector(
            Type t,
            GameObject obj,
            Action < GameObject, object > onCreate,
            bool allowInherited = false )
        {
            m_CustomInspectors.Add( new CustomInspectorEntry( t, obj, onCreate, allowInherited ) );
        }

        private static object CreateObjectInstance( Type t )
        {
            try
            {
                return t == typeof( string ) ? "" : Activator.CreateInstance( t );
            }
            catch ( Exception e )
            {
                return null;
            }
        }

        private SettingsValueInspector CreateInspector( Type type )
        {
            return CreateInspector( CreateObjectInstance( type ), type );
        }

        private SettingsValueInspector CreateInspector( object instance, Type type = null )
        {
            type ??= instance.GetType();
            SettingsValueInspectorEntry e = GetEntry( type );

            GameObject r = Instantiate( e.InspectorPrefab, m_InspectorParent );
            e.OnCreate( r, instance );

            return r.
                GetComponent < SettingsValueInspector >();
        }

        public SettingsValueInspectorEntry GetEntry( Type type )
        {
            SettingsValueInspectorEntry e = m_Inspectors.LastOrDefault( x => x.IsValidInspectorPropertyType( type ) );

            if ( e == null )
            {
                return s_FallbackInspector;
            }

            return e;
        }

        private void CreateButtonValue( object instance, string name, MethodInfo info )
        {
            GameObject button = Instantiate( m_InspectorButtonPrefab, m_InspectorParent );
            ButtonInspector inspector = button.GetComponent < ButtonInspector >();
            
            inspector.ButtonText.text = name;
            inspector.PropertyText.text = name;
            inspector.Button.onClick.AddListener( () => { info.Invoke( instance, null ); } );
        }

        private void CreateInspectorValue( SettingsPropertyWrapper property )
        {
            if ( property.Header != null )
            {
                GameObject header = Instantiate( m_InspectorHeaderPrefab, m_InspectorParent );
                header.GetComponentInChildren < Text >().text = property.Header.Name;
            }

            SettingsValueInspector inspector = property.Value != null
                                                   ? CreateInspector( property.Value )
                                                   : CreateInspector( property.Type );

            if ( inspector != null )
            {
                inspector.SetProperty( property );
            }
            else
            {
                Debug.Log( $"No Inspector found for type {property.Type}" );
            }
        }

        public void SetCategory( SettingsCategory category )
        {
            foreach ( Transform inspector in m_InspectorParent )
            {
                Destroy( inspector.gameObject );
            }

            foreach ( SettingsObjectWrapper objectWrapper in category.Objects )
            {
                foreach ( SettingsPropertyWrapper property in objectWrapper.Properties )
                {
                    CreateInspectorValue( property );
                }

                foreach ( (string name, MethodInfo info) method in objectWrapper.Methods )
                {
                    CreateButtonValue( objectWrapper.Instance, name, method.info );
                }
            }
        }

        private class CustomInspectorEntry : SettingsValueInspectorEntry
        {

            private readonly Type m_Type;
            private readonly Action < GameObject, object > m_OnCreate;
            private readonly bool m_AllowInherited;

            #region Public

            public CustomInspectorEntry(
                Type type,
                GameObject prefab,
                Action < GameObject, object > create,
                bool allowInherited = false )
            {
                m_Type = type;
                m_OnCreate = create;
                InspectorPrefab = prefab;
                Type = SettingsValueInspectorType.Custom;
                m_AllowInherited = allowInherited;
            }

            public override bool IsValidInspectorPropertyType( Type t )
            {
                return m_AllowInherited ? t.IsAssignableFrom( m_Type ) : t == m_Type;
            }

            public override void OnCreate( GameObject inspector, object instance )
            {
                base.OnCreate( inspector, instance );
                m_OnCreate( inspector, instance );
            }

            #endregion

        }

    }

}

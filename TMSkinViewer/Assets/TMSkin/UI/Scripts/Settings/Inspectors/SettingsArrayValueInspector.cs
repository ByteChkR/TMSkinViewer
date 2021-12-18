using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace UI.Settings
{

    public class SettingsArrayValueInspector : SettingsValueInspector
    {

        [SerializeField]
        private Transform m_InspectorContent;

        private readonly List < GameObject > m_InspectorItems = new List < GameObject >();

        private SettingsInspector m_Inspector;

        private SettingsValueInspectorEntry m_InspectorPrefab;

        private SettingsPropertyWrapper m_Wrapper;

        private void Awake()
        {
            m_Inspector = GetComponentInParent < SettingsInspector >();
        }

        public override void SetProperty( SettingsPropertyWrapper prop )
        {
            base.SetProperty( prop );

            foreach ( GameObject o in m_InspectorItems )
            {
                Destroy( o );
            }

            m_Wrapper = prop;

            m_InspectorPrefab = m_Inspector.GetEntry( prop.Type.GetElementType() );

            if ( m_InspectorPrefab == null )
            {
                Debug.Log( $"Can not get inspector for type {prop.Type.GetElementType()}" );

                gameObject.SetActive( false );

                return;
            }

            Array a = ( Array )prop.Value;

            for ( int i = 0; i < a.Length; i++ )
            {
                CreateInspectorUI( i );
            }
        }

        private void CreateInspectorUI( int i )
        {
            if ( m_InspectorPrefab == null )
            {
                Debug.LogError( "Can not create inspector" );

                return;
            }

            GameObject inspector = Instantiate( m_InspectorPrefab.InspectorPrefab, m_InspectorContent );

            object instance = ( ( Array )m_Wrapper.Value ).GetValue( i );
            m_InspectorPrefab.OnCreate(inspector, instance);
            m_InspectorItems.Add( inspector );

            inspector.transform.SetSiblingIndex( m_InspectorContent.childCount - 2 );

            SettingsValueInspector insp = inspector.
                GetComponent < SettingsValueInspector >();
            
            if ( insp == null )
                return;

            Action<object> setter = null;

            if ( m_Wrapper.CanWrite )
            {
                setter = o =>
                         {
                             Array cur = ( Array )m_Wrapper.Value;

                             cur.SetValue( o, i );
                         };
            }
            insp.SetProperty(
                             new SettingsArrayPropertyWrapper(
                                                              i.ToString(),
                                                              m_Wrapper.Type.GetElementType(),
                                                              setter,
                                                              () =>
                                                              {
                                                                  Array cur = ( Array )m_Wrapper.Value;

                                                                  return cur.GetValue( i );
                                                              },
                                                              null,
                                                              m_Wrapper.GetCustomAttributes<Attribute>()
                                                             )
                            );

            //Get Inspector UI Prefab for the element type
            //Create Inspector UI for each element
        }

        private object CreateInstance( Type t )
        {
            try
            {
                return Activator.CreateInstance( t );
            }
            catch ( Exception e )
            {
                return null;
            }
        }
        public void AddValue()
        {
            Type eType = m_Wrapper.Type.GetElementType();

            
            object o = eType == typeof( string ) ? "" : CreateInstance(eType);
            Array cur = ( Array )m_Wrapper.Value;

            Array a = Array.CreateInstance( eType, cur.Length + 1 );

            Array.Copy( cur, a, cur.Length );
            a.SetValue( o, cur.Length );
            m_Wrapper.Value = a;
            CreateInspectorUI( cur.Length );
        }

        public void RemoveValue()
        {
            Type eType = m_Wrapper.Type.GetElementType();
            Array cur = ( Array )m_Wrapper.Value;

            if ( cur.Length == 0 )
            {
                return;
            }

            Array a = Array.CreateInstance( eType, cur.Length - 1 );
            Array.Copy( cur, a, a.Length );
            m_Wrapper.Value = a;
            GameObject o = m_InspectorItems.LastOrDefault();

            if ( o != null )
            {
                m_InspectorItems.RemoveAt( m_InspectorItems.Count - 1 );
                Destroy( o );
            }
        }

        private class SettingsArrayPropertyWrapper : SettingsPropertyWrapper
        {

            private readonly IEnumerable <Attribute> m_Attributes;
            private readonly Action < object > m_Setter;
            private readonly Func < object > m_Getter;

            public override bool CanWrite => m_Setter != null;

            public override Type Type { get; }

            public override object Value
            {
                get => m_Getter();
                set => m_Setter?.Invoke( value );
            }

            public override IEnumerable < T > GetCustomAttributes < T >() => m_Attributes.Where(x=> x is T).Cast<T>();
            #region Public

            public SettingsArrayPropertyWrapper(
                string name,
                Type elemType,
                Action < object > set,
                Func < object > get, SettingsHeaderAttribute header,
                IEnumerable < Attribute > attributes ) : base( name, null, null, header )
            {
                Type = elemType;
                m_Setter = set;
                m_Getter = get;
                m_Attributes = attributes;
            }

            #endregion

        }

    }

}

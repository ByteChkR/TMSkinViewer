using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;

namespace UI.Settings
{

    public class SettingsArrayValueInspector : SettingsValueInspector
    {

        private class SettingsArrayPropertyWrapper : SettingsPropertyWrapper
        {

            public override Type Type { get; }

            public override object Value
            {
                get => m_Getter();
                set => m_Setter(value);
            }

            private readonly Action < object > m_Setter;
            private readonly Func < object > m_Getter;

            public SettingsArrayPropertyWrapper( string name, Type elemType, Action<object> set, Func<object> get ) : base( name, null, null )
            {
                Type = elemType;
                m_Setter = set;
                m_Getter = get;
            }

        }

        [SerializeField]
        private Transform m_InspectorContent;

        private SettingsInspector m_Inspector;

        private GameObject m_InspectorPrefab;

        private SettingsPropertyWrapper m_Wrapper;

        private readonly List < GameObject > m_InspectorItems  = new List < GameObject >();

        private void Awake()
        {
            m_Inspector = GetComponentInParent < SettingsInspector >();
        }

        public override void SetProperty( SettingsPropertyWrapper prop )
        {
            base.SetProperty(prop);
            
            
            foreach ( GameObject o in m_InspectorItems )
            {
                Destroy(o);
            }

            
            m_Wrapper = prop;

            m_InspectorPrefab = m_Inspector.GetPrefab( prop.Type.GetElementType() );

            if ( m_InspectorPrefab == null )
            {
                Debug.Log( $"Can not get inspector for type {prop.Type.GetElementType() }" );

                gameObject.SetActive(false);
                return;
            }

            Array a = ( Array )prop.Value;

            for ( int i = 0; i < a.Length; i++ )
            {
                CreateInspectorUI( i );
            }

        }

        private void CreateInspectorUI(int i)
        {
            if ( m_InspectorPrefab == null )
            {
                
                Debug.LogError( "Can not create inspector" );

                return;
            }
            SettingsValueInspector inspector =  Instantiate( m_InspectorPrefab, m_InspectorContent ).GetComponent<SettingsValueInspector>();
            m_InspectorItems.Add(inspector.gameObject  );
            inspector.transform.SetSiblingIndex(m_InspectorContent.childCount-2);
            inspector.SetProperty(
                                  new SettingsArrayPropertyWrapper(
                                                                   i.ToString(),
                                                                   m_Wrapper.Type.GetElementType(),
                                                                   o =>
                                                                   {
                                                                       Array cur = ( Array )m_Wrapper.Value;

                                                                       cur.SetValue(o,  i);
                                                                   },
                                                                   () =>
                                                                   {
                                                                       Array cur = ( Array )m_Wrapper.Value;

                                                                       return cur.GetValue( i );
                                                                   }
                                                                  )
                                 );

            //Get Inspector UI Prefab for the element type
            //Create Inspector UI for each element
        }

        public void AddValue()
        {
            Type eType = m_Wrapper.Type.GetElementType();



            object o = eType == typeof( string ) ? "" : Activator.CreateInstance( eType );
            Array cur = ( Array )m_Wrapper.Value;

            Array a = Array.CreateInstance( eType, cur.Length + 1 );
            
            Array.Copy(cur, a, cur.Length);
            a.SetValue(o, cur.Length);
            m_Wrapper.Value = a;
            CreateInspectorUI(cur.Length);
        }
        
        public void RemoveValue()
        {
            Type eType = m_Wrapper.Type.GetElementType();
            Array cur = ( Array )m_Wrapper.Value;

            if ( cur.Length == 0 )
                return;
            Array a = Array.CreateInstance( eType, cur.Length-1 );
            Array.Copy(cur, a, a.Length);
            m_Wrapper.Value = a;
            GameObject o = m_InspectorItems.LastOrDefault();

            if ( o != null )
            {
                m_InspectorItems.RemoveAt(m_InspectorItems.Count-1);
                Destroy( o );
            }
        }

    }

}

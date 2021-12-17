using System;

using UnityEngine;

namespace UI.ResourceSelector
{

    public class ResourceSelectorWindowBuilder : MonoBehaviour
    {

        private static ResourceSelectorWindowBuilder s_Instance;

        [SerializeField]
        private GameObject m_SelectorWindowPrefab;

        [SerializeField]
        private Transform m_SelectorWindowParent;

        private void Awake()
        {
            s_Instance = this;
        }


        public static ResourceSelectorWindow CreateWindow < T >() => CreateWindow( typeof( T ) );
        public static ResourceSelectorWindow CreateWindow(params Type[] types)
        {
            GameObject obj = Instantiate( s_Instance.m_SelectorWindowPrefab, s_Instance.m_SelectorWindowParent );
            ResourceSelectorWindow window = obj.GetComponent < ResourceSelectorWindow >();
            window.SetTypeFilter( types );

            return window;
        }

    }

}

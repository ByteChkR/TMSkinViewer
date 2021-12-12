using UnityEngine;

namespace UI.LoadingWindow
{

    public class LoadingWindowBuilder : MonoBehaviour
    {

        private static LoadingWindowBuilder s_Instance;

        [SerializeField]
        private GameObject m_LoadingWindowPrefab;
        
        [SerializeField]
        private Transform m_LoadingWindowParent;
        
        private void Awake()
        {
            s_Instance = this;
        }

        public static LoadingWindow CreateWindow()
        {
            GameObject window=Instantiate(s_Instance.m_LoadingWindowPrefab, s_Instance.m_LoadingWindowParent);

            return window.GetComponent < LoadingWindow >();
        }

    }

}
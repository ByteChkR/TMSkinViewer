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
        [SerializeField]
        private GameObject m_BlockingPanel;

        private void Awake()
        {
            s_Instance = this;
        }

        public static LoadingWindow CreateWindow()
        {
            s_Instance.m_BlockingPanel.SetActive(true);
            GameObject obj = Instantiate( s_Instance.m_LoadingWindowPrefab, s_Instance.m_LoadingWindowParent );
            LoadingWindow window = obj.GetComponent < LoadingWindow >();
            window.OnComplete += () => s_Instance.m_BlockingPanel.SetActive(false);
            return window;
        }

    }

}

using UnityEngine;

namespace UI
{

    public class Desktop : MonoBehaviour
    {

        [SerializeField]
        private Transform m_WindowContainer;

        [SerializeField]
        private DesktopApp m_ExitApp;
        [SerializeField]
        private DesktopApp[] m_Apps;

        [SerializeField]
        private GameObject m_AppButtonPrefab;

        private void Start()
        {
            foreach ( DesktopApp app in m_Apps )
            {
                DesktopItem item = Instantiate( m_AppButtonPrefab, transform ).GetComponent < DesktopItem >();
                item.Icon.sprite = app.Icon;
                item.Text.text = app.Name;
                item.Button.onClick.AddListener( app.OnClick.Invoke );
            }
            
#if !UNITY_WEBGL
            DesktopItem exitItem = Instantiate( m_AppButtonPrefab, transform ).GetComponent < DesktopItem >();
            exitItem.Icon.sprite = m_ExitApp.Icon;
            exitItem.Text.text = m_ExitApp.Name;
            exitItem.Button.onClick.AddListener( m_ExitApp.OnClick.Invoke );
#endif
            
            
        }

        public void Exit()
        {
            Application.Quit();
        }

        public void OpenWindow( GameObject obj )
        {
            Instantiate( obj, m_WindowContainer );
        }

    }

}

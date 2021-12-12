using UnityEngine;

namespace UI
{

    public class Desktop : MonoBehaviour
    {

        [SerializeField]
        private Transform m_WindowContainer;

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

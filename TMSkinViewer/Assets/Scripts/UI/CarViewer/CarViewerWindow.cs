using UnityEngine;
using UnityEngine.UI;

namespace UI.CarViewer
{

    public class CarViewerWindow : MonoBehaviour
    {

        [SerializeField]
        private Window m_Window;
        private Camera m_Camera;
        [SerializeField]
        private GameObject m_ViewerPrefab;
        private GameObject m_ViewerInstance;
        [SerializeField]
        private RectTransform m_RenderTextureContainer;
        [SerializeField]
        private RawImage m_RenderTextureImage;

        private void Awake()
        {
            m_ViewerInstance = Instantiate( m_ViewerPrefab );
            m_Camera = m_ViewerInstance.GetComponentInChildren<Camera>();
            m_Window.OnResized+= OnWindowResized;
            m_Window.OnClose += OnWindowClosed;
            OnWindowResized();
        }

        private void OnWindowClosed()
        {
            Destroy(m_ViewerInstance);
        }

        private void OnWindowResized()
        {
            Rect r = m_RenderTextureContainer.rect;
            if(m_Camera.targetTexture!=null)
                       m_Camera.targetTexture.Release();
            m_Camera.targetTexture = new RenderTexture((int)r.width, (int)r.height, 24);
            m_RenderTextureImage.texture = m_Camera.targetTexture;
        }

    }

}

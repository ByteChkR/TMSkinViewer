using UI.ResourceSelector;

using UnityEngine;
using UnityEngine.UI;

namespace UI.MaterialEditor
{

    public class MaterialEditorWindow : MonoBehaviour
    {

        [SerializeField]
        private Window m_Window;

        [SerializeField]
        private GameObject m_ViewerPrefab;

        [SerializeField]
        private RectTransform m_RenderTextureContainer;

        [SerializeField]
        private RawImage m_RenderTextureImage;

        [SerializeField]
        private CarMaterial m_Material;

        [SerializeField]
        private Material m_ViewerMaterial;

        [SerializeField]
        private Image m_AlbedoImage;

        [SerializeField]
        private Image m_NormalImage;

        [SerializeField]
        private Image m_AmbientOcclusionImage;

        [SerializeField]
        private Image m_EmissionImage;

        [SerializeField]
        private Image m_DirtMaskImage;

        [SerializeField]
        private Image m_RoughnessImage;

        [SerializeField]
        private Button m_AlbedoButton;

        [SerializeField]
        private Button m_NormalButton;

        [SerializeField]
        private Button m_AmbientOcclusionButton;

        [SerializeField]
        private Button m_EmissionButton;

        [SerializeField]
        private Button m_DirtMaskButton;

        [SerializeField]
        private Button m_RoughnessButton;

        private Camera m_Camera;
        private GameObject m_ViewerInstance;

        private void Awake()
        {
            m_AlbedoButton.onClick.AddListener(
                                               () =>
                                               {
                                                   ResourceSelectorWindow window =
                                                       ResourceSelectorWindowBuilder.CreateWindow();

                                                   window.OnResourceSelected += o =>
                                                   {
                                                       m_Material.Albedo = ( Texture2D )o;
                                                       UpdateButtons();
                                                   };
                                               }
                                              );

            m_NormalButton.onClick.AddListener(
                                               () =>
                                               {
                                                   ResourceSelectorWindow window =
                                                       ResourceSelectorWindowBuilder.CreateWindow();

                                                   window.OnResourceSelected += o =>
                                                   {
                                                       m_Material.Normal = ( Texture2D )o;
                                                       UpdateButtons();
                                                   };
                                               }
                                              );

            m_AmbientOcclusionButton.onClick.AddListener(
                                                         () =>
                                                         {
                                                             ResourceSelectorWindow window =
                                                                 ResourceSelectorWindowBuilder.CreateWindow();

                                                             window.OnResourceSelected += o =>
                                                             {
                                                                 m_Material.AmbientOcclusion = ( Texture2D )o;
                                                                 UpdateButtons();
                                                             };
                                                         }
                                                        );

            m_EmissionButton.onClick.AddListener(
                                                 () =>
                                                 {
                                                     ResourceSelectorWindow window =
                                                         ResourceSelectorWindowBuilder.CreateWindow();

                                                     window.OnResourceSelected += o =>
                                                     {
                                                         m_Material.Emissive = ( Texture2D )o;
                                                         UpdateButtons();
                                                     };
                                                 }
                                                );

            m_DirtMaskButton.onClick.AddListener(
                                                 () =>
                                                 {
                                                     ResourceSelectorWindow window =
                                                         ResourceSelectorWindowBuilder.CreateWindow();

                                                     window.OnResourceSelected += o =>
                                                     {
                                                         m_Material.DirtMask = ( Texture2D )o;
                                                         UpdateButtons();
                                                     };
                                                 }
                                                );

            m_RoughnessButton.onClick.AddListener(
                                                  () =>
                                                  {
                                                      ResourceSelectorWindow window =
                                                          ResourceSelectorWindowBuilder.CreateWindow();

                                                      window.OnResourceSelected += o =>
                                                      {
                                                          m_Material.Roughness = ( Texture2D )o;
                                                          UpdateButtons();
                                                      };
                                                  }
                                                 );

            m_ViewerInstance = Instantiate( m_ViewerPrefab, PrefabSpawnHelper.GetSpawn(), Quaternion.identity );
            m_ViewerMaterial = Instantiate( m_ViewerMaterial );

            foreach ( Renderer renderer in m_ViewerInstance.GetComponentsInChildren < Renderer >() )
            {
                renderer.material = m_ViewerMaterial;
            }

            m_Camera = m_ViewerInstance.GetComponentInChildren < Camera >();
            m_Window.OnResized += OnWindowResized;
            m_Window.OnClose += OnWindowClosed;
            OnWindowResized();

            if ( m_Material != null )
            {
                UpdateButtons();
            }
        }

        private void OnWindowClosed()
        {
            Destroy( m_ViewerInstance );
        }

        private void OnWindowResized()
        {
            Rect r = m_RenderTextureContainer.rect;

            if ( m_Camera.targetTexture != null )
            {
                m_Camera.targetTexture.Release();
            }

            m_Camera.targetTexture = new RenderTexture( ( int )r.width, ( int )r.height, 24 );
            m_RenderTextureImage.texture = m_Camera.targetTexture;
        }

        private void SetButtonImage( Image image, Texture2D texture )
        {
            if ( texture == null )
            {
                image.color = Color.clear;

                return;
            }

            image.color = Color.white;

            image.sprite = Sprite.Create(
                                         texture,
                                         new Rect( 0, 0, texture.width, texture.height ),
                                         new Vector2( 0.5f, 0.5f )
                                        );
        }

        private void UpdateButtons()
        {
            SetButtonImage( m_AlbedoImage, m_Material.Albedo );
            SetButtonImage( m_NormalImage, m_Material.Normal );
            SetButtonImage( m_AmbientOcclusionImage, m_Material.AmbientOcclusion );
            SetButtonImage( m_EmissionImage, m_Material.Emissive );
            SetButtonImage( m_DirtMaskImage, m_Material.DirtMask );
            SetButtonImage( m_RoughnessImage, m_Material.Roughness );

            m_ViewerMaterial.SetTexture( "_MainTex", m_Material.Albedo );
            m_ViewerMaterial.SetTexture( "_AO", m_Material.AmbientOcclusion );
            m_ViewerMaterial.SetTexture( "_Normals", m_Material.Normal );
            m_ViewerMaterial.SetTexture( "_Emission", m_Material.Emissive );
            m_ViewerMaterial.SetTexture( "_DirtMask", m_Material.DirtMask );
            m_ViewerMaterial.SetTexture( "_Roughness", m_Material.Roughness );
        }

        public void SetMaterial( CarMaterial material )
        {
            m_Material = material;
            UpdateButtons();
        }

    }

}

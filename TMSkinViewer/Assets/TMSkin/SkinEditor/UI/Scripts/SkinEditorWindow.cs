using System.Linq;

using UnityEngine;
using UnityEngine.UI;

namespace UI.SkinEditorMainWindow
{

    public class SkinEditorWindow : MonoBehaviour
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
        private Dropdown m_SkinMaterialDropdown;

        [SerializeField]
        private Dropdown m_DetailsMaterialDropdown;

        [SerializeField]
        private Dropdown m_GlassMaterialDropdown;

        [SerializeField]
        private Dropdown m_WheelMaterialDropdown;

        private Camera m_Camera;

        private CarSkin m_Skin;
        private SkinViewer m_ViewerInstance;

        public SkinViewer ViewerInstance => m_ViewerInstance;

        private void Awake()
        {
            m_ViewerInstance = Instantiate( m_ViewerPrefab, PrefabSpawnHelper.GetSpawn(), Quaternion.identity ).
                GetComponent < SkinViewer >();

            m_Camera = ViewerInstance.GetComponentInChildren < Camera >();
            m_Window.OnResized += OnWindowResized;
            m_Window.OnClose += OnWindowClosed;
            OnWindowResized();

            m_DetailsMaterialDropdown.onValueChanged.AddListener(
                                                                 idx =>
                                                                 {
                                                                     CarMaterial material =
                                                                         MaterialDatabase.Materials.First(
                                                                              x => x.MaterialName ==
                                                                                  m_DetailsMaterialDropdown.
                                                                                      options[idx].
                                                                                      text
                                                                             );

                                                                     m_Skin.Details = material;
                                                                     ViewerInstance.SetSkin( m_Skin );
                                                                 }
                                                                );

            m_GlassMaterialDropdown.onValueChanged.AddListener(
                                                               idx =>
                                                               {
                                                                   CarMaterial material =
                                                                       MaterialDatabase.Materials.First(
                                                                            x => x.MaterialName ==
                                                                                m_GlassMaterialDropdown.
                                                                                    options[idx].
                                                                                    text
                                                                           );

                                                                   m_Skin.Glass = material;
                                                                   ViewerInstance.SetSkin( m_Skin );
                                                               }
                                                              );

            m_SkinMaterialDropdown.onValueChanged.AddListener(
                                                              idx =>
                                                              {
                                                                  CarMaterial material =
                                                                      MaterialDatabase.Materials.First(
                                                                           x => x.MaterialName ==
                                                                               m_SkinMaterialDropdown.
                                                                                   options[idx].
                                                                                   text
                                                                          );

                                                                  m_Skin.Skin = material;
                                                                  ViewerInstance.SetSkin( m_Skin );
                                                              }
                                                             );

            m_WheelMaterialDropdown.onValueChanged.AddListener(
                                                               idx =>
                                                               {
                                                                   CarMaterial material =
                                                                       MaterialDatabase.Materials.First(
                                                                            x => x.MaterialName ==
                                                                                m_WheelMaterialDropdown.
                                                                                    options[idx].
                                                                                    text
                                                                           );

                                                                   m_Skin.Wheel = material;
                                                                   ViewerInstance.SetSkin( m_Skin );
                                                               }
                                                              );

            if ( m_Skin != null )
            {
                BuildMaterialDropdowns();
                SetDropdownValues();
            }
        }

        private void BuildMaterialDropdowns()
        {
            m_DetailsMaterialDropdown.options.Clear();

            m_DetailsMaterialDropdown.options.AddRange(
                                                       MaterialDatabase.Materials.Select(
                                                            x => new Dropdown.OptionData( x.MaterialName )
                                                           )
                                                      );

            m_GlassMaterialDropdown.options.Clear();

            m_GlassMaterialDropdown.options.AddRange(
                                                     MaterialDatabase.Materials.Select(
                                                          x => new Dropdown.OptionData( x.MaterialName )
                                                         )
                                                    );

            m_WheelMaterialDropdown.options.Clear();

            m_WheelMaterialDropdown.options.AddRange(
                                                     MaterialDatabase.Materials.Select(
                                                          x => new Dropdown.OptionData( x.MaterialName )
                                                         )
                                                    );

            m_SkinMaterialDropdown.options.Clear();

            m_SkinMaterialDropdown.options.AddRange(
                                                    MaterialDatabase.Materials.Select(
                                                         x => new Dropdown.OptionData( x.MaterialName )
                                                        )
                                                   );
        }

        private void OnWindowClosed()
        {
            Destroy( ViewerInstance.gameObject );
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

        public void SetSkin( CarSkin skin )
        {
            m_Skin = skin;
            ViewerInstance.SetSkin( skin );
            BuildMaterialDropdowns();
            SetDropdownValues();
        }

        private void SetDropdownValues()
        {
            m_DetailsMaterialDropdown.value =
                m_DetailsMaterialDropdown.options.FindIndex(
                                                            x => x.text == m_Skin.Details.MaterialName
                                                           );

            m_GlassMaterialDropdown.value =
                m_GlassMaterialDropdown.options.FindIndex(
                                                          x => x.text == m_Skin.Glass.MaterialName
                                                         );

            m_WheelMaterialDropdown.value =
                m_WheelMaterialDropdown.options.FindIndex(
                                                          x => x.text == m_Skin.Wheel.MaterialName
                                                         );

            m_SkinMaterialDropdown.value =
                m_SkinMaterialDropdown.options.FindIndex(
                                                         x => x.text == m_Skin.Skin.MaterialName
                                                        );
        }

    }

}

using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

using SFB;

using UnityEngine;

namespace UI.SkinEditorMainWindow
{

    public class SkinEditorMainWindow : MonoBehaviour
    {

        [SerializeField]
        private bool m_ViewOnly = false;

        [SerializeField]
        private Window m_Window;

        [SerializeField]
        private Sprite m_AddSkinSprite;

        [SerializeField]
        private Sprite m_ImportSkinSprite;

        [SerializeField]
        private Sprite m_SkinSprite;

        [SerializeField]
        private Transform m_SkinButtonContainer;

        [SerializeField]
        private GameObject m_SkinButtonPrefab;

        [SerializeField]
        private GameObject m_AddSkinDialogPrefab;

        [SerializeField]
        private GameObject m_ImportSkinDialogPrefab;

        [SerializeField]
        private GameObject m_EditSkinDialogPrefab;

        private readonly List < GameObject > m_SkinButtons = new List < GameObject >();

        private GameObject m_AddSkinButtonInstance;
        private GameObject m_ImportSkinButtonInstance;

        private void Awake()
        {
            SkinDatabase.OnSkinDatabaseChanged += OnSkinDatabaseChanged;

            if ( !m_ViewOnly )
            {
                m_AddSkinButtonInstance = Instantiate( m_SkinButtonPrefab, m_SkinButtonContainer );
                SkinEditorMainWindowItem addItem = m_AddSkinButtonInstance.GetComponent < SkinEditorMainWindowItem >();
                addItem.Button.onClick.AddListener( CreateSkin );
                addItem.Icon.sprite = m_AddSkinSprite;
                addItem.Text.text = "New";
                addItem.ExportButton.gameObject.SetActive( false );

                m_ImportSkinButtonInstance = Instantiate( m_SkinButtonPrefab, m_SkinButtonContainer );

                SkinEditorMainWindowItem importItem =
                    m_ImportSkinButtonInstance.GetComponent < SkinEditorMainWindowItem >();

                importItem.Button.onClick.AddListener( ImportSkin );
                importItem.Icon.sprite = m_AddSkinSprite;
                importItem.Text.text = "Import";
                importItem.ExportButton.gameObject.SetActive( false );
            }

            RebuildSkinList();
        }

        private void CreateSkin()
        {
            Instantiate( m_AddSkinDialogPrefab, transform.parent );
        }

        private void ImportSkin()
        {
            Instantiate( m_ImportSkinDialogPrefab, transform.parent );
        }

        private void EditSkin( CarSkin skin )
        {
            //Show Skin Editor
            SkinEditorWindow window = Instantiate( m_EditSkinDialogPrefab, transform.parent ).
                GetComponent < SkinEditorWindow >();

            window.ReadOnly = m_ViewOnly;

            window.SetSkin( skin );
        }

        private void RebuildSkinList()
        {
            foreach ( GameObject button in m_SkinButtons )
            {
                Destroy( button );
            }

            m_SkinButtons.Clear();

            foreach ( CarSkin skin in SkinDatabase.LoadedSkins )
            {
                GameObject button = Instantiate( m_SkinButtonPrefab, m_SkinButtonContainer );
                m_SkinButtons.Add( button );
                SkinEditorMainWindowItem item = button.GetComponent < SkinEditorMainWindowItem >();
                CarSkin s = skin;
                item.Button.onClick.AddListener( () => EditSkin( s ) );
                item.Icon.sprite = m_SkinSprite;
                item.Text.text = skin.SkinName;

                item.ExportButton.onClick.AddListener(
                                                      () => SaveCarSkinDialog( s.SkinName, SkinExporter.Export( s ) )
                                                     );
            }
        }

#if UNITY_WEBGL && !UNITY_EDITOR
    //
    // WebGL
    //
    [DllImport("__Internal")]
    private static extern void DownloadFile(string gameObjectName, string methodName, string filename, byte[] byteArray, int byteArraySize);

    // Broser plugin should be called in OnPointerDown.
    public void SaveCarSkinDialog(string name, byte[] data) {
        DownloadFile(gameObject.name, "SaveCarSkinDialog_Callback", $"{name}.zip", data, data.Length);
    }

    // Called from browser
    public void SaveCarSkinDialog_Callback() {
        Debug.Log("File Successfully Downloaded");
    }
#else

        public void SaveCarSkinDialog( string name, byte[] data )
        {
            string path = StandaloneFileBrowser.SaveFilePanel( "Title", "", name, "zip" );

            if ( !string.IsNullOrEmpty( path ) )
            {
                File.WriteAllBytes( path, data );
                Debug.Log( "File Successfully Downloaded" );
            }
        }
#endif

        private void OnSkinDatabaseChanged()
        {
            RebuildSkinList();
        }

    }

}

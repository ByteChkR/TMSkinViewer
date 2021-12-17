using System.Collections.Generic;

using UnityEngine;

namespace UI.SkinEditorMainWindow
{

    public class SkinEditorMainWindow : MonoBehaviour
    {

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
            m_AddSkinButtonInstance = Instantiate( m_SkinButtonPrefab, m_SkinButtonContainer );
            SkinEditorMainWindowItem addItem = m_AddSkinButtonInstance.GetComponent < SkinEditorMainWindowItem >();
            addItem.Button.onClick.AddListener( CreateSkin );
            addItem.Icon.sprite = m_AddSkinSprite;
            addItem.Text.text = "New";

            m_ImportSkinButtonInstance = Instantiate( m_SkinButtonPrefab, m_SkinButtonContainer );

            SkinEditorMainWindowItem importItem =
                m_ImportSkinButtonInstance.GetComponent < SkinEditorMainWindowItem >();

            importItem.Button.onClick.AddListener( ImportSkin );
            importItem.Icon.sprite = m_AddSkinSprite;
            importItem.Text.text = "Import";

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
            }
        }

        private void OnSkinDatabaseChanged()
        {
            RebuildSkinList();
        }

    }

}

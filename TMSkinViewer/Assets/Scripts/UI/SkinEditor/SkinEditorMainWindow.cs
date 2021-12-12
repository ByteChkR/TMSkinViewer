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
        private Sprite m_SkinSprite;
        
        [SerializeField]
        private Transform m_SkinButtonContainer;
        [SerializeField]
        private GameObject m_SkinButtonPrefab;

        private GameObject m_AddSkinButtonInstance;
        [SerializeField]
        private GameObject m_AddSkinDialogPrefab;
        [SerializeField]
        private GameObject m_EditSkinDialogPrefab;

        private readonly List < GameObject > m_SkinButtons = new List < GameObject >();

        private void Awake()
        {
            SkinDatabase.OnSkinDatabaseChanged+= OnSkinDatabaseChanged;
            m_AddSkinButtonInstance = Instantiate( m_SkinButtonPrefab, m_SkinButtonContainer );
            SkinEditorMainWindowItem item = m_AddSkinButtonInstance.GetComponent < SkinEditorMainWindowItem >();
            item.Button.onClick.AddListener( CreateSkin);
            item.Icon.sprite = m_AddSkinSprite;
            item.Text.text = "New";
            
            RebuildSkinList();
        }

        private void CreateSkin()
        {
            Instantiate( m_AddSkinDialogPrefab, transform.parent );
        }
        
        private void EditSkin( CarSkin skin )
        {
            //Show Skin Editor
            SkinEditorWindow window= Instantiate( m_EditSkinDialogPrefab, transform.parent ).GetComponent<SkinEditorWindow>();
            window.SetSkin(skin);
        }

        private void RebuildSkinList()
        {
            foreach ( GameObject button in m_SkinButtons )
            {
                Destroy( button );
            }
            m_SkinButtons.Clear();

            foreach ( CarSkin skin in SkinDatabase.Skins )
            {
                GameObject button = Instantiate( m_SkinButtonPrefab, m_SkinButtonContainer );
                m_SkinButtons.Add( button );
                SkinEditorMainWindowItem item = button.GetComponent < SkinEditorMainWindowItem >();
                CarSkin s = skin;
                item.Button.onClick.AddListener( () => EditSkin(s));
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

using System.Collections.Generic;

using UI.SkinEditorMainWindow;

using UnityEngine;

namespace UI.MaterialEditor
{

    public class MaterialEditorMainWindow : MonoBehaviour
    {

        [SerializeField]
        private Window m_Window;

        [SerializeField]
        private Sprite m_AddSkinSprite;
        [SerializeField]
        private Sprite m_SkinSprite;
        
        [SerializeField]
        private Transform m_MaterialButtonContainer;
        [SerializeField]
        private GameObject m_MaterialButtonPrefab;

        private GameObject m_AddMaterialButtonInstance;
        [SerializeField]
        private GameObject m_AddMaterialDialogPrefab;
        [SerializeField]
        private GameObject m_EditMaterialDialogPrefab;

        private readonly List < GameObject > m_SkinButtons = new List < GameObject >();

        private void Awake()
        {
            MaterialDatabase.OnMaterialDatabaseChanged+= OnMaterialDatabaseChanged;
            m_AddMaterialButtonInstance = Instantiate( m_MaterialButtonPrefab, m_MaterialButtonContainer );
            MaterialEditorMainWindowItem item = m_AddMaterialButtonInstance.GetComponent < MaterialEditorMainWindowItem >();
            item.Button.onClick.AddListener( CreateMaterial);
            item.Icon.sprite = m_AddSkinSprite;
            item.Text.text = "New";
            RebuildMaterialList();
        }

        private void CreateMaterial()
        {
            Instantiate( m_AddMaterialDialogPrefab, transform.parent );
            //Show Material Creation Dialog
        }
        
        private void EditMaterial( CarMaterial material )
        {
            //Show Material Editor
            MaterialEditorWindow window= Instantiate( m_EditMaterialDialogPrefab, transform.parent ).GetComponent<MaterialEditorWindow>();
            window.SetMaterial(material);
        }

        private void RebuildMaterialList()
        {
            foreach ( GameObject button in m_SkinButtons )
            {
                Destroy( button );
            }
            m_SkinButtons.Clear();

            foreach ( CarMaterial material in MaterialDatabase.Materials )
            {
                GameObject button = Instantiate( m_MaterialButtonPrefab, m_MaterialButtonContainer );
                m_SkinButtons.Add( button );
                MaterialEditorMainWindowItem item = button.GetComponent < MaterialEditorMainWindowItem >();
                CarMaterial m = material;
                item.Button.onClick.AddListener( () => EditMaterial(m));
                item.Icon.sprite = m_SkinSprite;
                item.Text.text = material.MaterialName;
            }
        }
        
        private void OnMaterialDatabaseChanged()
        {
            RebuildMaterialList();
        }

    }

}
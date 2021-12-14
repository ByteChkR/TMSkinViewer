using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace UI.Settings
{

    public class SettingsWindow : MonoBehaviour
    {


        [SerializeField]
        private Window m_Window;

        [SerializeField]
        private Transform m_CategoryParent;
        
        [SerializeField]
        private GameObject m_CategoryItem;

        [SerializeField]
        private SettingsInspector m_Inspector;

        private void CreateCategoryList()
        {
            foreach ( Transform item in m_CategoryParent )
            {
                Destroy(item.gameObject);
            }
            
            foreach ( SettingsCategory category in SettingsManager.AllCategories )
            {
                GameObject i = Instantiate(m_CategoryItem, m_CategoryParent);
                Button btn = i.GetComponent < Button >();
                Text txt = i.GetComponentInChildren < Text >();
                txt.text = category.Path;
                SettingsCategory c = category;
                btn.onClick.AddListener(() => m_Inspector.SetCategory(c));
            }
        }

        private void Start()
        {
            
            CreateCategoryList();
            
            
        }

    }

}

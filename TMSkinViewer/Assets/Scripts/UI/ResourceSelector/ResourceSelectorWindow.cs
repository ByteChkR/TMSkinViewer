using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace UI.ResourceSelector
{

    public class ResourceSelectorWindow : MonoBehaviour
    {

        [SerializeField]
        private Window m_Window;

        [SerializeField]
        private Transform m_Content;

        [SerializeField]
        private GameObject m_ResourceItemPrefab;

        private readonly List < GameObject > m_ActiveItems = new List < GameObject >();

        private void Awake()
        {
            SetActiveItems( ResourceSystem.GetResourceOrigins().Select( x => x.GetRootNode() ), null, false );
        }

        public event System.Action < Object > OnResourceSelected;

        private void ResourceSelected( ResourceNode node )
        {
            OnResourceSelected?.Invoke( node.GetResource() );
            m_Window.Close();
        }

        private void SetActiveItems( IEnumerable < ResourceNode > items, ResourceNode parent, bool addUpwards = true )
        {
            foreach ( GameObject item in m_ActiveItems )
            {
                Destroy( item );
            }

            m_ActiveItems.Clear();

            if ( parent == null && addUpwards )
            {
                ResourceSelectorWindowItem windowItem = Instantiate( m_ResourceItemPrefab, m_Content ).
                    GetComponent < ResourceSelectorWindowItem >();

                windowItem.Icon.sprite = ResourceSystem.GetDefaultIcon( ResourceType.Directory );
                windowItem.Text.text = "..";

                windowItem.Button.onClick.AddListener(
                                                      () => SetActiveItems(
                                                                           ResourceSystem.GetResourceOrigins().
                                                                               Select( x => x.GetRootNode() ),
                                                                           null
                                                                          )
                                                     );

                m_ActiveItems.Add( windowItem.gameObject );
            }
            else if ( parent != null )
            {
                ResourceSelectorWindowItem windowItem = Instantiate( m_ResourceItemPrefab, m_Content ).
                    GetComponent < ResourceSelectorWindowItem >();

                windowItem.Icon.sprite = ResourceSystem.GetDefaultIcon( ResourceType.Directory );
                windowItem.Text.text = "..";

                if(parent.Parent!=null)
                windowItem.Button.onClick.AddListener(
                                                      () => SetActiveItems(
                                                                           parent.Parent.Children,
                                                                           parent.Parent
                                                                          )
                                                     );
                else windowItem.Button.onClick.AddListener(
                                                          () => SetActiveItems(
                                                                               ResourceSystem.GetResourceOrigins().
                                                                                   Select( x => x.GetRootNode() ),
                                                                               null,
                                                                               false
                                                                              )
                                                         );

                m_ActiveItems.Add( windowItem.gameObject );
            }

            foreach ( ResourceNode item in items )
            {
                GameObject newItem = Instantiate( m_ResourceItemPrefab, m_Content );
                ResourceSelectorWindowItem windowItem = newItem.GetComponent < ResourceSelectorWindowItem >();

                windowItem.Text.text = item.Name;
                windowItem.Icon.sprite = item.GetIcon();

                if ( item.Type == ResourceType.Directory )
                {
                    windowItem.Button.onClick.AddListener( () => SetActiveItems( item.Children, item) );
                }
                else
                {
                    windowItem.Button.onClick.AddListener( () => ResourceSelected( item ) );
                }

                m_ActiveItems.Add( newItem );
            }
        }

    }

}

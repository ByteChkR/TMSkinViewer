using UI.ResourceSelector;

using UnityEngine;
using UnityEngine.UI;

namespace UI.Settings
{

    public abstract class SettingsResourceValueInspector < T > : SettingsResourceValueInspector
        where T: Object
    {

        protected abstract void OnResourceSelected( T resource, ResourceNode node );
        protected override void OnResourceSelected( ResourceNode resource ) => OnResourceSelected( ( T )resource.GetResource(), resource );

    }
    public abstract class SettingsResourceValueInspector : SettingsValueInspector
    {

        [SerializeField]
        private Button m_Button;
        
        protected SettingsPropertyWrapper m_Property;

        protected abstract void OnResourceSelected( ResourceNode resource );
        
        private void InnerResourceSelected(ResourceNode resource)
        {
            m_Property.Value = resource.GetResource();
            OnResourceSelected(resource);
        }
        
        public override void SetProperty( SettingsPropertyWrapper prop )
        {
            base.SetProperty( prop );
            m_Property = prop;
            m_Button.onClick.AddListener(
                                         () =>
                                         {
                                             ResourceSelectorWindow window = ResourceSelectorWindowBuilder.CreateWindow(prop.Type);
                                             window.OnResourceSelected += InnerResourceSelected;
                                         }
                                        );
        }

    }

}

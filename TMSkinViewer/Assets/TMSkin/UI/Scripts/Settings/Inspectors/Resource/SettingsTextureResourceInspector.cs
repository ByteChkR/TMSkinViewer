using UnityEngine;
using UnityEngine.UI;

namespace UI.Settings
{

    public class SettingsTextureResourceInspector : SettingsResourceValueInspector < Texture2D >
    {

        [SerializeField]
        private Image m_TexturePreview;

        protected override void OnResourceSelected( Texture2D resource, ResourceNode node )
        {
            m_TexturePreview.sprite = node.GetIcon();
        }

        public override void SetProperty( SettingsPropertyWrapper prop )
        {
            base.SetProperty( prop );

            if ( prop.Value == null )
            {
                return;
            }

            Texture2D val = ( Texture2D )prop.Value;

            m_TexturePreview.sprite = Sprite.Create(
                                                    val,
                                                    new Rect( 0, 0, val.width, val.height ),
                                                    new Vector2( 0.5f, 0.5f )
                                                   );
        }

    }

}

using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{

    public class WindowBottomHelper : MonoBehaviour, IDragHandler, IEndDragHandler
    {

        [SerializeField]
        private Window m_Window;

        public void OnDrag( PointerEventData eventData )
        {
            UpdateDrag( eventData );
        }

        public void OnEndDrag( PointerEventData eventData )
        {
            m_Window.TriggerOnResized();
        }

        private void UpdateDrag( PointerEventData eventData )
        {
            m_Window.Position = new Vector2( m_Window.Position.x, m_Window.Position.y + eventData.delta.y );
            m_Window.Size = m_Window.Size - new Vector2( -eventData.delta.x, eventData.delta.y );
        }

    }

}

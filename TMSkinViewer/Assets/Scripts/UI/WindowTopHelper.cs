using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{

    public class WindowTopHelper : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {

        [SerializeField]
        private Window m_Window;

        private Vector2 m_CursorOffset;

        public void OnBeginDrag( PointerEventData eventData )
        {
            m_CursorOffset = eventData.pressPosition - m_Window.Position;
        }

        public void OnDrag( PointerEventData eventData )
        {
            m_Window.Position = eventData.position - m_CursorOffset;
        }

        public void OnEndDrag( PointerEventData eventData )
        {
            m_Window.Position = eventData.position - m_CursorOffset;
            m_CursorOffset = Vector2.zero;
        }

    }

}

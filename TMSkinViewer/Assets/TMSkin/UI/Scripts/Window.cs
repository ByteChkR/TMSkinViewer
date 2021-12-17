using System;

using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{

    public class Window : MonoBehaviour, IPointerClickHandler
    {
        public static Transform DefaultHost { get; set; }

        [SerializeField]
        private Vector2 m_MinSize;

        [SerializeField]
        private bool m_HasMaxSize = false;

        [SerializeField]
        private Vector2 m_MaxSize;

        private RectTransform m_RectTransform;

        public bool HasFocus() => m_RectTransform.GetSiblingIndex() == m_RectTransform.parent.childCount - 1;
        public Vector2 Position
        {
            get => m_RectTransform.anchoredPosition;
            set => m_RectTransform.anchoredPosition = value;
        }

        public Vector2 Size
        {
            get => m_RectTransform.sizeDelta;
            set
            {
                if ( value.x < m_MinSize.x )
                {
                    value.x = m_MinSize.x;
                }

                if ( value.y < m_MinSize.y )
                {
                    value.y = m_MinSize.y;
                }

                if ( m_HasMaxSize )
                {
                    if ( value.x > m_MaxSize.x )
                    {
                        value.x = m_MaxSize.x;
                    }

                    if ( value.y > m_MaxSize.y )
                    {
                        value.y = m_MaxSize.y;
                    }
                }

                m_RectTransform.sizeDelta = value;
            }
        }

        private void Awake()
        {
            m_RectTransform = ( RectTransform )transform;
        }

        public void OnPointerClick( PointerEventData eventData )
        {
            transform.SetSiblingIndex( transform.parent.childCount - 1 );
        }

        public event Action OnResized;

        public event Action OnClose;

        public void TriggerOnResized()
        {
            OnResized?.Invoke();
        }

        public void Maximize()
        {
            Position = Vector2.zero;
            Size = new Vector2( Screen.width, Screen.height );
            TriggerOnResized();
        }

        public void Close()
        {
            OnClose?.Invoke();
            Destroy( gameObject );
        }

    }

}

using UnityEngine;
using UnityEngine.UI;

namespace UI.Settings
{

    public class SettingsVectorInspector : SettingsValueInspector
    {

        [SerializeField]
        private InputField m_XInputField;

        [SerializeField]
        private InputField m_YInputField;

        [SerializeField]
        private InputField m_ZInputField;

        [SerializeField]
        private InputField m_WInputField;

        [SerializeField]
        private GameObject m_ZContainer;

        [SerializeField]
        private GameObject m_WContainer;

        private SettingsPropertyWrapper m_Wrapper;

        public override void SetProperty( SettingsPropertyWrapper prop )
        {
            base.SetProperty( prop );

            m_Wrapper = prop;

            m_XInputField.interactable = prop.CanWrite;
            m_YInputField.interactable = prop.CanWrite;
            m_ZInputField.interactable = prop.CanWrite;
            m_WInputField.interactable = prop.CanWrite;

            if ( prop.Type == typeof( Vector2 ) )
            {
                m_ZContainer.SetActive( false );
                m_WContainer.SetActive( false );
                Vector2 v = ( Vector2 )prop.Value;
                m_XInputField.SetTextWithoutNotify( v.x.ToString() );
                m_YInputField.SetTextWithoutNotify( v.y.ToString() );
            }
            else if ( prop.Type == typeof( Vector3 ) )
            {
                m_WContainer.SetActive( false );
                Vector3 v = ( Vector3 )prop.Value;
                m_XInputField.SetTextWithoutNotify( v.x.ToString() );
                m_YInputField.SetTextWithoutNotify( v.y.ToString() );
                m_ZInputField.SetTextWithoutNotify( v.z.ToString() );
            }
            else if ( prop.Type == typeof( Vector4 ) )
            {
                Vector4 v = ( Vector4 )prop.Value;
                m_XInputField.SetTextWithoutNotify( v.x.ToString() );
                m_YInputField.SetTextWithoutNotify( v.y.ToString() );
                m_ZInputField.SetTextWithoutNotify( v.z.ToString() );
                m_WInputField.SetTextWithoutNotify( v.w.ToString() );
            }
            else
            {
                Debug.LogError( "Unsupported type: " + prop.Type );
            }
        }

        public void OnEndEdit()
        {
            if ( m_Wrapper.Type == typeof( Vector2 ) )
            {
                Vector2 v = new Vector2( float.Parse( m_XInputField.text ), float.Parse( m_YInputField.text ) );
                m_Wrapper.Value = v;
            }
            else if ( m_Wrapper.Type == typeof( Vector3 ) )
            {
                Vector3 v = new Vector3(
                                        float.Parse( m_XInputField.text ),
                                        float.Parse( m_YInputField.text ),
                                        float.Parse( m_ZInputField.text )
                                       );

                m_Wrapper.Value = v;
            }
            else if ( m_Wrapper.Type == typeof( Vector4 ) )
            {
                Vector4 v = new Vector4(
                                        float.Parse( m_XInputField.text ),
                                        float.Parse( m_YInputField.text ),
                                        float.Parse( m_ZInputField.text ),
                                        float.Parse( m_WInputField.text )
                                       );

                m_Wrapper.Value = v;
            }
            else
            {
                Debug.LogError( "Unsupported type" );
            }
        }

    }

}

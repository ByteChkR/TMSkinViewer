using System.Collections;

using UI;

using UnityEngine;
using UnityEngine.UI;

public class ConsoleWindow : MonoBehaviour
{

    [SerializeField]
    private Window m_Window;

    [SerializeField]
    private int m_MaxCharacters;

    [SerializeField]
    private Text m_Text;

    [SerializeField]
    private ScrollRect m_View;

    private void Start()
    {
        m_Window = GetComponent < Window >();
        Application.logMessageReceived += MessageReceived;
        StartCoroutine( ScrollRoutine() );

        m_Window.OnClose += () => { Application.logMessageReceived -= MessageReceived; };
    }

    private void MessageReceived( string condition, string stacktrace, LogType type )
    {
        m_Text.text += $"[{type}] {condition}\n";

        if ( m_Text.text.Length > m_MaxCharacters )
        {
            m_Text.text = m_Text.text.Remove( 0, m_MaxCharacters - m_Text.text.Length );
        }
    }

    private IEnumerator ScrollRoutine()
    {
        while ( true )
        {
            m_View.ScrollToBottom();

            yield return new WaitForEndOfFrame();
        }
    }

}

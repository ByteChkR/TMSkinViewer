using UnityEngine;
using UnityEngine.UI;

namespace UI.Settings
{

    public class PopOutWindowBuilder: MonoBehaviour
    {

        [SerializeField]
        private GameObject m_WindowPrefab;
        [SerializeField]
        private GameObject m_ButtonPrefab;

        private void Awake()
        {
            SettingsInspector.AddFallbackInspector(m_ButtonPrefab, OnCreatePopOutInspector);
        }
        
        
        private void OnCreatePopOutInspector( GameObject inspector, object instance )
        {
            ButtonInspector btn = inspector.GetComponentInChildren< ButtonInspector >();

            string title = instance.GetType().Name;
            string name = $"Edit {title}";
            btn.ButtonText.text = name;
            SettingsCategory c = new SettingsCategory( name , null);
            c.AddSettingsObject( instance );
            btn.Button.onClick.AddListener(
                                    () =>
                                    {
                                        PopOutEditorWindow insp = Instantiate(m_WindowPrefab, Window.DefaultHost).GetComponent<PopOutEditorWindow>();
                                        insp.SetTarget( c );
                                        
                                        
                                    });
        }

    }

}

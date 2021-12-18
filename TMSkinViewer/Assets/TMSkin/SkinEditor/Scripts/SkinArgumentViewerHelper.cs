using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UI;
using UI.SkinEditorMainWindow;

using UnityEngine;

public class SkinArgumentViewerHelper : MonoBehaviour
{

    [SerializeField]
    private GameObject m_EditorWindowPrefab;

    private void Awake()
    {
        
        GetComponent<SkinDatabase>().OnSkinDatabaseLoaded+= OnSkinDatabaseLoaded;
        
    }

    private void OnSkinDatabaseLoaded()
    {
        bool isReadOnly = AppStartArgs.Args.ContainsKey("edit_mode") && AppStartArgs.Args["edit_mode"] == "readonly";
        if ( AppStartArgs.Args.ContainsKey( "skin_name" ) )
        {
            
            CarSkin skin = SkinDatabase.LoadedSkins.First(x=>x.SkinName == AppStartArgs.Args["skin_name"]);
            if ( skin != null )
            {
                SkinEditorWindow window = Instantiate( m_EditorWindowPrefab, transform ).GetComponent<SkinEditorWindow>();
                Window w = window.GetComponent<Window>();
                window.ReadOnly = isReadOnly;
                w.Maximize();

                w.OnClosing += e =>
                               {
                                   e.Abort();
                               };
                window.SetSkin( skin );
            }
            
        }

    }

}

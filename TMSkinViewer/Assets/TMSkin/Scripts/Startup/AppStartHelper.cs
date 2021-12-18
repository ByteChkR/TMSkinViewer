using System.Collections.Generic;

using UnityEngine;

public class AppStartHelper : MonoBehaviour
{

    [SerializeField]
    private AppStartSettings[] m_Apps;

    [SerializeField]
    private string m_DefaultApp = "full";

    private void StartApp(AppStartSettings app, AppStartArgs args)
    {

        GameObject instance = Instantiate( app.AppPrefab );
        
        app.StartApp(instance, args);

    }
    private void Start()
    {
        AppStartArgs args = AppStartArgs.Args;

        string app;

        if ( args.ContainsKey( "app" ) )
        {
            app = args["app"];
        }
        else
        {
            app = m_DefaultApp;
        }
        
        foreach ( AppStartSettings appStartSettings in m_Apps )
        {
            if ( appStartSettings.AppName == app )
            {
                StartApp(appStartSettings, args);
                return;
            }
        }
    }

}

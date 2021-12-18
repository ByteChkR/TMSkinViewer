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
    private void Awake()
    {
        Dictionary < string, string > args = URLParameters.GetSearchParameters();

        string app;

        if ( args.ContainsKey( "mode" ) )
        {
            app = args["mode"];
        }
        else
        {
            app = m_DefaultApp;
        }
        
        foreach ( AppStartSettings appStartSettings in m_Apps )
        {
            if ( appStartSettings.AppName == app )
            {
                StartApp(appStartSettings, new AppStartArgs(args));
                return;
            }
        }
    }

}

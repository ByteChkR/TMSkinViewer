using System;
using System.Collections.Generic;
using System.IO;

using UI.LoadingWindow;

using UnityEngine;

public class ResourceSystem : MonoBehaviour
{

    private static ResourceSystem s_Instance;

    public static readonly List < ResourceOrigin > DefaultOrigins = new List < ResourceOrigin >();

    private static readonly Dictionary < string, Func < Uri, ResourceOrigin > > m_OriginCreators =
        new Dictionary < string, Func < Uri, ResourceOrigin > >();

    [SerializeField]
    private List < ResourceDefaultIcon > m_DefaultIcons;

    private bool m_IsInitialized = false;
    private List < ResourceOrigin > m_Origins;
    private ResourceSystemSettings m_Settings;

    private void Awake()
    {
        m_Origins = new List < ResourceOrigin >( DefaultOrigins );
        s_Instance = this;
        m_Settings = new ResourceSystemSettings();
        SettingsManager.AddSettingsObject( m_Settings );

        m_OriginCreators.Add(
                             "file",
                             u => new FileSystemResourceOrigin( Path.GetFileName( u.AbsolutePath ), u.AbsolutePath )
                            );

        m_Settings.OnReloadOrigins += () =>
                                      {
                                          for ( int i = m_Origins.Count - 1; i >= 0; i-- )
                                          {
                                              if ( !( m_Origins[i] is InternalResourceOrigin ) )
                                              {
                                                  m_Origins.RemoveAt( i );
                                              }
                                          }

                                          m_IsInitialized = false;

                                          foreach ( Uri origin in m_Settings.Uris )
                                          {
                                              AddOrigin( origin );
                                          }

                                          Initialize();
                                      };

        PrefabInitializeHelper helper =
            GetComponent < PrefabInitializeHelper >();

        helper.OnFinalize += () => { Initialize( helper.Tasks ); };
    }

    public static IEnumerable < ResourceOrigin > GetResourceOrigins()
    {
        return s_Instance.m_Origins;
    }

    public static Sprite GetDefaultIcon( ResourceType type )
    {
        return s_Instance.m_DefaultIcons.Find( x => x.Type == type ).Icon;
    }

    public static void AddOrigin( Uri uri )
    {
        AddOrigin( m_OriginCreators[uri.Scheme]( uri ) );
    }

    public static void AddOrigin( ResourceOrigin origin )
    {
        s_Instance.m_Origins.Add( origin );

        if ( s_Instance.m_IsInitialized )
        {
            TaskCollection taskCollection = new TaskCollection();
            origin.Initialize( taskCollection );
            LoadingWindow window = LoadingWindowBuilder.CreateWindow();
            window.Process( taskCollection );
        }
    }

    private static void Initialize( TaskCollection taskCollection )
    {
        s_Instance.m_Origins.ForEach( x => x.Initialize( taskCollection ) );
    }

    public static void Initialize()
    {
        if ( s_Instance.m_IsInitialized )
        {
            return;
        }

        s_Instance.m_IsInitialized = true;
        TaskCollection taskCollection = new TaskCollection();
        Initialize( taskCollection );
        LoadingWindow window = LoadingWindowBuilder.CreateWindow();
        window.Process( taskCollection );
    }

}

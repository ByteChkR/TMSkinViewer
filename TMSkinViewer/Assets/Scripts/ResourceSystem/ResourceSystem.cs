using System.Collections.Generic;

using UI.LoadingWindow;

using UnityEngine;

public class ResourceSystem : MonoBehaviour
{

    private static ResourceSystem s_Instance;

    [SerializeField]
    private GameObject m_BlockingPanel;

    [SerializeField]
    private List < ResourceDefaultIcon > m_DefaultIcons;

    private readonly List < ResourceOrigin > m_Origins = new List < ResourceOrigin >();
    private bool m_IsInitialized = false;

    private void Awake()
    {
        s_Instance = this;
    }

    public static IEnumerable < ResourceOrigin > GetResourceOrigins()
    {
        return s_Instance.m_Origins;
    }

    public static Sprite GetDefaultIcon( ResourceType type )
    {
        return s_Instance.m_DefaultIcons.Find( x => x.Type == type ).Icon;
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

    public static void Initialize()
    {
        if ( s_Instance.m_IsInitialized )
        {
            return;
        }

        s_Instance.m_IsInitialized = true;
        TaskCollection taskCollection = new TaskCollection();
        s_Instance.m_Origins.ForEach( x => x.Initialize( taskCollection ) );

        LoadingWindow window = LoadingWindowBuilder.CreateWindow();
        window.OnComplete += () => { s_Instance.m_BlockingPanel.SetActive( false ); };
        window.Process( taskCollection );
    }

}

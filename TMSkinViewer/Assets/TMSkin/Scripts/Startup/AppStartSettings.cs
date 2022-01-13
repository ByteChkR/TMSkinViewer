using UnityEngine;

[CreateAssetMenu( fileName = "New App Settings", menuName = "TMSkin/Startup/Settings" )]
public class AppStartSettings : ScriptableObject
{

    [SerializeField]
    protected string m_AppName;

    [SerializeField]
    protected GameObject m_AppPrefab;

    public string AppName => m_AppName;

    public GameObject AppPrefab => m_AppPrefab;

    public virtual void StartApp( GameObject instance, AppStartArgs startArgs )
    {
    }

}

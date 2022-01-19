using System.Collections;

using UnityEngine;

public class FileSystemResourceOriginHelper : MonoBehaviour
{

    [SerializeField]
    private string m_OriginName;
    [SerializeField]
    private string m_ResourcePath;

    [SerializeField]
    private bool m_AutoReload;
    
    [SerializeField]
    private float m_AutoReloadInterval = 1.0f;
    
    private FileSystemResourceOrigin m_Origin;

    private void Awake()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        m_Origin = new FileSystemResourceOrigin( m_OriginName, m_ResourcePath, m_AutoReload );
        ResourceSystem.AddOrigin(m_Origin );
        StartCoroutine( ProcessChangeQueue() );
#endif
    }

    private IEnumerator ProcessChangeQueue()
    {
        while( true )
        {
            yield return new WaitForSeconds( m_AutoReloadInterval );
            m_Origin.ProcessChangeQueue();
        }
    }
    

}

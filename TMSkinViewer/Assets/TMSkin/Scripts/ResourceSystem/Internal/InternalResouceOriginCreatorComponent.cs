using UnityEngine;

public class InternalResouceOriginCreatorComponent : MonoBehaviour
{

    [SerializeField]
    private InternalResourceOriginCreator[] m_Origins;

    private void Start()
    {
        foreach ( InternalResourceOriginCreator origin in m_Origins )
        {
            ResourceOrigin resourceOrigin = origin.CreateOrigin();
            ResourceSystem.AddOrigin( resourceOrigin );
            ResourceSystem.Initialize();
        }
    }

}

using UnityEngine;

public class InternalResouceOriginCreatorComponent : MonoBehaviour
{

    [SerializeField]
    private InternalResourceOriginCreator[] m_Origins;

    private void Awake()
    {

        for ( int I = 0; I < m_Origins.Length; I++ )
        {
            InternalResourceOriginCreator origin = m_Origins[I];

            ResourceOrigin resourceOrigin = origin.CreateOrigin();
            ResourceSystem.AddOrigin( resourceOrigin );
        }
    }


}

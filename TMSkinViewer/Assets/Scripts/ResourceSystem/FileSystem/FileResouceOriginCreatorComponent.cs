using UnityEngine;

public class FileResouceOriginCreatorComponent : MonoBehaviour
{

    [SerializeField]
    private FileResourceOriginCreator[] m_Origins;

    private void Start()
    {
        foreach ( FileResourceOriginCreator origin in m_Origins )
        {
            ResourceOrigin resourceOrigin = origin.CreateOrigin();
            ResourceSystem.AddOrigin( resourceOrigin );
            ResourceSystem.Initialize();
        }
    }

}
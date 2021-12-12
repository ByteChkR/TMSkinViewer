using System;

using UnityEngine;

[Serializable]
public class InternalResourceOriginCreator
{

    [SerializeField]
    private string m_Name;
    [SerializeField]
    private InternalResourceOriginContent[] m_Content;


    public InternalResourceOrigin CreateOrigin()
    {
        return new InternalResourceOrigin( m_Name, m_Content );
    }

}
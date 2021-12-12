using System;

using UnityEngine;

[Serializable]
public class FileResourceOriginCreator
{

    [SerializeField]
    private string m_Name;
    [SerializeField]
    private string m_Path;


    public FileSystemResourceOrigin CreateOrigin()
    {
        return new FileSystemResourceOrigin( m_Name, m_Path );
    }

}
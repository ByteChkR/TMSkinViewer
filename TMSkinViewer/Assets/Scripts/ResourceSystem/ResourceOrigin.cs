using UnityEngine;

public abstract class ResourceOrigin
{

    private readonly string m_Name;

    protected ResourceOrigin( string name )
    {
        m_Name = name;
    }

    public string Name => m_Name;
    public abstract void Initialize(TaskCollection taskCollection);
    public abstract ResourceNodeRoot GetRootNode();

    public abstract Object GetResource( string path );

    public abstract Sprite GetIcon( string path );

}
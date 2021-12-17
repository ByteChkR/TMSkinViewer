using UnityEngine;

public abstract class ResourceOrigin
{

    private readonly string m_Name;

    public string Name => m_Name;

    #region Public

    public abstract Sprite GetIcon( string path );

    public abstract Object GetResource( string path );

    public abstract ResourceNodeRoot GetRootNode();

    public abstract void Initialize( TaskCollection taskCollection );

    #endregion

    #region Protected

    protected ResourceOrigin( string name )
    {
        m_Name = name;
    }

    #endregion

}

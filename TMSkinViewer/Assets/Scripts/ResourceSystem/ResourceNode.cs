using System;
using System.Collections.Generic;

using UnityEngine;

using Object = UnityEngine.Object;

public class ResourceNode
{

    public readonly ResourceOrigin Origin;

    public readonly ResourceType Type;

    public readonly string Name;

    private ResourceNode m_Parent;

    private readonly List < ResourceNode > m_Children;

    public string Path => Parent == null ? Name : Parent.Path + "/" + Name;

    public ResourceNode Parent => m_Parent;

    public IEnumerable < ResourceNode > Children => m_Children;

    #region Public

    public ResourceNode AddChild( string name, ResourceType type )
    {
        if ( !HasNode( name, type ) )
        {
            ResourceNode node = new ResourceNode( name, this, type, Origin );
            m_Children.Add( node );

            return node;
        }
        else
        {
            throw new Exception( $"Node {name} with type {type} already exists" );
        }
    }

    public Sprite GetIcon()
    {
        return Origin.GetIcon( Path );
    }

    public ResourceNode GetNode( string name )
    {
        return m_Children.Find( x => x.Name == name );
    }

    public ResourceNode GetNode( string name, ResourceType type )
    {
        return m_Children.Find( x => x.Name == name && x.Type == type );
    }

    public Object GetResource()
    {
        return Origin.GetResource( Path );
    }

    public bool HasNode( string name )
    {
        return m_Children.Exists( x => x.Name == name );
    }

    public bool HasNode( string name, ResourceType type )
    {
        return m_Children.Exists( x => x.Name == name && x.Type == type );
    }

    #endregion

    #region Protected

    protected ResourceNode( string name, ResourceNode parent, ResourceType type, ResourceOrigin origin )
    {
        Name = name;
        m_Parent = parent;
        Type = type;
        Origin = origin;
        m_Children = new List < ResourceNode >();
    }

    #endregion

}

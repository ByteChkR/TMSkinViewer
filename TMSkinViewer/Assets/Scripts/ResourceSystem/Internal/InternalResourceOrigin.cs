using System.Collections.Generic;

using UnityEngine;

public class InternalResourceOrigin : ResourceOrigin
{

    private readonly InternalResourceOriginContent[] m_Content;
    private ResourceNodeRoot m_Root;
    private readonly Dictionary < string, Object > m_Resources = new Dictionary < string, Object >();
    private readonly Dictionary < string, Sprite > m_Icons = new Dictionary < string, Sprite >();

    #region Public

    public InternalResourceOrigin( string name, InternalResourceOriginContent[] content ) : base( name )
    {
        m_Content = content;
    }

    public override Sprite GetIcon( string path )
    {
        Debug.Log( $"GetIcon for {path}" );

        if ( m_Icons.ContainsKey( path ) )
        {
            return m_Icons[path];
        }

        return ResourceSystem.GetDefaultIcon( ResourceType.Directory );
    }

    public override Object GetResource( string path )
    {
        return m_Resources[path];
    }

    public override ResourceNodeRoot GetRootNode()
    {
        return m_Root;
    }

    public override void Initialize( TaskCollection taskCollection )
    {
        m_Resources.Clear();
        m_Icons.Clear();
        m_Root = new ResourceNodeRoot( Name, this );

        m_Icons[Name] = ResourceSystem.GetDefaultIcon( ResourceType.Directory );

        foreach ( InternalResourceOriginContent content in m_Content )
        {
            ResourceNode node = m_Root.CreateNode( content.Path, content.Type );

            taskCollection.AddTask(
                                   $"Loading Object: {node.Name}",
                                   () => { m_Resources[node.Path] = content.Value; }
                                  );

            if ( content.Type == ResourceType.Texture )
            {
                taskCollection.AddTask(
                                       $"Generating Icon for {node.Name}",
                                       () =>
                                       {
                                           Texture2D tex = ( Texture2D )m_Resources[node.Path];

                                           m_Icons[node.Path] = Sprite.Create(
                                                                              tex,
                                                                              new Rect( 0, 0, tex.width, tex.height ),
                                                                              new Vector2( 0.5f, 0.5f )
                                                                             );
                                       }
                                      );
            }
            else
            {
                //Load Resource

                //Add Default Icon
                m_Icons[node.Path] = ResourceSystem.GetDefaultIcon( content.Type );
            }
        }
    }

    #endregion

}

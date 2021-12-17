using System.Collections.Generic;
using System.IO;

using UnityEngine;

public class FileSystemResourceOrigin : ResourceOrigin
{

    private readonly string m_RootPath;
    private ResourceNodeRoot m_RootNode;

    private readonly Dictionary < string, Object > m_Resources = new Dictionary < string, Object >();
    private readonly Dictionary < string, Sprite > m_Icons = new Dictionary < string, Sprite >();

    #region Public

    public FileSystemResourceOrigin( string name, string path ) : base( name )
    {
        m_RootPath = path;
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
        return m_RootNode;
    }

    public override void Initialize( TaskCollection taskCollection )
    {
        m_Icons.Clear();
        m_Resources.Clear();
        m_Icons[Name] = ResourceSystem.GetDefaultIcon( ResourceType.Directory );
        m_RootNode = new ResourceNodeRoot( Name, this );
        PopulateNode( m_RootPath, m_RootNode, taskCollection );
    }

    #endregion

    #region Private

    private Texture2D LoadTexture( string path )
    {
        Texture2D tex = new Texture2D( 2, 2 );
        tex.LoadImage( File.ReadAllBytes( path ) );

        return tex;
    }

    private void PopulateNode( string dir, ResourceNode node, TaskCollection taskCollection )
    {
        string[] files = Directory.GetFiles( dir, "*.*", SearchOption.TopDirectoryOnly );

        foreach ( string file in files )
        {
            string name = Path.GetFileName( file );
            ResourceType type = ResourceTypeParser.ParseFromFileExtension( name );
            ResourceNode child = node.AddChild( name, type, GetDefaultType(type) );

            if ( type == ResourceType.Texture )
            {
                string f = file;
                ResourceNode c = child;

                taskCollection.AddTask(
                                       $"Loading Texture: {name}",
                                       () =>
                                       {
                                           Texture2D tex = LoadTexture( f );
                                           m_Resources[c.Path] = tex;
                                       }
                                      );

                taskCollection.AddTask(
                                       $"Generating Icon for {name}",
                                       () =>
                                       {
                                           Texture2D tex = ( Texture2D )m_Resources[c.Path];

                                           m_Icons[c.Path] = Sprite.Create(
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
                m_Icons[child.Path] = ResourceSystem.GetDefaultIcon( type );
            }
        }

        string[] dirs = Directory.GetDirectories( dir, "*", SearchOption.TopDirectoryOnly );

        foreach ( string directory in dirs )
        {
            string name = Path.GetFileName( directory );
            ResourceNode child = node.AddChild( name, ResourceType.Directory , null);
            PopulateNode( directory, child, taskCollection );
        }
    }

    #endregion

}

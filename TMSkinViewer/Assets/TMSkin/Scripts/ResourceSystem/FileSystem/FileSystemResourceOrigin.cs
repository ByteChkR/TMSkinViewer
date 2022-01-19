using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

using UI.LoadingWindow;
using UI.SkinEditorMainWindow;

using UnityEngine;

using Object = UnityEngine.Object;

public class FileSystemResourceOrigin : ResourceOrigin
{

    private readonly string m_RootPath;
    private ResourceNodeRoot m_RootNode;
    private readonly bool m_AutoReload;
    private readonly FileSystemWatcher m_Watcher;

    private readonly ConcurrentQueue < Action > m_ChangeQueue = new ConcurrentQueue < Action >();

    private readonly Dictionary < string, Object > m_Resources = new Dictionary < string, Object >();
    private readonly Dictionary < string, Sprite > m_Icons = new Dictionary < string, Sprite >();

    #region Public

    public FileSystemResourceOrigin( string name, string path, bool autoReload ) : base( name )
    {
        m_RootPath = Path.GetFullPath(path);
        Directory.CreateDirectory( m_RootPath );
        m_AutoReload = autoReload;

        if ( m_AutoReload )
        {
            m_Watcher = new FileSystemWatcher( m_RootPath, "*.dds" );
            m_Watcher.IncludeSubdirectories = true;
            m_Watcher.Changed += (s, e) => m_ChangeQueue.Enqueue( () => OnFileChanged(s,e));
            m_Watcher.Created += (s, e) => m_ChangeQueue.Enqueue( () => OnFileCreated(s,e));;
            m_Watcher.Deleted += (s, e) => m_ChangeQueue.Enqueue( () => OnFileDeleted(s,e));;
            m_Watcher.Renamed += (s, e) => m_ChangeQueue.Enqueue( () => OnFileRenamed(s,e));;
            m_Watcher.EnableRaisingEvents = true;
        }
    }

    public void ProcessChangeQueue()
    {
        Action action;
        while ( m_ChangeQueue.TryDequeue( out action ) )
        {
            action();
        }
    }
    
    private void OnFileCreated( object sender, FileSystemEventArgs e )
    {
        string p =  e.FullPath.Replace( m_RootPath, m_RootNode.Name ).Replace('\\', '/') ;
        string name = Path.GetFileName( p );
        
        TaskCollection taskCollection = new TaskCollection();
        taskCollection.AddTask(
                               $"Loading Texture: {name}",
                               () =>
                               {
                                   using Stream stream = File.OpenRead( e.FullPath );
                                   CarTexture tex = SkinImporter.LoadTexture( stream, (int)stream.Length, true );
                                   m_Resources[p] = tex;
                               }
                              );

        taskCollection.AddTask(
                               $"Generating Icon for {name}",
                               () =>
                               {
                                   CarTexture tex = ( CarTexture )m_Resources[p];

                                   m_Icons[p] = Sprite.Create(
                                                              tex.Texture,
                                                              new Rect( 0, 0, tex.Texture.width, tex.Texture.height ),
                                                              new Vector2( 0.5f, 0.5f )
                                                             );
                               }
                              );
        LoadingWindow window = LoadingWindowBuilder.CreateWindow();
        window.Process( taskCollection );
    }

    private void OnFileRenamed( object sender, RenamedEventArgs e )
    {
        string p =  e.OldFullPath.Replace( m_RootPath, m_RootNode.Name ).Replace('\\', '/') ;
        string newP =e.FullPath.Replace( m_RootPath, m_RootNode.Name ).Replace('\\', '/') ;
        CarTexture tex = ( CarTexture ) m_Resources[ p ];
        Sprite s = m_Icons[p];
        m_Icons.Remove( p );
        m_Resources.Remove( p );
        m_Icons[newP] = s;
        m_Resources[newP] = tex;
    }

    private void OnFileDeleted( object sender, FileSystemEventArgs e )
    {
        string p =  e.FullPath.Replace( m_RootPath, m_RootNode.Name ).Replace('\\', '/') ;
        m_Icons.Remove( p );
        m_Resources.Remove( p );
    }

    private void OnFileChanged( object sender, FileSystemEventArgs e )
    {
        string p =  e.FullPath.Replace( m_RootPath, m_RootNode.Name ).Replace('\\', '/') ;

        string name = Path.GetFileName( p );
        if ( m_Resources.ContainsKey( p ) )
        {
            TaskCollection taskCollection = new TaskCollection();
            taskCollection.AddTask(
                                   $"Loading Texture: {name}",
                                   () =>
                                   {
                                       CarTexture tex = (CarTexture) m_Resources[ p ];
                                       using Stream s = File.OpenRead( e.FullPath );
                                       CarTexture newTex = SkinImporter.LoadTexture( s, (int)s.Length, false );
                                       tex.Texture.SetPixels(newTex.Texture.GetPixels());
                                       tex.Texture.Apply();
                                       tex.TextureData = newTex.TextureData;
                                   }
                                  );

            taskCollection.AddTask(
                                   $"Generating Icon for {name}",
                                   () =>
                                   {
                                       CarTexture tex = ( CarTexture )m_Resources[p];

                                       m_Icons[p] = Sprite.Create(
                                                                  tex.Texture,
                                                                  new Rect( 0, 0, tex.Texture.width, tex.Texture.height ),
                                                                  new Vector2( 0.5f, 0.5f )
                                                                 );
                                   }
                                  );
            LoadingWindow window = LoadingWindowBuilder.CreateWindow();
            window.Process( taskCollection );
        }
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
            ResourceNode child = node.AddChild( name, type, GetDefaultType( type ) );

            if ( type == ResourceType.Texture )
            {
                string f = file;
                ResourceNode c = child;

                taskCollection.AddTask(
                                       $"Loading Texture: {name}",
                                       () =>
                                       {
                                           using Stream stream = File.OpenRead( f );
                                           CarTexture tex = SkinImporter.LoadTexture( stream, (int)stream.Length, true );
                                           m_Resources[c.Path] = tex;
                                       }
                                      );

                taskCollection.AddTask(
                                       $"Generating Icon for {name}",
                                       () =>
                                       {
                                           CarTexture tex = ( CarTexture )m_Resources[c.Path];

                                           m_Icons[c.Path] = Sprite.Create(
                                                                           tex.Texture,
                                                                           new Rect( 0, 0, tex.Texture.width, tex.Texture.height ),
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
            ResourceNode child = node.AddChild( name, ResourceType.Directory, null );
            PopulateNode( directory, child, taskCollection );
        }
    }

    #endregion

}

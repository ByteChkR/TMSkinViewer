using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

using S16.Drawing;

using UI.LoadingWindow;

using UnityEngine;

using Object = UnityEngine.Object;

public static class SkinImporter
{

    private class SkinImporterResourceOrigin : ResourceOrigin
    {

        private ResourceNodeRoot m_Root;
        private readonly Dictionary < string, Object > m_Resources = new Dictionary < string, Object >();
        private readonly Dictionary < string, Sprite > m_Icons = new Dictionary < string, Sprite >();

        #region Public

        public SkinImporterResourceOrigin( string name ) : base( name )
        {
            m_Root = new ResourceNodeRoot( name, this );
        }

        public void AddTexture( string nodePath, CarTexture resource )
        {
            string path = nodePath.Replace( '\\', '/' );
            ResourceNode node = m_Root.CreateNode( path, ResourceType.Texture, typeof( Texture2D ) );

            m_Resources.Add( $"Imported Skins/{path}", resource );

            m_Icons.Add(
                        $"Imported Skins/{path}",
                        Sprite.Create(
                                      resource.Texture,
                                      new Rect( 0, 0, resource.Texture.width, resource.Texture.height ),
                                      new Vector2( 0.5f, 0.5f )
                                     )
                       );
        }

        public override Sprite GetIcon( string path )
        {
            if ( m_Icons.TryGetValue( path, out Sprite sprite ) )
            {
                return sprite;
            }
            else
            {
                return ResourceSystem.GetDefaultIcon( ResourceType.Directory );
            }
        }

        public override Object GetResource( string path )
        {
            Object o = m_Resources[path];

            return o;
        }

        public override ResourceNodeRoot GetRootNode()
        {
            return m_Root;
        }

        public override void Initialize( TaskCollection taskCollection )
        {
        }

        #endregion

    }

    private static readonly SkinImporterResourceOrigin s_SkinImporterResources =
        new SkinImporterResourceOrigin( "Imported Skins" );

    public static ResourceOrigin SkinImporterResources => s_SkinImporterResources;

    #region Public

    public static void Import( CarSkin skin, Stream data, Action onComplete )
    {
        Import( new SkinImporterArgs( skin, data ), onComplete );
    }

    public static void Import( SkinImporterArgs args, Action onComplete )
    {
        TaskCollection tasks = new TaskCollection();
        Import( args, tasks );

        ShowDialog( tasks, onComplete );
    }

    public static void Import( SkinImporterArgs args, TaskCollection tasks )
    {
        ZipArchive archive = new ZipArchive( args.Data, ZipArchiveMode.Read );

        LoadSkinMaterial( args.Skin.Skin, archive, tasks, args.ResourcePath, args.ImportAsDefault );
        LoadDetailsMaterial( args.Skin.Details, archive, tasks, args.ResourcePath, args.ImportAsDefault );
        LoadWheelMaterial( args.Skin.Wheel, archive, tasks, args.ResourcePath, args.ImportAsDefault );
        LoadGlassMaterial( args.Skin.Glass, archive, tasks, args.ResourcePath, args.ImportAsDefault );

        tasks.AddTask(
                      "Skin Import Cleanup",
                      () =>
                      {
                          archive.Dispose();
                          args.Dispose();
                      }
                     );
    }

    public static void Import( CarSkin skin, Stream data, TaskCollection tasks )
    {
        Import( new SkinImporterArgs( skin, data ), tasks );
    }

    public static void Import( SkinImporterArgs[] args, TaskCollection tasks )
    {
        foreach ( SkinImporterArgs importerArgs in args )
        {
            Import( importerArgs, tasks );
        }
    }

    public static void Import( SkinImporterArgs[] args, Action onComplete )
    {
        TaskCollection tasks = new TaskCollection();
        Import( args, tasks );

        ShowDialog( tasks, onComplete );
    }

    #endregion

    #region Private


    private static void LoadDetailsMaterial(
        CarMaterial material,
        ZipArchive archive,
        TaskCollection tasks,
        string resourcePath,
        bool importAsDefault )
    {
        tasks.AddTask(
                      "Loading Details_AO.dds",
                      () =>
                      {
                          ZipArchiveEntry ao = archive.GetEntry( "Details_AO.dds" );

                          if ( ao != null )
                          {
                              CarTexture texture = LoadTexture( ao,importAsDefault );

                              if ( texture != null )
                              {
                                  material.AmbientOcclusion = texture;

                                  if ( resourcePath != null )
                                  {
                                      s_SkinImporterResources.AddTexture(
                                                                         Path.Combine(
                                                                              resourcePath,
                                                                              material.MaterialName,
                                                                              "Details_AO.dds"
                                                                             ),
                                                                         texture
                                                                        );
                                  }
                              }
                          }
                      }
                     );

        tasks.AddTask(
                      "Loading Details_B.dds",
                      () =>
                      {
                          ZipArchiveEntry b = archive.GetEntry( "Details_B.dds" );

                          if ( b != null )
                          {
                              CarTexture texture = LoadTexture( b,importAsDefault );

                              if ( texture != null )
                              {
                                  material.Albedo = texture;

                                  if ( resourcePath != null )
                                  {
                                      s_SkinImporterResources.AddTexture(
                                                                         Path.Combine(
                                                                              resourcePath,
                                                                              material.MaterialName,
                                                                              "Details_B.dds"
                                                                             ),
                                                                         texture
                                                                        );
                                  }
                              }
                          }
                      }
                     );

        tasks.AddTask(
                      "Loading Details_DirtMask.dds",
                      () =>
                      {
                          ZipArchiveEntry dirtMask = archive.GetEntry( "Details_DirtMask.dds" );

                          if ( dirtMask != null )
                          {
                              CarTexture texture = LoadTexture( dirtMask,importAsDefault );

                              if ( texture != null )
                              {
                                  material.DirtMask = texture;

                                  if ( resourcePath != null )
                                  {
                                      s_SkinImporterResources.AddTexture(
                                                                         Path.Combine(
                                                                              resourcePath,
                                                                              material.MaterialName,
                                                                              "Details_DirtMask.dds"
                                                                             ),
                                                                         texture
                                                                        );
                                  }
                              }
                          }
                      }
                     );

        tasks.AddTask(
                      "Loading Details_I.dds",
                      () =>
                      {
                          ZipArchiveEntry i = archive.GetEntry( "Details_I.dds" );

                          if ( i != null )
                          {
                              CarTexture texture = LoadTexture( i,importAsDefault );

                              if ( texture != null )
                              {
                                  material.Emissive = texture;

                                  if ( resourcePath != null )
                                  {
                                      s_SkinImporterResources.AddTexture(
                                                                         Path.Combine(
                                                                              resourcePath,
                                                                              material.MaterialName,
                                                                              "Details_I.dds"
                                                                             ),
                                                                         texture
                                                                        );
                                  }
                              }
                          }
                      }
                     );

        tasks.AddTask(
                      "Loading Details_N.dds",
                      () =>
                      {
                          ZipArchiveEntry n = archive.GetEntry( "Details_N.dds" );

                          if ( n != null )
                          {
                              CarTexture texture = LoadTexture( n,importAsDefault );

                              if ( texture != null )
                              {
                                  material.Normal = texture;

                                  if ( resourcePath != null )
                                  {
                                      s_SkinImporterResources.AddTexture(
                                                                         Path.Combine(
                                                                              resourcePath,
                                                                              material.MaterialName,
                                                                              "Details_N.dds"
                                                                             ),
                                                                         texture
                                                                        );
                                  }
                              }
                          }
                      }
                     );

        tasks.AddTask(
                      "Loading Details_R.dds",
                      () =>
                      {
                          ZipArchiveEntry r = archive.GetEntry( "Details_R.dds" );

                          if ( r != null )
                          {
                              CarTexture texture = LoadTexture( r,importAsDefault );

                              if ( texture != null )
                              {
                                  material.Roughness = texture;

                                  if ( resourcePath != null )
                                  {
                                      s_SkinImporterResources.AddTexture(
                                                                         Path.Combine(
                                                                              resourcePath,
                                                                              material.MaterialName,
                                                                              "Details_R.dds"
                                                                             ),
                                                                         texture
                                                                        );
                                  }
                              }
                          }
                      }
                     );
    }

    private static void LoadGlassMaterial(
        CarMaterial material,
        ZipArchive archive,
        TaskCollection tasks,
        string resourcePath,
        bool importAsDefault )
    {
        tasks.AddTask(
                      "Loading Glass_AO.dds",
                      () =>
                      {
                          ZipArchiveEntry ao = archive.GetEntry( "Glass_AO.dds" );

                          if ( ao != null )
                          {
                              CarTexture texture = LoadTexture( ao,importAsDefault );

                              if ( texture != null )
                              {
                                  material.AmbientOcclusion = texture;

                                  if ( resourcePath != null )
                                  {
                                      s_SkinImporterResources.AddTexture(
                                                                         Path.Combine(
                                                                              resourcePath,
                                                                              material.MaterialName,
                                                                              "Glass_AO.dds"
                                                                             ),
                                                                         texture
                                                                        );
                                  }
                              }
                          }
                      }
                     );

        tasks.AddTask(
                      "Loading Glass_D.dds",
                      () =>
                      {
                          ZipArchiveEntry d = archive.GetEntry( "Glass_D.dds" );

                          if ( d != null )
                          {
                              CarTexture texture = LoadTexture( d,importAsDefault );

                              if ( texture != null )
                              {
                                  material.Albedo = texture;

                                  if ( resourcePath != null )
                                  {
                                      s_SkinImporterResources.AddTexture(
                                                                         Path.Combine(
                                                                              resourcePath,
                                                                              material.MaterialName,
                                                                              "Glass_D.dds"
                                                                             ),
                                                                         texture
                                                                        );
                                  }
                              }
                          }
                      }
                     );

        tasks.AddTask(
                      "Loading Glass_I.dds",
                      () =>
                      {
                          ZipArchiveEntry i = archive.GetEntry( "Glass_I.dds" );

                          if ( i != null )
                          {
                              CarTexture texture = LoadTexture( i,importAsDefault );

                              if ( texture != null )
                              {
                                  material.Emissive = texture;

                                  if ( resourcePath != null )
                                  {
                                      s_SkinImporterResources.AddTexture(
                                                                         Path.Combine(
                                                                              resourcePath,
                                                                              material.MaterialName,
                                                                              "Glass_I.dds"
                                                                             ),
                                                                         texture
                                                                        );
                                  }
                              }
                          }
                      }
                     );
    }

    private static void LoadSkinMaterial(
        CarMaterial material,
        ZipArchive archive,
        TaskCollection tasks,
        string resourcePath,
        bool importAsDefault)
    {
        tasks.AddTask(
                      "Loading Skin_AO.dds",
                      () =>
                      {
                          ZipArchiveEntry ao = archive.GetEntry( "Skin_AO.dds" );

                          if ( ao != null )
                          {
                              CarTexture texture = LoadTexture( ao,importAsDefault );

                              if ( texture != null )
                              {
                                  material.AmbientOcclusion = texture;

                                  if ( resourcePath != null )
                                  {
                                      s_SkinImporterResources.AddTexture(
                                                                         Path.Combine(
                                                                              resourcePath,
                                                                              material.MaterialName,
                                                                              "Skin_AO.dds"
                                                                             ),
                                                                         texture
                                                                        );
                                  }
                              }
                          }
                      }
                     );

        tasks.AddTask(
                      "Loading Skin_B.dds",
                      () =>
                      {
                          ZipArchiveEntry b = archive.GetEntry( "Skin_B.dds" );

                          if ( b != null )
                          {
                              CarTexture texture = LoadTexture( b,importAsDefault );

                              if ( texture != null )
                              {
                                  material.Albedo = texture;

                                  if ( resourcePath != null )
                                  {
                                      s_SkinImporterResources.AddTexture(
                                                                         Path.Combine(
                                                                              resourcePath,
                                                                              material.MaterialName,
                                                                              "Skin_B.dds"
                                                                             ),
                                                                         texture
                                                                        );
                                  }
                              }
                          }
                      }
                     );

        tasks.AddTask(
                      "Loading Skin_DirtMask.dds",
                      () =>
                      {
                          ZipArchiveEntry dirtMask = archive.GetEntry( "Skin_DirtMask.dds" );

                          if ( dirtMask != null )
                          {
                              CarTexture texture = LoadTexture( dirtMask,importAsDefault );

                              if ( texture != null )
                              {
                                  material.DirtMask = texture;

                                  if ( resourcePath != null )
                                  {
                                      s_SkinImporterResources.AddTexture(
                                                                         Path.Combine(
                                                                              resourcePath,
                                                                              material.MaterialName,
                                                                              "Skin_DirtMask.dds"
                                                                             ),
                                                                         texture
                                                                        );
                                  }
                              }
                          }
                      }
                     );

        tasks.AddTask(
                      "Loading Skin_I.dds",
                      () =>
                      {
                          ZipArchiveEntry i = archive.GetEntry( "Skin_I.dds" );

                          if ( i != null )
                          {
                              CarTexture texture = LoadTexture( i,importAsDefault );

                              if ( texture != null )
                              {
                                  material.Emissive = texture;

                                  if ( resourcePath != null )
                                  {
                                      s_SkinImporterResources.AddTexture(
                                                                         Path.Combine(
                                                                              resourcePath,
                                                                              material.MaterialName,
                                                                              "Skin_I.dds"
                                                                             ),
                                                                         texture
                                                                        );
                                  }
                              }
                          }
                      }
                     );

        tasks.AddTask(
                      "Loading Skin_R.dds",
                      () =>
                      {
                          ZipArchiveEntry r = archive.GetEntry( "Skin_R.dds" );

                          if ( r != null )
                          {
                              CarTexture texture = LoadTexture( r,importAsDefault );

                              if ( texture != null )
                              {
                                  material.Roughness = texture;

                                  if ( resourcePath != null )
                                  {
                                      s_SkinImporterResources.AddTexture(
                                                                         Path.Combine(
                                                                              resourcePath,
                                                                              material.MaterialName,
                                                                              "Skin_R.dds"
                                                                             ),
                                                                         texture
                                                                        );
                                  }
                              }
                          }
                      }
                     );
    }

    private static CarTexture LoadTexture( ZipArchiveEntry entry, bool importAsDefault )
    {
        CarTexture ret = ScriptableObject.CreateInstance < CarTexture >();
        ret.IsDefault = importAsDefault;

        using ( Stream s = entry.Open() )
        {
            ret.TextureData = new byte[entry.Length];

            s.Read( ret.TextureData, 0, ( int )entry.Length );

            try
            {
                DDSImage image = new DDSImage( ret.TextureData );

                ret.Texture = image.BitmapImage;
            }
            catch ( Exception e )
            {
                Debug.LogError( "Can not Load Texture: " + entry.Name + " " + e.Message );

                return null;
            }

            return ret;
        }
    }

    private static void LoadWheelMaterial(
        CarMaterial material,
        ZipArchive archive,
        TaskCollection tasks,
        string resourcePath,
        bool importAsDefault )
    {
        tasks.AddTask(
                      "Loading Wheels_AO.dds",
                      () =>
                      {
                          ZipArchiveEntry ao = archive.GetEntry( "Wheels_AO.dds" );

                          if ( ao != null )
                          {
                              CarTexture texture = LoadTexture( ao,importAsDefault );

                              if ( texture != null )
                              {
                                  material.AmbientOcclusion = texture;

                                  if ( resourcePath != null )
                                  {
                                      s_SkinImporterResources.AddTexture(
                                                                         Path.Combine(
                                                                              resourcePath,
                                                                              material.MaterialName,
                                                                              "Wheels_AO.dds"
                                                                             ),
                                                                         texture
                                                                        );
                                  }
                              }
                          }
                      }
                     );

        tasks.AddTask(
                      "Loading Wheels_B.dds",
                      () =>
                      {
                          ZipArchiveEntry b = archive.GetEntry( "Wheels_B.dds" );

                          if ( b != null )
                          {
                              CarTexture texture = LoadTexture( b,importAsDefault );

                              if ( texture != null )
                              {
                                  material.Albedo = texture;

                                  if ( resourcePath != null )
                                  {
                                      s_SkinImporterResources.AddTexture(
                                                                         Path.Combine(
                                                                              resourcePath,
                                                                              material.MaterialName,
                                                                              "Wheels_B.dds"
                                                                             ),
                                                                         texture
                                                                        );
                                  }
                              }
                          }
                      }
                     );

        tasks.AddTask(
                      "Loading Wheels_DirtMask.dds",
                      () =>
                      {
                          ZipArchiveEntry dirtMask = archive.GetEntry( "Wheels_DirtMask.dds" );

                          if ( dirtMask != null )
                          {
                              CarTexture texture = LoadTexture( dirtMask,importAsDefault );

                              if ( texture != null )
                              {
                                  material.DirtMask = texture;

                                  if ( resourcePath != null )
                                  {
                                      s_SkinImporterResources.AddTexture(
                                                                         Path.Combine(
                                                                              resourcePath,
                                                                              material.MaterialName,
                                                                              "Wheels_DirtMask.dds"
                                                                             ),
                                                                         texture
                                                                        );
                                  }
                              }
                          }
                      }
                     );

        tasks.AddTask(
                      "Loading Wheels_I.dds",
                      () =>
                      {
                          ZipArchiveEntry i = archive.GetEntry( "Wheels_I.dds" );

                          if ( i != null )
                          {
                              CarTexture texture = LoadTexture( i,importAsDefault );

                              if ( texture != null )
                              {
                                  material.Emissive = texture;

                                  if ( resourcePath != null )
                                  {
                                      s_SkinImporterResources.AddTexture(
                                                                         Path.Combine(
                                                                              resourcePath,
                                                                              material.MaterialName,
                                                                              "Wheels_I.dds"
                                                                             ),
                                                                         texture
                                                                        );
                                  }
                              }
                          }
                      }
                     );

        tasks.AddTask(
                      "Loading Wheels_N.dds",
                      () =>
                      {
                          ZipArchiveEntry n = archive.GetEntry( "Wheels_N.dds" );

                          if ( n != null )
                          {
                              CarTexture texture = LoadTexture( n,importAsDefault );

                              if ( texture != null )
                              {
                                  material.Normal = texture;

                                  if ( resourcePath != null )
                                  {
                                      s_SkinImporterResources.AddTexture(
                                                                         Path.Combine(
                                                                              resourcePath,
                                                                              material.MaterialName,
                                                                              "Wheels_N.dds"
                                                                             ),
                                                                         texture
                                                                        );
                                  }
                              }
                          }
                      }
                     );

        tasks.AddTask(
                      "Loading Wheels_R.dds",
                      () =>
                      {
                          ZipArchiveEntry r = archive.GetEntry( "Wheels_R.dds" );

                          if ( r != null )
                          {
                              CarTexture texture = LoadTexture( r,importAsDefault );

                              if ( texture != null )
                              {
                                  material.Roughness = texture;

                                  if ( resourcePath != null )
                                  {
                                      s_SkinImporterResources.AddTexture(
                                                                         Path.Combine(
                                                                              resourcePath,
                                                                              material.MaterialName,
                                                                              "Wheels_R.dds"
                                                                             ),
                                                                         texture
                                                                        );
                                  }
                              }
                          }
                      }
                     );
    }

    private static void ShowDialog( TaskCollection tasks, Action onComplete )
    {
        LoadingWindow window = LoadingWindowBuilder.CreateWindow();

        window.OnComplete += () => { onComplete?.Invoke(); };

        window.Process( tasks );
    }

    #endregion

}

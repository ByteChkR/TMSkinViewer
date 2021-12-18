using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

using S16.Drawing;

using UI.LoadingWindow;

using UnityEngine;

public readonly struct SkinImporterArgs : IDisposable
{

    public readonly CarSkin Skin;
    public readonly Stream Data;

    public SkinImporterArgs(CarSkin skin, Stream data)
    {
        Skin = skin;
        Data = data;
    }

    public void Dispose()
    {
        Data?.Dispose();
    }

}
public static class SkinImporter
{

    #region Public

    public static void Import( CarSkin skin, Stream data, Action onComplete)
    {
        Import(new SkinImporterArgs(skin, data), onComplete);
    }

    private static void ShowDialog( TaskCollection tasks, Action onComplete )
    {
        LoadingWindow window = LoadingWindowBuilder.CreateWindow();
        
        window.OnComplete += () =>
                             {
                                 onComplete?.Invoke();
                             };
        
        window.Process(tasks);
    }
    public static void Import( SkinImporterArgs args , Action onComplete)
    {
        TaskCollection tasks = new TaskCollection();
        Import( args, tasks );

        ShowDialog(tasks, onComplete);
        
    }
    public static void Import( SkinImporterArgs args, TaskCollection tasks)
    {
        ZipArchive archive = new ZipArchive( args.Data, ZipArchiveMode.Read );

        
        LoadSkinMaterial( args.Skin.Skin, archive, tasks);
        LoadDetailsMaterial( args.Skin.Details, archive , tasks);
        LoadWheelMaterial( args.Skin.Wheel, archive, tasks );
        LoadGlassMaterial( args.Skin.Glass, archive, tasks );
        tasks.AddTask("Skin Import Cleanup",
                      () =>
                      {
                          archive.Dispose();
                          args.Dispose();
                      });
    }

    public static void Import( CarSkin skin, Stream data, TaskCollection tasks )
    {
        Import(new SkinImporterArgs(skin, data), tasks);
    }

    public static void Import( SkinImporterArgs[] args, TaskCollection tasks )
    {
        foreach ( SkinImporterArgs importerArgs in args )
        {
            Import(importerArgs, tasks);
        }
    }

    public static void Import( SkinImporterArgs[] args , Action onComplete )
    {
        TaskCollection tasks = new TaskCollection();
        Import(args, tasks);
        
        ShowDialog(tasks, onComplete);
    }

    #endregion

    #region Private

    private static void LoadDetailsMaterial( CarMaterial material, ZipArchive archive, TaskCollection tasks )
    {
        tasks.AddTask("Loading Details_AO.dds",
                      () =>
                      {
                          ZipArchiveEntry ao = archive.GetEntry( "Details_AO.dds" );

                          if ( ao != null )
                          {
                              Texture2D texture = LoadTexture( ao );

                              if ( texture != null )
                              {
                                  material.AmbientOcclusion = texture;
                              }
                          }
                      });

        
        tasks.AddTask("Loading Details_AO.dds",
                      () =>
                      {
                          ZipArchiveEntry b = archive.GetEntry( "Details_AO.dds" );

                          if ( b != null )
                          {
                              Texture2D texture = LoadTexture( b );

                              if ( texture != null )
                              {
                                  material.Albedo = texture;
                              }
                          }
                      });
        
        
        tasks.AddTask("Loading Details_DirtMask.dds",
                      () =>
                      {
                          ZipArchiveEntry dirtMask = archive.GetEntry( "Details_DirtMask.dds" );

                          if ( dirtMask != null )
                          {
                              Texture2D texture = LoadTexture( dirtMask );

                              if ( texture != null )
                              {
                                  material.DirtMask = texture;
                              }
                          }
                      });

        
        tasks.AddTask("Loading Details_I.dds",
                      () =>
                      {
                          ZipArchiveEntry i = archive.GetEntry( "Details_I.dds" );

                          if ( i != null )
                          {
                              Texture2D texture = LoadTexture( i );

                              if ( texture != null )
                              {
                                  material.Emissive = texture;
                              }
                          }
                      });

        tasks.AddTask("Loading Details_N.dds",
                      () =>
                      {
                          ZipArchiveEntry n = archive.GetEntry( "Details_N.dds" );

                          if ( n != null )
                          {
                              Texture2D texture = LoadTexture( n );

                              if ( texture != null )
                              {
                                  material.Normal = texture;
                              }
                          }
                      });
        
        
        tasks.AddTask("Loading Details_R.dds",
                      () =>
                      {
                          ZipArchiveEntry r = archive.GetEntry( "Details_R.dds" );

                          if ( r != null )
                          {
                              Texture2D texture = LoadTexture( r );

                              if ( texture != null )
                              {
                                  material.Roughness = texture;
                              }
                          }
                      });
    }

    private static void LoadGlassMaterial( CarMaterial material, ZipArchive archive, TaskCollection tasks )
    {
        tasks.AddTask("Loading Glass_AO.dds",
                      () =>
                      {
                          ZipArchiveEntry ao = archive.GetEntry( "Glass_AO.dds" );

                          if ( ao != null )
                          {
                              Texture2D texture = LoadTexture( ao );

                              if ( texture != null )
                              {
                                  material.AmbientOcclusion = texture;
                              }
                          }
                      });
        
        tasks.AddTask("Loading Glass_D.dds",
                      () =>
                      {
                          ZipArchiveEntry d = archive.GetEntry( "Glass_D.dds" );

                          if ( d != null )
                          {
                              Texture2D texture = LoadTexture( d );

                              if ( texture != null )
                              {
                                  material.Albedo = texture;
                              }
                          }
                      });
        
        tasks.AddTask("Loading Glass_I.dds",
                      () =>
                      {
                          ZipArchiveEntry i = archive.GetEntry( "Glass_I.dds" );

                          if ( i != null )
                          {
                              Texture2D texture = LoadTexture( i );

                              if ( texture != null )
                              {
                                  material.Emissive = texture;
                              }
                          }
                      });
    }

    private static void LoadSkinMaterial( CarMaterial material, ZipArchive archive , TaskCollection tasks)
    {
        
        tasks.AddTask("Loading Skin_AO.dds",
                      () =>
                      {
                          ZipArchiveEntry ao = archive.GetEntry( "Skin_AO.dds" );

                          if ( ao != null )
                          {
                              Texture2D texture = LoadTexture( ao );

                              if ( texture != null )
                              {
                                  material.AmbientOcclusion = texture;
                              }
                          }
                      });
        
        
        tasks.AddTask("Loading Skin_B.dds",
                      () =>
                      {
                          ZipArchiveEntry b = archive.GetEntry( "Skin_B.dds" );

                          if ( b != null )
                          {
                              Texture2D texture = LoadTexture( b );

                              if ( texture != null )
                              {
                                  material.Albedo = texture;
                              }
                          }
                      });
        

        tasks.AddTask("Loading Skin_DirtMask.dds",
                      () =>
                      {
                          ZipArchiveEntry dirtMask = archive.GetEntry( "Skin_DirtMask.dds" );

                          if ( dirtMask != null )
                          {
                              Texture2D texture = LoadTexture( dirtMask );

                              if ( texture != null )
                              {
                                  material.DirtMask = texture;
                              }
                          }
                      });

        tasks.AddTask("Loading Skin_I.dds",
                      () =>
                      {
                          ZipArchiveEntry i = archive.GetEntry( "Skin_I.dds" );

                          if ( i != null )
                          {
                              Texture2D texture = LoadTexture( i );

                              if ( texture != null )
                              {
                                  material.Emissive = texture;
                              }
                          }
                      });
        tasks.AddTask("Loading Skin_R.dds",
                      () =>
                      {
                          ZipArchiveEntry r = archive.GetEntry( "Skin_R.dds" );

                          if ( r != null )
                          {
                              Texture2D texture = LoadTexture( r );

                              if ( texture != null )
                              {
                                  material.Roughness = texture;
                              }
                          }
                      });
        
    }

    private static Texture2D LoadTexture( ZipArchiveEntry entry )
    {
        using ( Stream s = entry.Open() )
        {
            byte[] data = new byte[entry.Length];

            s.Read( data, 0, ( int )entry.Length );

            Texture2D tex = null;

            try
            {
                DDSImage image = new DDSImage( data );
                tex = image.BitmapImage;
            }
            catch ( Exception e )
            {
                Debug.LogError( "Can not Load Texture: " + entry.Name + " " + e.Message );

                return null;
            }

            return tex;
        }
    }

    private static void LoadWheelMaterial( CarMaterial material, ZipArchive archive, TaskCollection tasks )
    {
        
        tasks.AddTask("Loading Wheels_AO.dds",
                      () =>
                      {
                          ZipArchiveEntry ao = archive.GetEntry( "Wheels_AO.dds" );

                          if ( ao != null )
                          {
                              Texture2D texture = LoadTexture( ao );

                              if ( texture != null )
                              {
                                  material.AmbientOcclusion = texture;
                              }
                          }
                      });
        
        tasks.AddTask("Loading Wheels_B.dds",
                      () =>
                      {
                          ZipArchiveEntry b = archive.GetEntry( "Wheels_B.dds" );

                          if ( b != null )
                          {
                              Texture2D texture = LoadTexture( b );

                              if ( texture != null )
                              {
                                  material.Albedo = texture;
                              }
                          }
                      });
        
        
        tasks.AddTask("Loading Wheels_DirtMask.dds",
                      () =>
                      {
                          ZipArchiveEntry dirtMask = archive.GetEntry( "Wheels_DirtMask.dds" );

                          if ( dirtMask != null )
                          {
                              Texture2D texture = LoadTexture( dirtMask );

                              if ( texture != null )
                              {
                                  material.DirtMask = texture;
                              }
                          }
                      });
        
        
        tasks.AddTask("Loading Wheels_I.dds",
                      () =>
                      {
                          ZipArchiveEntry i = archive.GetEntry( "Wheels_I.dds" );

                          if ( i != null )
                          {
                              Texture2D texture = LoadTexture( i );

                              if ( texture != null )
                              {
                                  material.Emissive = texture;
                              }
                          }
                      });
        
        
        
        tasks.AddTask("Loading Wheels_N.dds",
                      () =>
                      {
                          ZipArchiveEntry n = archive.GetEntry( "Wheels_N.dds" );

                          if ( n != null )
                          {
                              Texture2D texture = LoadTexture( n );

                              if ( texture != null )
                              {
                                  material.Normal = texture;
                              }
                          }
                      });
        
        tasks.AddTask("Loading Wheels_R.dds",
                      () =>
                      {
                          ZipArchiveEntry r = archive.GetEntry( "Wheels_R.dds" );

                          if ( r != null )
                          {
                              Texture2D texture = LoadTexture( r );

                              if ( texture != null )
                              {
                                  material.Roughness = texture;
                              }
                          }
                      });
    }

    #endregion

}

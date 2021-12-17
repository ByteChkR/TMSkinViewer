using System;
using System.IO;
using System.IO.Compression;

using S16.Drawing;

using UnityEngine;

public static class SkinImporter
{

    #region Public

    public static void Import( CarSkin skin, Stream data )
    {
        ZipArchive archive = new ZipArchive( data, ZipArchiveMode.Read );

        LoadSkinMaterial( skin.Skin, archive );
        LoadDetailsMaterial( skin.Details, archive );
        LoadWheelMaterial( skin.Wheel, archive );
        LoadGlassMaterial( skin.Glass, archive );
    }

    #endregion

    #region Private

    private static void LoadDetailsMaterial( CarMaterial material, ZipArchive archive )
    {
        //Details_AO.dds
        ZipArchiveEntry ao = archive.GetEntry( "Details_AO.dds" );

        if ( ao != null )
        {
            Texture2D texture = LoadTexture( ao );

            if ( texture != null )
            {
                material.AmbientOcclusion = texture;
            }
        }

        //Details_B.dds
        ZipArchiveEntry b = archive.GetEntry( "Details_B.dds" );

        if ( b != null )
        {
            Texture2D texture = LoadTexture( b );

            if ( texture != null )
            {
                material.Albedo = texture;
            }
        }

        //Details_DirtMask.dds
        ZipArchiveEntry dirtMask = archive.GetEntry( "Details_DirtMask.dds" );

        if ( dirtMask != null )
        {
            Texture2D texture = LoadTexture( dirtMask );

            if ( texture != null )
            {
                material.DirtMask = texture;
            }
        }

        //Details_I.dds
        ZipArchiveEntry i = archive.GetEntry( "Details_I.dds" );

        if ( i != null )
        {
            Texture2D texture = LoadTexture( i );

            if ( texture != null )
            {
                material.Emissive = texture;
            }
        }

        //Details_N.dds
        ZipArchiveEntry n = archive.GetEntry( "Details_N.dds" );

        if ( n != null )
        {
            Texture2D texture = LoadTexture( n );

            if ( texture != null )
            {
                material.Normal = texture;
            }
        }

        //Details_R.dds
        ZipArchiveEntry r = archive.GetEntry( "Details_R.dds" );

        if ( r != null )
        {
            Texture2D texture = LoadTexture( r );

            if ( texture != null )
            {
                material.Roughness = texture;
            }
        }
    }

    private static void LoadGlassMaterial( CarMaterial material, ZipArchive archive )
    {
        //Glass_AO.dds
        ZipArchiveEntry ao = archive.GetEntry( "Glass_AO.dds" );

        if ( ao != null )
        {
            Texture2D texture = LoadTexture( ao );

            if ( texture != null )
            {
                material.AmbientOcclusion = texture;
            }
        }

        //Glass_D.dds
        ZipArchiveEntry d = archive.GetEntry( "Glass_D.dds" );

        if ( d != null )
        {
            Texture2D texture = LoadTexture( d );

            if ( texture != null )
            {
                material.Albedo = texture;
            }
        }

        //Glass_I.dds
        ZipArchiveEntry i = archive.GetEntry( "Glass_I.dds" );

        if ( i != null )
        {
            Texture2D texture = LoadTexture( i );

            if ( texture != null )
            {
                material.Emissive = texture;
            }
        }
    }

    private static void LoadSkinMaterial( CarMaterial material, ZipArchive archive )
    {
        //Skin_AO.dds
        ZipArchiveEntry ao = archive.GetEntry( "Skin_AO.dds" );

        if ( ao != null )
        {
            Texture2D texture = LoadTexture( ao );

            if ( texture != null )
            {
                material.AmbientOcclusion = texture;
            }
        }

        //Skin_B.dds
        ZipArchiveEntry b = archive.GetEntry( "Skin_B.dds" );

        if ( b != null )
        {
            Texture2D texture = LoadTexture( b );

            if ( texture != null )
            {
                material.Albedo = texture;
            }
        }

        //Skin_DirtMask.dds
        ZipArchiveEntry dirtMask = archive.GetEntry( "Skin_DirtMask.dds" );

        if ( dirtMask != null )
        {
            Texture2D texture = LoadTexture( dirtMask );

            if ( texture != null )
            {
                material.DirtMask = texture;
            }
        }

        //Skin_I.dds
        ZipArchiveEntry i = archive.GetEntry( "Skin_I.dds" );

        if ( i != null )
        {
            Texture2D texture = LoadTexture( i );

            if ( texture != null )
            {
                material.Emissive = texture;
            }
        }

        //Skin_R.dds
        ZipArchiveEntry r = archive.GetEntry( "Skin_R.dds" );

        if ( r != null )
        {
            Texture2D texture = LoadTexture( r );

            if ( texture != null )
            {
                material.Roughness = texture;
            }
        }
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

    private static void LoadWheelMaterial( CarMaterial material, ZipArchive archive )
    {
        //Wheels_AO.dds
        ZipArchiveEntry ao = archive.GetEntry( "Wheels_AO.dds" );

        if ( ao != null )
        {
            Texture2D texture = LoadTexture( ao );

            if ( texture != null )
            {
                material.AmbientOcclusion = texture;
            }
        }

        //Wheels_B.dds
        ZipArchiveEntry b = archive.GetEntry( "Wheels_B.dds" );

        if ( b != null )
        {
            Texture2D texture = LoadTexture( b );

            if ( texture != null )
            {
                material.Albedo = texture;
            }
        }

        //Wheels_DirtMask.dds
        ZipArchiveEntry dirtMask = archive.GetEntry( "Wheels_DirtMask.dds" );

        if ( dirtMask != null )
        {
            Texture2D texture = LoadTexture( dirtMask );

            if ( texture != null )
            {
                material.DirtMask = texture;
            }
        }

        //Wheels_I.dds
        ZipArchiveEntry i = archive.GetEntry( "Wheels_I.dds" );

        if ( i != null )
        {
            Texture2D texture = LoadTexture( i );

            if ( texture != null )
            {
                material.Emissive = texture;
            }
        }

        //Wheels_N.dds
        ZipArchiveEntry n = archive.GetEntry( "Wheels_N.dds" );

        if ( n != null )
        {
            Texture2D texture = LoadTexture( n );

            if ( texture != null )
            {
                material.Normal = texture;
            }
        }

        //Wheels_R.dds
        ZipArchiveEntry r = archive.GetEntry( "Wheels_R.dds" );

        if ( r != null )
        {
            Texture2D texture = LoadTexture( r );

            if ( texture != null )
            {
                material.Roughness = texture;
            }
        }
    }

    #endregion

}

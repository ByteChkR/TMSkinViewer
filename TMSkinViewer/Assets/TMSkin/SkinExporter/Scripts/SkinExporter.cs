using System.IO;
using System.IO.Compression;

using UnityEngine;

public static class SkinExporter
{

    public static SkinExporterSettings Settings { get; set; }

    #region Public

    public static byte[] Export( CarSkin skin )
    {
        using MemoryStream ms = new MemoryStream();
        ZipArchive zip = new ZipArchive( ms, ZipArchiveMode.Create );
        AddDetailsMaterial( zip, skin.Details );
        AddGlassMaterial( zip, skin.Glass );
        AddSkinMaterial( zip, skin.Skin );
        AddWheelMaterial( zip, skin.Wheel );
        zip.Dispose();

        return ms.ToArray();
    }

    #endregion

    #region Private

    private static void AddDetailsMaterial( ZipArchive arch, CarMaterial mat )
    {
        AddTexture( arch, "Details_AO.dds", mat.AmbientOcclusion );
        AddTexture( arch, "Details_B.dds", mat.Albedo );
        AddTexture( arch, "Details_DirtMask.dds", mat.DirtMask );
        AddTexture( arch, "Details_I.dds", mat.Emissive );
        AddTexture( arch, "Details_N.dds", mat.Normal );
        AddTexture( arch, "Details_R.dds", mat.Roughness );
    }

    private static void AddGlassMaterial( ZipArchive arch, CarMaterial mat )
    {
        AddTexture( arch, "Glass_AO.dds", mat.AmbientOcclusion );
        AddTexture( arch, "Glass_D.dds", mat.Albedo );
        AddTexture( arch, "Glass_I.dds", mat.Emissive );
    }

    private static void AddSkinMaterial( ZipArchive arch, CarMaterial mat )
    {
        AddTexture( arch, "Skin_AO.dds", mat.AmbientOcclusion );
        AddTexture( arch, "Skin_B.dds", mat.Albedo );
        AddTexture( arch, "Skin_DirtMask.dds", mat.DirtMask );
        AddTexture( arch, "Skin_I.dds", mat.Emissive );
        AddTexture( arch, "Skin_R.dds", mat.Roughness );
    }

    private static void AddTexture( ZipArchive arch, string path, CarTexture tex )
    {
        if ( tex == null || tex.IsDefault && !Settings.ExportDefaultTextures )
        {
            return;
        }

        ZipArchiveEntry e = arch.CreateEntry( path );
        using Stream s = e.Open();

        s.Write( tex.TextureData, 0, tex.TextureData.Length );
    }

    private static void AddWheelMaterial( ZipArchive arch, CarMaterial mat )
    {
        AddTexture( arch, "Wheels_AO.dds", mat.AmbientOcclusion );
        AddTexture( arch, "Wheels_B.dds", mat.Albedo );
        AddTexture( arch, "Wheels_DirtMask.dds", mat.DirtMask );
        AddTexture( arch, "Wheels_I.dds", mat.Emissive );
        AddTexture( arch, "Wheels_N.dds", mat.Normal );
        AddTexture( arch, "Wheels_R.dds", mat.Roughness );
    }

    #endregion

}

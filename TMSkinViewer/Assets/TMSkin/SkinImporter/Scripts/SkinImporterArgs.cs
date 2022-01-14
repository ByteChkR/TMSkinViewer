using System;
using System.IO;

public readonly struct SkinImporterArgs : IDisposable
{

    public readonly CarSkin Skin;
    public readonly Stream Data;
    public readonly string ResourcePath;
    public readonly bool ImportAsDefault;

    public SkinImporterArgs( CarSkin skin, Stream data, string resourcePath = null, bool importAsDefault = false )
    {
        Skin = skin;
        Data = data;
        ResourcePath = resourcePath;
        ImportAsDefault = importAsDefault;
    }

    public void Dispose()
    {
        Data?.Dispose();
    }

}

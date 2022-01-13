using System;
using System.IO;

public readonly struct SkinImporterArgs : IDisposable
{

    public readonly CarSkin Skin;
    public readonly Stream Data;
    public readonly string ResourcePath;

    public SkinImporterArgs( CarSkin skin, Stream data, string resourcePath = null )
    {
        Skin = skin;
        Data = data;
        ResourcePath = resourcePath;
    }

    public void Dispose()
    {
        Data?.Dispose();
    }

}

using System;

[AttributeUsage( AttributeTargets.Class )]
public class SettingsCategoryAttribute : Attribute
{

    public readonly string Path;

    #region Public

    public SettingsCategoryAttribute( string path )
    {
        Path = path;
    }

    #endregion

}

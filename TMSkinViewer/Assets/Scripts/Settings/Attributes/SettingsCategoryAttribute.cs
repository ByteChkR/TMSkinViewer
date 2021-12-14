

using System;

[AttributeUsage(AttributeTargets.Class)]
public class SettingsCategoryAttribute : Attribute
{

    public readonly string Path;

    public SettingsCategoryAttribute( string path)
    {
        Path = path;
    }

}
using System;

public class SettingsPropertyAttribute : Attribute
{

    public readonly string Name;

    public SettingsPropertyAttribute( string name = null )
    {
        Name = name;
    }

}
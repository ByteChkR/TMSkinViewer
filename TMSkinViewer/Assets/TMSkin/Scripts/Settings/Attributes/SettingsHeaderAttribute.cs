using System;

[AttributeUsage( AttributeTargets.Property| AttributeTargets.Method )]
public class SettingsHeaderAttribute : Attribute
{

    public readonly string Name;

    #region Public

    public SettingsHeaderAttribute( string name )
    {
        Name = name;
    }

    #endregion

}
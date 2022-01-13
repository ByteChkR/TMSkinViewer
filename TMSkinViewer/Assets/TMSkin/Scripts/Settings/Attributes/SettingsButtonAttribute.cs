using System;

[AttributeUsage( AttributeTargets.Method )]
public class SettingsButtonAttribute : Attribute
{

    public readonly string Name;

    #region Public

    public SettingsButtonAttribute( string name = null )
    {
        Name = name;
    }

    #endregion

}

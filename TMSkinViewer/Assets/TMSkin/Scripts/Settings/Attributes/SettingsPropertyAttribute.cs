using System;

public class SettingsPropertyAttribute : Attribute
{

    public readonly string Name;

    #region Public

    public SettingsPropertyAttribute( string name = null )
    {
        Name = name;
    }

    #endregion

}

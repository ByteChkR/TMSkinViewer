using System;

[AttributeUsage( AttributeTargets.Property )]
public class SettingsTextAreaAttribute : Attribute
{

    public readonly float? Height;

    #region Public

    public SettingsTextAreaAttribute( float? height = null )
    {
        Height = height;
    }

    #endregion

}

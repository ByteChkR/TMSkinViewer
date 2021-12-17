using System;

[AttributeUsage( AttributeTargets.Property )]
public class SettingsTextAreaAttribute : Attribute
{

    #region Public
    
    public readonly float? Height;

    public SettingsTextAreaAttribute( float? height = null)
    {
        Height = height;
    }

    #endregion

}
using System;

[AttributeUsage( AttributeTargets.Property )]
public class SettingsRangeAttribute : Attribute
{

    public readonly float Min;
    public readonly float Max;

    #region Public

    public SettingsRangeAttribute( float min, float max )
    {
        if( min > max )
        {
            throw new ArgumentException( "Min cannot be greater than max" );
        }
        Min = min;
        Max = max;
    }

    #endregion

}
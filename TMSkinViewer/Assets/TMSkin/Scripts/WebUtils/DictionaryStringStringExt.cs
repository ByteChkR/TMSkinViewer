using System.Collections.Generic;
using System.Globalization;

public static class DictionaryStringStringExt
{

    #region Public

    public static double GetDouble( this Dictionary < string, string > aDict, string aKey, double aDefault )
    {
        string str;

        if ( !aDict.TryGetValue( aKey, out str ) )
        {
            return aDefault;
        }

        double val;

        if ( !double.TryParse( str, NumberStyles.Float, CultureInfo.InvariantCulture, out val ) )
        {
            return aDefault;
        }

        return val;
    }

    public static int GetInt( this Dictionary < string, string > aDict, string aKey, int aDefault )
    {
        string str;

        if ( !aDict.TryGetValue( aKey, out str ) )
        {
            return aDefault;
        }

        int val;

        if ( !int.TryParse( str, NumberStyles.Integer, CultureInfo.InvariantCulture, out val ) )
        {
            return aDefault;
        }

        return val;
    }

    #endregion

}

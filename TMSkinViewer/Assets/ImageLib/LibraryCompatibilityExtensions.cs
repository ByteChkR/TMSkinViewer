using System;
using System.Collections.Generic;

namespace CSharpImageLibrary
{

    

    public static class LibraryCompatibilityExtensions
    {

        #region Public

        public static bool Contains( this IEnumerable < string > e, string s, StringComparison comparison )
        {
            foreach ( string s1 in e )
            {
                if ( s.Equals( s1, comparison ) )
                {
                    return true;
                }
            }

            return false;
        }

        public static bool Contains( this string str, string s, StringComparison comparison )
        {
            return str.Contains( s );
        }

        #endregion

    }

}

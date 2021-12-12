using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class ResourceTypeParser
{

    private static readonly Dictionary < ResourceType, string[] > s_FileExtensions =
        new Dictionary < ResourceType, string[] > { { ResourceType.Texture, new[] { ".png" } } };

    #region Public

    public static ResourceType ParseFromFileExtension( string file )
    {
        string ext = Path.GetExtension( file );

        foreach ( KeyValuePair < ResourceType, string[] > extensions in s_FileExtensions )
        {
            if ( extensions.Value.Contains( ext ) )
            {
                return extensions.Key;
            }
        }

        return ResourceType.Unknown;
    }

    #endregion

}

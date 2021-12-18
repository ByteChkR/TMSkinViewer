/* * * * *
 * URLParameters.cs
 * ----------------
 *
 * This singleton script provides easy access to any URL components in a Web-build.
 * Since Unity is about to deprecate the old "Application.ExternalEval" api we can
 * no longer inject our javascript into the browser. We now need to use a jslib file
 * which holds the javascript part. This script, when imported in a project, will
 * automatically extract a jslib file into the Plugins folder named: "URLParameters.jslib".
 *
 * To get access to the various URI parts, you can simply access the static properties
 * of this class. You can additionally attach the script to a gameobject to provide test
 * data for in-editor testing, however this is not required. The available properties are
 * Protocol, Hostname, Port, Pathname, Search, Hash as well as Host, Origin and Href.
 * The meaning of those are explained in a comment inside the class. In addition i've also
 * added two methods: "GetSearchParameters()" and "GetHashParameters()" which will parse
 * the query/hash part into seperate key=value pairs. Note that the content of those values
 * are not "unescaped". If you have URL encoded strings, you may want to use
 * "Uri.UnescapeDataString" or something similar.
 *
 * Note that this file also comes with two Dictionary<string,string> extension methods
 * which makes it easier to get a double or int value from either the search parameters
 * or the hash/fragment part of the URI. You can just do:
 *
 * double d = URLParameters.GetSearchParameters().GetDouble("keyName", 42d);
 * 
 * This would return "123" for an URI like this:
 * https://my.hostname.net/path/file.html?keyName=123
 * If the parameter isn't present or can't be converted to a double, the default value (42)
 * is returned
 *
 * The MIT License (MIT)
 *
 * Copyright (c) 2012-2019 Markus G�bel (Bunny83)
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 *
 * * * * */
using System;
using System.Collections.Generic;
using System.IO;

using UnityEditor;

using UnityEngine;

public class URLParameters : MonoBehaviour
{

    /*
     * https://some.domain.com:port/path/to/resource/resourceName.html?name=Foobar&id=3#HashString
     * | #1 |  |     #2      | |#3||          #4                     ||      #5       ||    #6   |
     *         |        #7        |
     * |            #8            |
     * |                                          #9                                             |
     * #1: location.protocol  "https:"
     * #2: location.hostname  "some.domain.com"
     * #3: location.port      "port"
     * #4: location.pathname  "/path/to/resource/resourceName.html"
     * #5: location.search    "?name=Foobar&id=3"
     * #6: location.hash      "#HashString"
     * 
     * #7: location.host      "some.domain.com:port"
     * #8: location.origin    "https://some.domain.com:port"
     * #9: location.href      "*full URL*"
    **/

    [Serializable]
    public struct TestData
    {

        public string Protocol;
        public string Hostname;
        public string Port;
        public string Pathname;
        public string Search;
        public string Hash;

    }

    public TestData testData;

    public static string Protocol => location_protocol();

    public static string Hostname => location_hostname();

    public static string Port => location_port();

    public static string Pathname => location_pathname();

    public static string Search
    {
        get => location_search();
        set => location_set_search( value );
    }

    public static string Hash
    {
        get => location_hash();
        set => location_set_hash( value );
    }

    public static string Host => location_host();

    public static string Origin => location_origin();

    public static string Href => location_href();

    private static char[] m_SplitChars = new char[] { '&' };

    private static Dictionary < string, string > ParseURLParams( string aText )
    {
        if ( aText == null || aText.Length <= 1 )
        {
            return new Dictionary < string, string >();
        }

        // skip "?" / "#" and split parameters at "&"
        string[] parameters = aText.Substring( 1 ).Split( m_SplitChars );
        Dictionary < string, string > res = new Dictionary < string, string >( parameters.Length );

        foreach ( string p in parameters )
        {
            int pos = p.IndexOf( '=' );

            if ( pos > 0 )
            {
                res[p.Substring( 0, pos )] = p.Substring( pos + 1 );
            }
            else
            {
                res[p] = "";
            }
        }

        return res;
    }

    public static Dictionary < string, string > GetSearchParameters()
    {
        return ParseURLParams( Search );
    }

    public static Dictionary < string, string > GetHashParameters()
    {
        return ParseURLParams( Hash );
    }

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")] public static extern string location_protocol();
    [DllImport("__Internal")] public static extern string location_hostname();
    [DllImport("__Internal")] public static extern string location_port();
    [DllImport("__Internal")] public static extern string location_pathname();
    [DllImport("__Internal")] public static extern string location_search();
    [DllImport("__Internal")] public static extern string location_hash();
    [DllImport("__Internal")] public static extern string location_host();
    [DllImport("__Internal")] public static extern string location_origin();
    [DllImport("__Internal")] public static extern string location_href();

    [DllImport("__Internal")] public static extern void location_set_search(string aSearch);
    [DllImport("__Internal")] public static extern void location_set_hash(string aHash);

#else

    private static TestData m_Data;

    public static string location_protocol()
    {
        return m_Data.Protocol;
    }

    public static string location_hostname()
    {
        return m_Data.Hostname;
    }

    public static string location_port()
    {
        return m_Data.Port;
    }

    public static string location_pathname()
    {
        return m_Data.Pathname;
    }

    public static string location_search()
    {
        return m_Data.Search;
    }

    public static string location_hash()
    {
        return m_Data.Hash;
    }

    public static string location_host()
    {
        return m_Data.Hostname + ( string.IsNullOrEmpty( m_Data.Port ) ? "" : ":" + m_Data.Port );
    }

    public static string location_origin()
    {
        return m_Data.Protocol + "//" + location_host();
    }

    public static string location_href()
    {
        return location_origin() + m_Data.Pathname + m_Data.Search + m_Data.Hash;
    }

    public static void location_set_search( string aSearch )
    {
    }

    public static void location_set_hash( string aHash )
    {
    }

    public void Awake()
    {
        m_Data = testData;
    }

#if UNITY_EDITOR
    private static string m_EmbeddedLib = @"
var URLParamLib = {

    location_protocol: function()
    {
        var str = window.location.protocol;
        var bufferSize = lengthBytesUTF8(str) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(str, buffer, bufferSize);
        return buffer;
    },
    location_hostname: function()
    {
        var str = window.location.hostname;
        var bufferSize = lengthBytesUTF8(str) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(str, buffer, bufferSize);
        return buffer;
    },
    location_port: function()
    {
        var str = window.location.port;
        var bufferSize = lengthBytesUTF8(str) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(str, buffer, bufferSize);
        return buffer;
    },
    location_pathname: function()
    {
        var str = window.location.pathname;
        var bufferSize = lengthBytesUTF8(str) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(str, buffer, bufferSize);
        return buffer;
    },
    location_search: function()
    {
        var str = window.location.search;
        var bufferSize = lengthBytesUTF8(str) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(str, buffer, bufferSize);
        return buffer;
    },
    location_hash: function()
    {
        var str = window.location.hash;
        var bufferSize = lengthBytesUTF8(str) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(str, buffer, bufferSize);
        return buffer;
    },
    location_host: function()
    {
        var str = window.location.host;
        var bufferSize = lengthBytesUTF8(str) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(str, buffer, bufferSize);
        return buffer;
    },
    location_origin: function()
    {
        var str = window.location.origin;
        var bufferSize = lengthBytesUTF8(str) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(str, buffer, bufferSize);
        return buffer;
    },
    location_href: function()
    {
        var str = window.location.href;
        var bufferSize = lengthBytesUTF8(str) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(str, buffer, bufferSize);
        return buffer;
    },
    location_set_search: function(str)
    {
        window.location.search = Pointer_stringify(str);
    },
    location_set_hash: function(str)
    {
        window.location.hash = Pointer_stringify(str);
    },

};

mergeInto(LibraryManager.library, URLParamLib);
";

    [InitializeOnLoadMethod]
    private static void ExtractJSLib()
    {
        string path = Path.Combine( Application.dataPath, "Plugins" );
        DirectoryInfo folder = new DirectoryInfo( path );

        if ( !folder.Exists )
        {
            folder.Create();
        }

        path = Path.Combine( path, "URLParameters.jslib" );

        if ( File.Exists( path ) )
        {
            return;
        }

        Debug.Log( "URLParameters.jslib does not exist in the plugins folder, extracting" );
        File.WriteAllText( path, m_EmbeddedLib );
        AssetDatabase.Refresh();
        EditorApplication.RepaintProjectWindow();
    }
#endif

#endif

}
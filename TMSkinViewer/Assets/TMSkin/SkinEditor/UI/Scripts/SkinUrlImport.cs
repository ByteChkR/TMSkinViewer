using System;
using System.Text;

using UnityEngine;

namespace UI.SkinEditorMainWindow
{
    [Serializable]
    public class SkinUrlImportContainer
    {

        public SkinUrlImport[] Items;

        public static string ToUrlArgument( SkinUrlImport[] items )
        {
            return Convert.ToBase64String( System.Text.Encoding.UTF8.GetBytes( ToJson( items ) ) );
        }
        public static SkinUrlImport[] FromUrlArgument( string arg )
        {
            return FromJson(Encoding.UTF8.GetString(Convert.FromBase64String(arg)));
        }
        public static string ToJson( SkinUrlImport[] items )
        {
            return JsonUtility.ToJson( new SkinUrlImportContainer { Items = items } );
        }
        
        public static SkinUrlImport[] FromJson( string json )
        {
            return JsonUtility.FromJson<SkinUrlImportContainer>( json ).Items;
        }
    }
    [Serializable]
    public class SkinUrlImport
    {

        
        public string Name;
        public string Url;
        
    }

}